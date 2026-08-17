from __future__ import annotations

import math
import struct
from datetime import datetime, timedelta
from pathlib import Path

from .models import EDVFile, EDVHeader, EventRecord


class EDVFormatError(ValueError):
    pass


class _Reader:
    def __init__(self, data: bytes):
        self.data = data
        self.position = 0

    @property
    def remaining(self) -> int:
        return len(self.data) - self.position

    def read(self, size: int) -> bytes:
        if size < 0 or self.position + size > len(self.data):
            raise EOFError(f"wanted {size} bytes at offset 0x{self.position:X}")
        value = self.data[self.position : self.position + size]
        self.position += size
        return value

    def unpack(self, fmt: str):
        size = struct.calcsize(fmt)
        values = struct.unpack(fmt, self.read(size))
        return values[0] if len(values) == 1 else values

    def u8(self) -> int:
        return self.unpack("<B")

    def u16(self) -> int:
        return self.unpack("<H")

    def u32(self) -> int:
        return self.unpack("<I")

    def f32(self) -> float:
        return self.unpack("<f")

    def fixed_utf16(self, byte_count: int) -> str:
        return self.read(byte_count).decode("utf-16-le", errors="replace").split("\0", 1)[0]


def _event_time(seconds: int, relative: int, shift_hours: float, use_local_time: bool) -> datetime:
    if use_local_time:
        base = datetime.fromtimestamp(seconds)
    else:
        base = datetime(1970, 1, 1) + timedelta(seconds=seconds)
    # .NET Framework DateTime.AddMilliseconds rounds to a whole millisecond.
    milliseconds = math.floor((relative / 65535.0) * 1000.0 + 0.5)
    return base + timedelta(milliseconds=milliseconds, hours=shift_hours)


def read_edv(
    path: str | Path,
    *,
    use_local_time: bool = False,
    shift_hours: float = 0.0,
) -> EDVFile:
    source = Path(path)
    reader = _Reader(source.read_bytes())
    try:
        byte_order_control = reader.u16()
        header = EDVHeader(
            byte_order_control=byte_order_control,
            file_id=reader.fixed_utf16(16),
            prefix=reader.fixed_utf16(8),
            ed_version=reader.fixed_utf16(8),
            project_name=reader.fixed_utf16(16),
            project_version=reader.fixed_utf16(30),
            diagnosis_system=reader.fixed_utf16(32),
            odbs_address="",  # Filled below because its size depends on the protocol.
            vehicle_name="",
            readout_date="",
            readout_time="",
            event_copy="",
            property_options=b"",
        )
        if header.prefix != "ED_V":
            raise EDVFormatError(f"{source.name} is not an ED_V file (prefix={header.prefix!r})")
        if not header.ed_version.isdigit():
            raise EDVFormatError(f"invalid ED_V protocol version {header.ed_version!r}")
        header.odbs_address = reader.fixed_utf16(202 if header.is_extended else 6)
        header.vehicle_name = reader.fixed_utf16(32)
        header.readout_date = reader.fixed_utf16(16)
        header.readout_time = reader.fixed_utf16(12)
        header.event_copy = reader.fixed_utf16(2)
        header.property_options = reader.read(8)
    except EOFError as exc:
        raise EDVFormatError(f"truncated ED_V header: {exc}") from exc

    records: list[EventRecord] = []
    # TDS files carry a four-byte trailer that the original program consumed as
    # the beginning of a 551st record and then silently discarded at EOF.
    while reader.remaining > 4:
        offset = reader.position
        try:
            reference_number = reader.u16() | (reader.u16() << 16)
            event_id = reader.u16()
            limit = reader.u8()
            vehicle_position = reader.u8()
            process_id = reader.u8()
            subsystem_number = reader.u8()
            location = reader.u8()
            priority = reader.u8()
            error_codes = tuple(reader.u16() for _ in range(4))
            acknowledgements = tuple(reader.u8() for _ in range(4))
            error_code_mismatch = reader.u8()
            active = reader.u8()
            deleted = reader.u8()
            uploaded = reader.u8()
            event_count = reader.u16()
            start_seconds = reader.u32()
            start_relative = reader.u16()
            start_time = _event_time(start_seconds, start_relative, shift_hours, use_local_time)
            raw_end_seconds = reader.u32()
            end_relative = reader.u16()
            end_time = (
                None
                if raw_end_seconds in {0, 0xFFFFFFFF}
                else _event_time(raw_end_seconds, end_relative, shift_hours, use_local_time)
            )

            extended_values: list[float | int] = []
            if header.is_extended:
                extended_values.extend(reader.f32() for _ in range(5))
                extended_values.extend(reader.u32() for _ in range(7))

            env_meta = tuple(reader.u8() for _ in range(8))
            odbs_meta = tuple(reader.u8() for _ in range(8)) if header.is_extended else (0,) * 8

            environment_byte_count = 2 * (
                env_meta[0] * env_meta[1] + env_meta[4] * env_meta[5]
            )
            environment_data = reader.read(environment_byte_count)
            odbs_byte_count = 2 * (
                odbs_meta[0] * odbs_meta[1] + odbs_meta[4] * odbs_meta[5]
            )
            odbs_environment_data = reader.read(odbs_byte_count)
        except EOFError as exc:
            raise EDVFormatError(
                f"truncated event record {len(records) + 1} at offset 0x{offset:X}: {exc}"
            ) from exc

        latitude, longitude, altitude, speed, heading = (extended_values + [0.0] * 5)[:5]
        integers = (extended_values[5:] + [0] * 7)[:7]
        records.append(
            EventRecord(
                file_offset=offset,
                unique_reference=len(records) + 1,
                reference_number=reference_number,
                event_id=event_id,
                limit=limit,
                vehicle_position=vehicle_position,
                process_id=process_id,
                subsystem_number=subsystem_number,
                location=location,
                priority=priority,
                error_codes=error_codes,  # type: ignore[arg-type]
                acknowledgements=acknowledgements,  # type: ignore[arg-type]
                error_code_mismatch=error_code_mismatch,
                active=active,
                deleted=deleted,
                uploaded=uploaded,
                event_count=event_count,
                start_seconds=start_seconds,
                start_relative=start_relative,
                start_time=start_time,
                end_seconds=raw_end_seconds,
                end_relative=end_relative,
                end_time=end_time,
                latitude=float(latitude),
                longitude=float(longitude),
                altitude=float(altitude),
                speed=float(speed),
                heading=float(heading),
                utc_time=int(integers[0]),
                odometer=int(integers[1]),
                trip=int(integers[2]),
                reserved=tuple(int(value) for value in integers[3:7]),  # type: ignore[arg-type]
                environment_words_start=env_meta[0],
                environment_block_count_start=env_meta[1],
                environment_old_index_start=env_meta[2],
                environment_trigger_index_start=env_meta[3],
                environment_words_end=env_meta[4],
                environment_block_count_end=env_meta[5],
                environment_old_index_end=env_meta[6],
                environment_trigger_index_end=env_meta[7],
                odbs_words_start=odbs_meta[0],
                odbs_block_count_start=odbs_meta[1],
                odbs_old_index_start=odbs_meta[2],
                odbs_trigger_index_start=odbs_meta[3],
                odbs_words_end=odbs_meta[4],
                odbs_block_count_end=odbs_meta[5],
                odbs_old_index_end=odbs_meta[6],
                odbs_trigger_index_end=odbs_meta[7],
                environment_data=environment_data,
                odbs_environment_data=odbs_environment_data,
            )
        )

    trailer = reader.read(reader.remaining)
    if trailer and len(trailer) != 4:
        raise EDVFormatError(f"unexpected {len(trailer)}-byte trailer at 0x{reader.position:X}")
    return EDVFile(source_path=source, header=header, records=records, trailer=trailer)
