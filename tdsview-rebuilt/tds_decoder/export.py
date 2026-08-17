from __future__ import annotations

from collections import Counter
from datetime import timedelta
from pathlib import Path
from typing import Callable, Iterable

from openpyxl import Workbook
from openpyxl.cell import WriteOnlyCell
from openpyxl.styles import Font, PatternFill
from openpyxl.utils import get_column_letter

from .decoder import environment_rows
from .models import DecodedDataset, EventRecord


EVENT_HEADERS = [
    "Ref Nr",
    "Start time",
    "End Time",
    "Duration",
    "Dist Text",
    "ECode 0",
    "ECode 0 Descr",
    "Event Name",
    "EnvBl Id",
    "Cnt",
    "Ev Id",
    "Limit",
    "Vh Pos",
    "Prc Id",
    "Subsys",
    "Subsys Desc",
    "Prio",
    "ECode 1",
    "ECode 1 Descr",
    "ECode 2",
    "ECode 2 Descr",
    "ECode 3",
    "ECode 3 Descr",
    "UniqueRef",
    "Vehicle Name",
    "User1",
    "User2",
    "User3",
    "User4",
    "User5",
    "Position",
    "Altitude",
    "Speed",
    "Heading",
    "UTC Time",
    "Odometer",
    "Trip",
    "rep_prio_active",
    "rep_prio_passive",
    "extra_prio",
    "extra attrib1",
    "extra attrib2",
    "extra attrib3",
    "extra attrib4",
    "extra attrib5",
]

HEADER_FILL = PatternFill("solid", fgColor="FFF200")
CHANGED_FILL = PatternFill("solid", fgColor="FFD700")
INFERRED_FILL = PatternFill("solid", fgColor="FFE6B3")
HEADER_FONT = Font(bold=True)


def _time(value) -> str:
    return value.strftime("%Y-%m-%d %H:%M:%S") if value else ""


def _duration(record: EventRecord) -> str:
    if record.end_time is None:
        return ""
    delta = record.end_time - record.start_time
    if delta.total_seconds() < 0:
        return "Negative!"
    days = delta.days
    seconds = delta.seconds
    prefix = f"{days}." if days else ""
    return f"{prefix}{seconds // 3600:02d}:{(seconds % 3600) // 60:02d}:{seconds % 60:02d}"


def _compact_float(value: float) -> str:
    if value == int(value):
        return str(int(value))
    return format(value, ".7g")


def event_export_row(dataset: DecodedDataset, record: EventRecord, source_index: int = 0) -> list[str]:
    definition = record.definition
    subsystem = dataset.definitions.subsystems.get(record.subsystem_number)
    descriptions = [code.description if code else "" for code in record.code_definitions]
    extra = definition.extra_attributes if definition else ("", "", "", "", "")
    return [
        f"{record.reference_number:05d}",
        _time(record.start_time),
        _time(record.end_time),
        _duration(record),
        definition.description if definition else "",
        f"{record.error_codes[0]:04X}",
        descriptions[0],
        definition.name if definition else "",
        definition.environment_block_id if definition else "",
        f"{record.event_count:05d}",
        f"{record.event_id:05d}",
        str(record.limit),
        str(record.vehicle_position),
        f"{record.process_id:03d}",
        subsystem.name if subsystem else str(record.subsystem_number),
        subsystem.description if subsystem else str(record.subsystem_number),
        str(record.priority),
        f"{record.error_codes[1]:04X}",
        descriptions[1],
        f"{record.error_codes[2]:04X}",
        descriptions[2],
        f"{record.error_codes[3]:04X}",
        descriptions[3],
        f"{source_index}:{record.unique_reference}",
        dataset.edv.header.vehicle_name,
        "",
        "",
        "",
        "",
        "",
        f"{record.latitude:.6f}/{record.longitude:.6f}",
        _compact_float(record.altitude),
        _compact_float(record.speed),
        _compact_float(record.heading),
        str(record.utc_time),
        str(record.odometer),
        str(record.trip),
        definition.rep_prio_active if definition else "",
        definition.rep_prio_passive if definition else "",
        definition.extra_prio if definition else "",
        *extra,
    ]


def _header_row(sheet, headers: Iterable[str]):
    cells = []
    for value in headers:
        cell = WriteOnlyCell(sheet, value=value)
        cell.fill = HEADER_FILL
        cell.font = HEADER_FONT
        cells.append(cell)
    sheet.append(cells)


def _set_widths(sheet, widths: list[float]):
    for index, width in enumerate(widths, 1):
        sheet.column_dimensions[get_column_letter(index)].width = width


def export_xlsx(
    dataset: DecodedDataset,
    path: str | Path,
    progress: Callable[[str], None] | None = None,
) -> Path:
    output = Path(path)
    workbook = Workbook(write_only=True)

    events_sheet = workbook.create_sheet("Event List")
    events_sheet.freeze_panes = "A2"
    _header_row(events_sheet, EVENT_HEADERS)
    for record in dataset.edv.records:
        events_sheet.append(event_export_row(dataset, record))
    events_sheet.auto_filter.ref = (
        f"A1:{get_column_letter(len(EVENT_HEADERS))}{len(dataset.edv.records) + 1}"
    )
    _set_widths(
        events_sheet,
        [9, 20, 20, 12, 48, 10, 20, 34, 24, 9, 9, 8, 8, 9, 12, 22, 8]
        + [12] * (len(EVENT_HEADERS) - 17),
    )
    if progress:
        progress("Event list written")

    max_samples = max(
        (record.environment_block_count_start for record in dataset.edv.records), default=0
    )
    environment_headers = [
        "Ref Nr",
        "Start Time",
        "Event Name",
        "Event Description",
        "Process",
        "Event Id",
        "Env Block",
        "Mapping",
        "Variable Name",
        "Description",
        "Unit",
    ]
    for sample in range(1, max_samples + 1):
        environment_headers.extend([f"Offset {sample} (ms)", f"Value {sample}"])

    environment_sheet = workbook.create_sheet("Environment Data")
    environment_sheet.freeze_panes = "A2"
    _header_row(environment_sheet, environment_headers)
    environment_row_count = 1
    for record_index, record in enumerate(dataset.edv.records, 1):
        event_name = (
            record.definition.name
            if record.definition
            else f"Unknown {record.process_id:02X}:{record.event_id:03X}"
        )
        event_description = record.definition.description if record.definition else ""
        block_name = record.environment_block.block_id if record.environment_block else ""
        for environment in environment_rows(record):
            values: list[object] = [
                f"{record.reference_number:05d}",
                _time(record.start_time),
                event_name,
                event_description,
                f"{record.process_id:02X}",
                f"{record.event_id:03X}",
                block_name,
                record.environment_mapping,
                environment.name,
                environment.description,
                environment.unit,
            ]
            cells = [WriteOnlyCell(environment_sheet, value=value) for value in values]
            if record.environment_mapping != "exact":
                for cell in cells:
                    cell.fill = INFERRED_FILL
            for sample in range(max_samples):
                if sample < len(environment.values):
                    offset_cell = WriteOnlyCell(
                        environment_sheet, value=environment.offsets_ms[sample]
                    )
                    value_cell = WriteOnlyCell(
                        environment_sheet, value=environment.values[sample]
                    )
                    if environment.changed[sample]:
                        value_cell.fill = CHANGED_FILL
                    cells.extend([offset_cell, value_cell])
                else:
                    cells.extend(
                        [WriteOnlyCell(environment_sheet, value=""), WriteOnlyCell(environment_sheet, value="")]
                    )
            environment_sheet.append(cells)
            environment_row_count += 1
        if progress and record_index % 50 == 0:
            progress(f"Environment data: {record_index}/{len(dataset.edv.records)} events")
    environment_sheet.auto_filter.ref = (
        f"A1:{get_column_letter(len(environment_headers))}{environment_row_count}"
    )
    _set_widths(
        environment_sheet,
        [9, 20, 32, 48, 9, 9, 24, 32, 30, 44, 10] + [14, 14] * max_samples,
    )

    summary_sheet = workbook.create_sheet("Fault Summary")
    _header_row(summary_sheet, ["Count", "Process", "Event Id", "Event Name", "Description"])
    counts = Counter((record.process_id, record.event_id) for record in dataset.edv.records)
    for (process_id, event_id), count in counts.most_common():
        definition = dataset.definitions.events.get((process_id, event_id))
        summary_sheet.append(
            [
                count,
                f"{process_id:02X}",
                f"{event_id:03X}",
                definition.name if definition else "",
                definition.description if definition else "Definition unavailable",
            ]
        )
    _set_widths(summary_sheet, [10, 10, 10, 38, 60])

    repairs_sheet = workbook.create_sheet("Repair Text")
    _header_row(repairs_sheet, ["Event Name", "Event Description", "Repair / Cause / Remedy"])
    used_names: set[str] = set()
    for record in dataset.edv.records:
        if not record.definition or record.definition.name in used_names:
            continue
        used_names.add(record.definition.name)
        details = dataset.texts.details_for(record.definition.name)
        repairs_sheet.append([record.definition.name, record.definition.description, details])
    _set_widths(repairs_sheet, [40, 60, 100])

    info_sheet = workbook.create_sheet("File Information")
    header = dataset.edv.header
    information = [
        ("ED_V file", str(dataset.edv.source_path)),
        ("File ID", header.file_id),
        ("ED protocol", header.ed_version),
        ("Project", header.project_name),
        ("ED_V project version", header.project_version),
        ("ED_D file", str(dataset.definitions.source_path)),
        ("ED_D project version", dataset.definitions.project_version),
        ("OTI schema version", dataset.definitions.schema_version),
        ("Vehicle", header.vehicle_name),
        ("ODBS address", header.odbs_address),
        ("Readout date", header.readout_date),
        ("Readout time", header.readout_time),
        ("Event records", len(dataset.edv.records)),
        ("Mapped event definitions", dataset.mapped_event_count),
        ("Decodable environments", dataset.environment_event_count),
        ("Four-byte trailer", dataset.edv.trailer.hex().upper()),
    ]
    _header_row(info_sheet, ["Field", "Value"])
    for key, value in information:
        info_sheet.append([key, value])
    for warning in dataset.warnings:
        info_sheet.append(["Warning", warning])
    _set_widths(info_sheet, [30, 110])

    output.parent.mkdir(parents=True, exist_ok=True)
    workbook.save(output)
    if progress:
        progress(f"Saved {output}")
    return output
