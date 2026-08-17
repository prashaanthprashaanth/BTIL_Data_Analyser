# BTIL Data Analyser

BTIL Data Analyser is a Windows desktop application and Python decoder for MITRAC TDS/VCU event files used in BTIL propulsion locomotives. It reads `ED_V` event data with `ED_D`/`ED_T` OTI definitions and presents the complete event history together with pre-trigger and post-trigger environment parameters.

## Main application

The maintained application is in [`tdsview-rebuilt`](tdsview-rebuilt/README.md).

To start it on Windows, double-click:

```text
tdsview-rebuilt\run_tdsview.bat
```

Main capabilities include:

- complete VCU event and fault decoding;
- environment samples from before and after each trigger;
- engineering-unit conversion using OTI scaling definitions;
- selectable environment rows and keyboard/keypad navigation;
- search, sorting, fault totals, repair details, and Excel export;
- explicit OTI revision and mapping warnings.

## Repository structure

- `tdsview-rebuilt/` — maintained Python/Tkinter viewer, decoder library, tests, and Excel exporter.
- `reverse-engineering/` — recovered format documentation, validation captures, and decompiled reference code.
- `Bombardier Transportation/` — original reference software and OTI definition package used during compatibility analysis.

## Test

From `tdsview-rebuilt`:

```powershell
py -3 -m unittest discover -s tests -v
```

Integration tests accept `TDS_SAMPLE_EDV` and `TDS_SAMPLE_EDD` environment variables pointing to a compatible sample and definition file.

## Confidentiality

This repository contains railway diagnostic definitions, original reference binaries, and decoded operational data. It is intended to remain private and does not declare an open-source license.
