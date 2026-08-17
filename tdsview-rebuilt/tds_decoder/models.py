from __future__ import annotations

from dataclasses import dataclass, field
from datetime import datetime
from pathlib import Path
from typing import Optional


@dataclass
class EDVHeader:
    byte_order_control: int
    file_id: str
    prefix: str
    ed_version: str
    project_name: str
    project_version: str
    diagnosis_system: str
    odbs_address: str
    vehicle_name: str
    readout_date: str
    readout_time: str
    event_copy: str
    property_options: bytes

    @property
    def protocol_version(self) -> int:
        return int(self.ed_version)

    @property
    def is_extended(self) -> bool:
        return self.protocol_version >= 2200


@dataclass
class CodeDefinition:
    value: int
    name: str = ""
    description: str = ""


@dataclass
class EventDefinition:
    process_id: int
    event_id: int
    name: str
    description: str
    rep_prio_active: str = ""
    rep_prio_passive: str = ""
    extra_prio: str = ""
    extra_attributes: tuple[str, str, str, str, str] = ("", "", "", "", "")
    environment_block_id: str = ""
    subsystem_id: str = ""


@dataclass
class EnvironmentSignal:
    block_id: str
    word: int
    bit: Optional[int]
    byte_selector: str
    data_type: str
    name: str
    description: str
    display_type: str
    coefficient_a: float
    coefficient_b: float
    unit: str
    normal_value: str
    decimals: int

    @property
    def sort_key(self) -> int:
        return self.word * 16 + (8 if self.byte_selector == "H" else 0) + (self.bit or 0)

    @property
    def word_span(self) -> int:
        return 2 if self.data_type in {"D", "F"} else 1


@dataclass
class EnvironmentBlock:
    block_id: str
    description: str
    cycle_time_us: int
    signals: list[EnvironmentSignal] = field(default_factory=list)

    @property
    def word_count(self) -> int:
        return max((signal.word + signal.word_span - 1 for signal in self.signals), default=0)


@dataclass
class PCLDefinition:
    process_id: int
    name: str
    description: str
    odbs_name: str = ""


@dataclass
class DeviceAddress:
    name: str
    address: str
    ip_address: str = ""
    description: str = ""
    environment_block_name: str = ""


@dataclass
class SubsystemDefinition:
    subsystem_id: int
    name: str
    description: str


@dataclass
class OtiDefinitions:
    source_path: Path
    meta_file_type: str = ""
    schema_version: str = ""
    project_name: str = ""
    project_version: str = ""
    diagnosis_system: str = ""
    language: str = ""
    project_description: str = ""
    events: dict[tuple[int, int], EventDefinition] = field(default_factory=dict)
    environment_blocks: dict[str, EnvironmentBlock] = field(default_factory=dict)
    code_tables: tuple[
        dict[int, CodeDefinition],
        dict[int, CodeDefinition],
        dict[int, CodeDefinition],
        dict[int, CodeDefinition],
    ] = field(default_factory=lambda: ({}, {}, {}, {}))
    priorities: dict[int, str] = field(default_factory=dict)
    subsystems: dict[int, SubsystemDefinition] = field(default_factory=dict)
    pcls: dict[int, PCLDefinition] = field(default_factory=dict)
    device_addresses: dict[str, DeviceAddress] = field(default_factory=dict)
    parse_warnings: list[str] = field(default_factory=list)


@dataclass
class TextDefinitions:
    source_path: Optional[Path] = None
    project_name: str = ""
    project_version: str = ""
    language: str = ""
    repairs: dict[str, str] = field(default_factory=dict)
    texts: dict[str, list[tuple[str, str]]] = field(default_factory=dict)

    def details_for(self, event_name: str) -> str:
        parts: list[str] = []
        repair = self.repairs.get(event_name, "").replace(r"\n", "\n")
        if repair:
            parts.append(f"REPAIR:\n{repair}")
        for text_type, value in self.texts.get(event_name, []):
            if value:
                rendered = value.replace(r"\n", "\n")
                parts.append(f"{text_type.upper()}:\n{rendered}")
        return "\n\n".join(parts)


@dataclass
class EventRecord:
    file_offset: int
    unique_reference: int
    reference_number: int
    event_id: int
    limit: int
    vehicle_position: int
    process_id: int
    subsystem_number: int
    location: int
    priority: int
    error_codes: tuple[int, int, int, int]
    acknowledgements: tuple[int, int, int, int]
    error_code_mismatch: int
    active: int
    deleted: int
    uploaded: int
    event_count: int
    start_seconds: int
    start_relative: int
    start_time: datetime
    end_seconds: int
    end_relative: int
    end_time: Optional[datetime]
    latitude: float = 0.0
    longitude: float = 0.0
    altitude: float = 0.0
    speed: float = 0.0
    heading: float = 0.0
    utc_time: int = 0
    odometer: int = 0
    trip: int = 0
    reserved: tuple[int, int, int, int] = (0, 0, 0, 0)
    environment_words_start: int = 0
    environment_block_count_start: int = 0
    environment_old_index_start: int = 0
    environment_trigger_index_start: int = 0
    environment_words_end: int = 0
    environment_block_count_end: int = 0
    environment_old_index_end: int = 0
    environment_trigger_index_end: int = 0
    odbs_words_start: int = 0
    odbs_block_count_start: int = 0
    odbs_old_index_start: int = 0
    odbs_trigger_index_start: int = 0
    odbs_words_end: int = 0
    odbs_block_count_end: int = 0
    odbs_old_index_end: int = 0
    odbs_trigger_index_end: int = 0
    environment_data: bytes = b""
    odbs_environment_data: bytes = b""
    definition: Optional[EventDefinition] = None
    environment_block: Optional[EnvironmentBlock] = None
    environment_mapping: str = "missing"
    odbs_environment_block: Optional[EnvironmentBlock] = None
    code_definitions: tuple[
        Optional[CodeDefinition],
        Optional[CodeDefinition],
        Optional[CodeDefinition],
        Optional[CodeDefinition],
    ] = (None, None, None, None)


@dataclass
class EDVFile:
    source_path: Path
    header: EDVHeader
    records: list[EventRecord]
    trailer: bytes = b""


@dataclass
class EnvironmentRow:
    name: str
    description: str
    unit: str
    offsets_ms: list[int]
    values: list[str]
    changed: list[bool]
    source: str = "event"


@dataclass
class DecodedDataset:
    edv: EDVFile
    definitions: OtiDefinitions
    texts: TextDefinitions = field(default_factory=TextDefinitions)
    warnings: list[str] = field(default_factory=list)

    @property
    def mapped_event_count(self) -> int:
        return sum(record.definition is not None for record in self.edv.records)

    @property
    def environment_event_count(self) -> int:
        return sum(record.environment_block is not None for record in self.edv.records)
