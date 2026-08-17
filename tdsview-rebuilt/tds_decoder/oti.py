from __future__ import annotations

import re
from pathlib import Path

from .models import (
    CodeDefinition,
    DeviceAddress,
    EDVHeader,
    EnvironmentBlock,
    EnvironmentSignal,
    EventDefinition,
    OtiDefinitions,
    PCLDefinition,
    SubsystemDefinition,
    TextDefinitions,
)


class OTIFormatError(ValueError):
    pass


def _read_oti_text(path: Path) -> str:
    data = path.read_bytes()
    if data.startswith((b"\xff\xfe", b"\xfe\xff")):
        return data.decode("utf-16")
    if data.startswith(b"\xef\xbb\xbf"):
        return data.decode("utf-8-sig")
    try:
        return data.decode("utf-8")
    except UnicodeDecodeError:
        return data.decode("iso-8859-1")


def split_oti_line(line: str) -> list[str]:
    """Match the original viewer's quote-aware semicolon parser."""
    result: list[str] = []
    value: list[str] = []
    quoted = False
    for character in line:
        if character == '"':
            quoted = not quoted
        elif character == ";" and not quoted:
            result.append("".join(value))
            value.clear()
        else:
            value.append(character)
    result.append("".join(value))
    return result


def _hex(value: str, default: int = 0) -> int:
    return int(value, 16) if value else default


def _float(value: str, default: float = 0.0) -> float:
    try:
        return float(value)
    except (TypeError, ValueError):
        return default


def _int(value: str, default: int = 0) -> int:
    try:
        return int(value)
    except (TypeError, ValueError):
        return default


def _sections(path: Path):
    section = ""
    section_tag = ""
    for line_number, raw_line in enumerate(_read_oti_text(path).splitlines(), 1):
        line = raw_line.strip()
        if not line:
            continue
        if line.startswith("<") and line.endswith(">"):
            section_tag = line
            section = line[1:-1].split(None, 1)[0]
            continue
        yield section, section_tag, line_number, split_oti_line(line)


def read_ed_d(path: str | Path) -> OtiDefinitions:
    source = Path(path)
    definitions = OtiDefinitions(source_path=source)
    pending_signals: list[EnvironmentSignal] = []

    for section, _tag, line_number, fields in _sections(source):
        try:
            if section == "META" and len(fields) >= 2:
                definitions.meta_file_type = fields[0]
                definitions.schema_version = fields[1]
            elif section == "PROJECT" and len(fields) >= 7:
                definitions.project_name = fields[0]
                definitions.project_version = fields[1]
                definitions.diagnosis_system = fields[2]
                definitions.language = fields[5]
                definitions.project_description = fields[6]
            elif section == "DEV_ADDRESS" and len(fields) in {3, 5}:
                device = DeviceAddress(
                    name=fields[0],
                    address=fields[1],
                    ip_address=fields[2] if len(fields) == 5 else "",
                    description=fields[3] if len(fields) == 5 else fields[2],
                    environment_block_name=fields[4] if len(fields) == 5 else "",
                )
                definitions.device_addresses[device.name] = device
            elif section == "PCL" and len(fields) in {3, 4}:
                pcl = PCLDefinition(
                    process_id=_hex(fields[0]),
                    name=fields[1],
                    description=fields[2],
                    odbs_name=fields[3] if len(fields) == 4 else "",
                )
                definitions.pcls[pcl.process_id] = pcl
            elif section == "SUBSYSTEM" and len(fields) >= 3:
                subsystem = SubsystemDefinition(_hex(fields[0]), fields[1], fields[2])
                definitions.subsystems[subsystem.subsystem_id] = subsystem
            elif section in {"CODE_0", "CODE_1", "CODE_2", "CODE_3"} and len(fields) >= 3:
                index = int(section[-1])
                code = CodeDefinition(_hex(fields[0]), fields[1], fields[2])
                # The legacy viewer performs a linear search and uses the first
                # duplicate definition in file order.
                definitions.code_tables[index].setdefault(code.value, code)
            elif section == "PRIORITY" and len(fields) >= 2:
                definitions.priorities[_hex(fields[0])] = fields[1]
            elif section == "EVENT" and len(fields) >= 10:
                if len(fields) == 10:
                    extra = (fields[7], fields[8], "", "", "")
                    environment_block_id = fields[9]
                    subsystem_id = ""
                elif len(fields) == 11:
                    extra = (fields[7], fields[8], "", "", "")
                    environment_block_id = fields[9]
                    subsystem_id = fields[10]
                else:
                    extra = tuple((fields[7:12] + [""] * 5)[:5])
                    environment_block_id = fields[12] if len(fields) > 12 else ""
                    subsystem_id = fields[13] if len(fields) > 13 else ""
                event = EventDefinition(
                    process_id=_hex(fields[0]),
                    event_id=_hex(fields[1]),
                    name=fields[2],
                    description=fields[3],
                    rep_prio_active=fields[4],
                    rep_prio_passive=fields[5],
                    extra_prio=fields[6],
                    extra_attributes=extra,  # type: ignore[arg-type]
                    environment_block_id=environment_block_id,
                    subsystem_id=subsystem_id,
                )
                definitions.events.setdefault((event.process_id, event.event_id), event)
            elif section == "ENV_BLOCK" and len(fields) >= 3:
                block = EnvironmentBlock(fields[0], fields[1], _int(fields[2]))
                definitions.environment_blocks[block.block_id] = block
            elif section == "ENV_SIGNAL" and len(fields) >= 13:
                signal = EnvironmentSignal(
                    block_id=fields[0],
                    word=_hex(fields[1]),
                    bit=_hex(fields[2]) if fields[2] else None,
                    byte_selector=fields[3],
                    data_type=fields[4],
                    name=fields[5],
                    description=fields[6],
                    display_type=fields[7],
                    coefficient_a=_float(fields[8], 1.0),
                    coefficient_b=_float(fields[9]),
                    unit=fields[10],
                    normal_value=fields[11],
                    decimals=_int(fields[12]),
                )
                pending_signals.append(signal)
        except (IndexError, TypeError, ValueError) as exc:
            definitions.parse_warnings.append(f"line {line_number}: {exc}")

    if definitions.meta_file_type and definitions.meta_file_type != "OTI_ED_D":
        raise OTIFormatError(f"{source.name} is {definitions.meta_file_type}, not OTI_ED_D")
    if not definitions.events or not definitions.environment_blocks:
        raise OTIFormatError(f"{source.name} does not contain usable ED_D definitions")

    for signal in pending_signals:
        block = definitions.environment_blocks.get(signal.block_id)
        if block is None:
            definitions.parse_warnings.append(
                f"environment signal {signal.name} references missing block {signal.block_id}"
            )
            continue
        block.signals.append(signal)
    for block in definitions.environment_blocks.values():
        block.signals.sort(key=lambda signal: signal.sort_key)
    return definitions


_TEXT_PARAMETERS = re.compile(r'(SCOPE|TYPE)="([^"]*)"')


def read_ed_t(path: str | Path) -> TextDefinitions:
    source = Path(path)
    texts = TextDefinitions(source_path=source)
    for section, tag, _line_number, fields in _sections(source):
        if section == "PROJECT" and len(fields) >= 3:
            texts.project_name = fields[0]
            texts.project_version = fields[1]
            texts.language = fields[2]
        elif section == "REPAIR" and len(fields) >= 4:
            texts.repairs[fields[0]] = fields[3]
        elif section == "TEXT" and len(fields) >= 4:
            parameters = dict(_TEXT_PARAMETERS.findall(tag))
            texts.texts.setdefault(fields[0], []).append((parameters.get("TYPE", "text"), fields[3]))
    return texts


def expected_definition_filename(header: EDVHeader, language: str = "EN") -> str:
    project = (header.project_name + "________")[:8]
    version = header.project_version.replace(".", "_")
    return f"ED_D_{project}_{version}_{language.upper()}.oti"


def find_definition(
    header: EDVHeader,
    search_directory: str | Path,
    language: str = "EN",
) -> tuple[Path | None, bool]:
    directory = Path(search_directory)
    exact = directory / expected_definition_filename(header, language)
    if exact.is_file():
        return exact, True

    project = (header.project_name + "________")[:8]
    version_parts = header.project_version.split(".")
    family = "_".join(version_parts[:3])
    candidates = sorted(directory.rglob(f"ED_D_{project}_{family}_*_{language.upper()}.oti"))
    if not candidates:
        candidates = sorted(directory.rglob(f"ED_D_{project}_*_{language.upper()}.oti"))
    if not candidates:
        return None, False

    def score(path: Path) -> tuple[int, tuple[int, ...]]:
        match = re.search(r"_(\d{3})_(\d{3})_(\d{3})_(\d{3})_[A-Z]{2}\.oti$", path.name)
        parts = tuple(int(value) for value in match.groups()) if match else (0, 0, 0, 0)
        same_family = int(parts[:3] == tuple(int(value) for value in version_parts[:3]))
        return same_family, parts

    return max(candidates, key=score), False


def sibling_text_definition(ed_d_path: str | Path) -> Path | None:
    path = Path(ed_d_path)
    candidate = path.with_name(path.name.replace("ED_D", "ED_T", 1))
    return candidate if candidate.is_file() else None
