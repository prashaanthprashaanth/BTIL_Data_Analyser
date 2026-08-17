from __future__ import annotations

import struct
from collections import defaultdict
from pathlib import Path

from .binary import read_edv
from .models import (
    DecodedDataset,
    EnvironmentBlock,
    EnvironmentRow,
    EnvironmentSignal,
    EventRecord,
    TextDefinitions,
)
from .oti import read_ed_d, read_ed_t, sibling_text_definition


def decode_environment_value(
    signal: EnvironmentSignal,
    block_offset: int,
    data: bytes,
    *,
    value_only: bool = True,
    hex_allowed: bool = True,
) -> str:
    offset = block_offset + signal.word * 2 - 2
    needed = 4 if signal.data_type in {"D", "F"} else 2
    if signal.data_type in {"B", "Y"}:
        needed = 1 + int(signal.data_type == "Y" and signal.byte_selector == "H")
    if offset < 0 or offset + needed > len(data):
        return "????"

    if signal.data_type == "B":
        bit = signal.bit or 0
        if bit >= 8:
            offset += 1
            bit -= 8
        enabled = bool(data[offset] & (1 << bit))
        return ("1" if enabled else "0") if value_only else ("ON" if enabled else "OFF")

    if signal.data_type == "Y":
        if signal.byte_selector == "H":
            offset += 1
        raw = data[offset]
        if signal.display_type == "O":
            return str(raw)
        if signal.display_type == "M":
            return str(struct.unpack("<b", bytes([raw]))[0])
        return f"{raw:02X}H" if hex_allowed else str(raw)

    if signal.data_type == "W":
        if signal.display_type in {"A", "M"}:
            raw_signed = struct.unpack_from("<h", data, offset)[0]
            if signal.display_type == "M":
                return str(raw_signed)
            scaled = raw_signed * signal.coefficient_a + signal.coefficient_b
            return f"{scaled:.{signal.decimals}f}"
        raw_unsigned = struct.unpack_from("<H", data, offset)[0]
        if signal.display_type == "O":
            return str(raw_unsigned)
        return f"{raw_unsigned:04X}" if hex_allowed else str(raw_unsigned)

    if signal.data_type == "D":
        raw_unsigned = struct.unpack_from("<I", data, offset)[0]
        raw_signed = struct.unpack_from("<i", data, offset)[0]
        if signal.display_type == "O":
            return str(raw_signed)
        if hex_allowed:
            return f"{raw_unsigned:016X}" if raw_signed < 0 else f"{raw_unsigned:X}".rjust(16, "0")
        return str(raw_signed)

    if signal.data_type == "F":
        return f"{struct.unpack_from('<f', data, offset)[0]:.2f}"
    return ""


def _ring_order(record: EventRecord) -> tuple[list[int], list[int]]:
    count = record.environment_block_count_start
    if count <= 0:
        return [], []
    order: list[int] = []
    index = record.environment_old_index_start
    for _ in range(count):
        order.append(index)
        index = (index + 1) % count
    try:
        trigger_position = order.index(record.environment_trigger_index_start)
    except ValueError:
        trigger_position = 0
    cycle_ms = (record.environment_block.cycle_time_us // 1000) if record.environment_block else 0
    offsets = [(position - trigger_position) * cycle_ms for position in range(count)]
    return order, offsets


def environment_rows(record: EventRecord) -> list[EnvironmentRow]:
    rows: list[EnvironmentRow] = []
    block = record.environment_block
    if block is not None:
        order, offsets = _ring_order(record)
        for signal in block.signals:
            values = [
                decode_environment_value(
                    signal,
                    record.environment_words_start * block_index * 2,
                    record.environment_data,
                )
                for block_index in order
            ]
            changed = [False] + [values[index] != values[index - 1] for index in range(1, len(values))]
            rows.append(
                EnvironmentRow(
                    name=signal.name,
                    description=signal.description,
                    unit=signal.unit,
                    offsets_ms=offsets.copy(),
                    values=values,
                    changed=changed,
                )
            )

    odbs = record.odbs_environment_block
    if odbs is not None and record.odbs_environment_data:
        _, offsets = _ring_order(record)
        trigger_position = offsets.index(0) if 0 in offsets else 0
        for signal in odbs.signals:
            values = [""] * len(offsets)
            if values:
                values[trigger_position] = decode_environment_value(
                    signal, 0, record.odbs_environment_data
                )
            rows.append(
                EnvironmentRow(
                    name=signal.name,
                    description=signal.description,
                    unit=signal.unit,
                    offsets_ms=offsets.copy(),
                    values=values,
                    changed=[False] * len(values),
                    source="odbs",
                )
            )
    return rows


def _infer_environment_block(
    record: EventRecord,
    process_blocks: dict[int, set[str]],
    blocks: dict[str, EnvironmentBlock],
) -> EnvironmentBlock | None:
    candidates = [
        blocks[block_id]
        for block_id in process_blocks.get(record.process_id, set())
        if block_id in blocks and blocks[block_id].word_count == record.environment_words_start
    ]
    if len(candidates) == 1:
        return candidates[0]
    size_matches = [
        block for block in blocks.values() if block.word_count == record.environment_words_start
    ]
    return size_matches[0] if len(size_matches) == 1 else None


def _odbs_block_for(record: EventRecord, dataset: DecodedDataset) -> EnvironmentBlock | None:
    pcl = dataset.definitions.pcls.get(record.process_id)
    if not pcl or not pcl.odbs_name:
        return None
    device = dataset.definitions.device_addresses.get(pcl.odbs_name)
    if not device or not device.environment_block_name:
        return None
    return dataset.definitions.environment_blocks.get(device.environment_block_name)


def decode_dataset(
    edv_path: str | Path,
    ed_d_path: str | Path,
    ed_t_path: str | Path | None = None,
    *,
    use_local_time: bool = False,
    shift_hours: float = 0.0,
) -> DecodedDataset:
    edv = read_edv(edv_path, use_local_time=use_local_time, shift_hours=shift_hours)
    definitions = read_ed_d(ed_d_path)
    text_path = Path(ed_t_path) if ed_t_path else sibling_text_definition(ed_d_path)
    texts = read_ed_t(text_path) if text_path and text_path.is_file() else TextDefinitions()
    dataset = DecodedDataset(edv=edv, definitions=definitions, texts=texts)

    process_blocks: dict[int, set[str]] = defaultdict(set)
    for definition in definitions.events.values():
        if definition.environment_block_id:
            process_blocks[definition.process_id].add(definition.environment_block_id)

    for record in edv.records:
        record.definition = definitions.events.get((record.process_id, record.event_id))
        record.code_definitions = tuple(
            definitions.code_tables[index].get(code)
            for index, code in enumerate(record.error_codes)
        )  # type: ignore[assignment]
        if record.definition:
            record.environment_block = definitions.environment_blocks.get(
                record.definition.environment_block_id
            )
            record.environment_mapping = "exact" if record.environment_block else "missing"
        elif record.environment_words_start:
            record.environment_block = _infer_environment_block(record, process_blocks, definitions.environment_blocks)
            if record.environment_block:
                record.environment_mapping = "inferred from process and payload size"
        record.odbs_environment_block = _odbs_block_for(record, dataset)

    if edv.header.project_name != definitions.project_name:
        dataset.warnings.append(
            f"project mismatch: ED_V={edv.header.project_name}, ED_D={definitions.project_name}"
        )
    if edv.header.project_version != definitions.project_version:
        dataset.warnings.append(
            f"definition revision mismatch: ED_V requests {edv.header.project_version}; "
            f"loaded ED_D is {definitions.project_version}"
        )
    missing_events = len(edv.records) - dataset.mapped_event_count
    if missing_events:
        combinations = sorted(
            {(record.process_id, record.event_id) for record in edv.records if not record.definition}
        )
        labels = ", ".join(f"{process:02X}:{event:03X}" for process, event in combinations)
        dataset.warnings.append(f"{missing_events} records have no event definition ({labels})")
    missing_environment = len(edv.records) - dataset.environment_event_count
    if missing_environment:
        dataset.warnings.append(f"{missing_environment} records have no decodable environment block")
    dataset.warnings.extend(definitions.parse_warnings)
    return dataset
