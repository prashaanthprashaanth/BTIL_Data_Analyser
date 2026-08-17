from __future__ import annotations

import argparse
from pathlib import Path

from .decoder import decode_dataset, environment_rows
from .export import export_xlsx


def build_parser() -> argparse.ArgumentParser:
    parser = argparse.ArgumentParser(description="Decode MITRAC/Bombardier TDS ED_V event data")
    parser.add_argument("edv", help="ED_V binary file")
    parser.add_argument("--edd", required=True, help="matching ED_D .oti definition")
    parser.add_argument("--edt", help="optional ED_T .oti repair-text definition")
    parser.add_argument("--xlsx", help="write a complete XLSX report")
    parser.add_argument("--event", type=int, help="print environment data for a reference number")
    parser.add_argument("--time-shift", type=float, default=0.0, help="hours added to timestamps")
    return parser


def main(argv: list[str] | None = None) -> int:
    args = build_parser().parse_args(argv)
    dataset = decode_dataset(
        args.edv,
        args.edd,
        args.edt,
        shift_hours=args.time_shift,
    )
    header = dataset.edv.header
    print(f"File: {Path(args.edv).name}")
    print(f"Project: {header.project_name} {header.project_version}")
    print(f"Vehicle: {header.vehicle_name}; ODBS: {header.odbs_address}")
    print(
        f"Events: {len(dataset.edv.records)}; definitions: {dataset.mapped_event_count}; "
        f"environments: {dataset.environment_event_count}"
    )
    for warning in dataset.warnings:
        print(f"WARNING: {warning}")

    if args.event is not None:
        record = next(
            (item for item in dataset.edv.records if item.reference_number == args.event), None
        )
        if record is None:
            raise SystemExit(f"reference {args.event} was not found")
        label = record.definition.description if record.definition else "definition unavailable"
        print(f"\n{record.reference_number:05d}: {label}")
        for row in environment_rows(record):
            samples = ", ".join(
                f"{offset:+d}ms={value}" for offset, value in zip(row.offsets_ms, row.values)
            )
            print(f"{row.name} [{row.unit}]: {samples}")

    if args.xlsx:
        export_xlsx(dataset, args.xlsx, progress=print)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
