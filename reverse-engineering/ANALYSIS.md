# Bombardier TDSView end-to-end analysis

## Outcome

`TDSView.exe` is a .NET 2.0 Windows Forms application, not an encrypted or native black box. Its source-level control flow, file structures, OTI parsers, environment scaling rules, GUI behavior, and export paths were recovered. The readable decompilation is under `reverse-engineering/decompiled/`; the clean replacement is under `tdsview-rebuilt/`.

The diagnostic pipeline is:

```text
ED_V binary (raw event records + environment bytes)
        │
        ├── ED_D OTI (event IDs, fault text, environment layouts/scaling)
        └── ED_T OTI (repair, cause, remedy text)
                         │
                         ▼
          event table + selected environment trace
                         │
                         ├── UI / filtering / repair view
                         └── Excel, TSV, TXT, XML, STSNT
```

## Analysed components

| Component | Version | Role |
|---|---:|---|
| `TDSView.exe` | 2.0.0.0 | WinForms ED_V viewer/decoder, dated 25 April 2014 in its embedded revision history |
| `Tsv2Excel.exe` | 0.9.2.0 | Office-COM importer that converts tab-delimited TSV to XLSX |
| `ED_D_*.oti` | OTI schema 2.3.0.0 | Structural/event/environment definitions |
| `ED_T_*.oti` | OTI schema 2.3.0.0 | Repair/cause/remedy text |
| `CD_D_*.oti` | project-specific | Condition-data definitions; not used by TDSView's ED_V path |
| `CD_T_*.oti` | project-specific | Condition-data text container; nearly empty in this project |

The original executable contains separate classes for `ED_V`, `ED_D`, `ED_T`, event/code/environment models, the main form, Excel/TSV/TXT/XML exporters, STSNT export, mapping, packages, filtering, ranking, and raw view.

## Supplied ED_V file

| Field | Value |
|---|---|
| File | `ED_V_IR_PRP___20260810_044_A.9240` |
| File ID | `E89949C9` |
| ED protocol | `2000` (2.0.0.0) |
| Project | `IR_PRP` |
| Project revision | `001.005.004.019` |
| Diagnosis system | `TDS` |
| ODBS address | `044` |
| Vehicle | `IRPRP39240` |
| Readout | `2026-08-14 11:05:19` |
| Event records | 550 |
| Final four bytes | `A0 2B 38 4D` (ignored by the legacy reader; not standard CRC-32) |

## OTI meaning and version finding

OTI files are UTF-16, semicolon-delimited tagged text files. Quotes protect semicolons inside human-readable text. They are definitions, not vehicle log data.

### ED_D sections

| Section | Meaning |
|---|---|
| `META` | OTI file type and OTI schema version |
| `PROJECT` | project, project version, diagnosis system, flags, language |
| `DEV_ADDRESS` | ODBS device/address and optional global environment block |
| `PCL` | one-byte process ID to process name/device mapping |
| `SUBSYSTEM` | subsystem ID/name/description |
| `EVENT_LOCATION` | physical/vehicle event location definitions |
| `CODE_0` … `CODE_3` | four error-code dictionaries |
| `PRIORITY` | priority dictionary |
| `EVENT` | process ID + event ID to name, fault text, priorities, attributes, environment block, subsystem |
| `ENV_BLOCK` | environment block ID, description, acquisition cycle in microseconds |
| `ENV_SIGNAL` | word/bit location, primitive type, display type, scale, offset, unit, normal value, precision |

### ED_T sections

`REPAIR` maps an event name to repair instructions. Repeated `TEXT SCOPE="EVENT" TYPE="..."` sections supply cause, driver remedy, and other text. The join key is the ED_D event name.

### CD files

`CD_D` describes long-lived condition/counter values such as total distance, trip distance, recovered/consumed energy, wheel diameters, locomotive number, and locomotive type. The recovered TDSView executable has no CD_D/CD_T reader and does not use them to render ED_V events.

### Revisions found on this machine

| Revision | Event result for this ED_V | Observed role |
|---:|---:|---|
| `001.005.004.000` | 508/550 named; 550/550 environments recoverable | old workspace copy |
| `001.005.004.017` | 508/550 named; 550/550 environments recoverable | closest match to most labels in the supplied Excel |
| `001.005.004.020` | 550/550 named; 550/550 environments exact | matches screenshot labels such as `AFL Activated` and resolves event `0AC` |
| `001.005.004.019` | not found | exact revision requested by ED_V |

Revision `.020` resolves the 42 formerly blank events as:

- process `F5`, event `0AC`: `D_UAcLoFrDt_CON18` — `L:0913-Low Frequency Oscillations in line voltage detected`;
- process `F6`, event `0AC`: `D_UAcLoFrDt_CON28` — the converter-2 equivalent.

Without `.020`, their environment is still recoverable safely: both raw records contain 25 words × 7 samples, and F5/F6 uniquely map to `S_CvDgEnvGrL_1`/`S_CvDgEnvGrL_2`. The replacement labels this as inferred instead of inventing an event name.

The supplied Excel and screenshot were not generated with identical OTI content. The Excel uses older labels such as `Capacitor MR-blower not off`; the screenshot and `.020` use `AFL Activated`. This is why definition provenance must remain visible.

## ED_V binary layout

All numeric values are little-endian. Fixed strings are UTF-16LE byte arrays terminated/padded with NUL characters.

### Header (protocol 2.0.0.0)

| Offset | Bytes | Field |
|---:|---:|---|
| `0x000` | 2 | byte-order control (`FF FE` on disk) |
| `0x002` | 16 | file ID |
| `0x012` | 8 | prefix (`ED_V`) |
| `0x01A` | 8 | ED protocol (`2000`) |
| `0x022` | 16 | project name |
| `0x032` | 30 | project version |
| `0x050` | 32 | diagnosis system |
| `0x070` | 6 | ODBS address |
| `0x076` | 32 | vehicle name |
| `0x096` | 16 | readout date |
| `0x0A6` | 12 | readout time |
| `0x0B2` | 2 | event-copy flag |
| `0x0B4` | 8 | property/options bytes |

Header length is 188 bytes. In protocol 2.2.0.0 and newer, the ODBS field expands from 6 to 202 bytes, making the header 384 bytes.

### Event fixed fields (protocol 2.0.0.0)

| Record offset | Type | Field |
|---:|---|---|
| `0x00` | `uint32` | reference number, stored as two 16-bit halves |
| `0x04` | `uint16` | event ID |
| `0x06` … `0x0B` | six `byte`s | limit, vehicle position, process, subsystem, location, priority |
| `0x0C` … `0x13` | four `uint16`s | error codes 0–3 |
| `0x14` … `0x17` | four `byte`s | acknowledgement flags 0–3 |
| `0x18` … `0x1B` | four `byte`s | code mismatch, active, deleted, uploaded |
| `0x1C` | `uint16` | event occurrence count |
| `0x1E` | `uint32` | start Unix seconds |
| `0x22` | `uint16` | start fractional second (`value / 65535`) |
| `0x24` | `uint32` | end Unix seconds (`0`/`FFFFFFFF` means open/no end) |
| `0x28` | `uint16` | end fractional second |
| `0x2A` … `0x31` | eight `byte`s | environment start/end word counts, block counts, oldest and trigger ring indices |
| `0x32` | variable | environment payload |

Protocol 2.2.0.0 adds five 32-bit floats (latitude, longitude, altitude, speed, heading), UTC/odometer/trip, four reserved 32-bit values, and a second eight-byte metadata set for an ODBS/global environment payload. Its variable payload starts at record offset 106.

The normal payload byte count is:

```text
2 × ((start_words × start_blocks) + (end_words × end_blocks))
```

The same formula applies to the ODBS payload in protocol 2.2.0.0+.

## Environment decoding

For the common blocks in this file, OTI cycle time is `128000 µs`, so seven blocks represent:

```text
-512 ms, -384 ms, -256 ms, -128 ms, 0 ms, +128 ms, +256 ms
```

The raw samples are a circular buffer. Starting with `old_block_idx_start`, the viewer walks and wraps through `block_cnt_start` entries. The sample whose ring index equals `trx_block_idx_start` is 0 ms. Offset is:

```text
(display_position - trigger_position) × (cycle_time_us / 1000)
```

For a signal, the zero-based byte address within one sample block is:

```text
(hex ENV_WORD × 2) - 2
```

`ENV_BIT` selects a bit for `B`; `ENV_BYTE=H` selects the high byte for `Y`.

| `ENV_TYPE` | Raw representation | Display behavior |
|---|---|---|
| `B` | one bit in a 16-bit word | `0/1` in grids; optional `OFF/ON` |
| `Y` | selected byte | unsigned decimal (`O`), signed decimal (`M`), or hex (`H`) |
| `W` | 16-bit word | unsigned/signed/hex or scaled analog |
| `D` | 32-bit integer | decimal or 16-digit hex |
| `F` | IEEE-754 float32 | two decimals in the original viewer |

Scaled analog display (`DISP_TYPE=A`) is:

```text
display = signed_int16(raw) × coefficient_A + coefficient_B
```

`DISP_FORMAT` is the number of decimal places. Variables are sorted by physical bit position (`word × 16 + byte_offset + bit`). A cell is gold when its rendered value differs from the immediately preceding sample. The first/oldest sample is never highlighted.

Example recovered from reference 00521 (`S_CvDgEnvGrG1_1`):

| Signal | Unit | -512 | -384 | -256 | -128 | 0 | +128 | +256 |
|---|---|---:|---:|---:|---:|---:|---:|---:|
| `XU_Ln_CON11` | kV | 25.96 | 25.97 | 25.99 | 26.02 | 26.07 | 26.09 | 26.07 |
| `XI_Ln_CON11` | A | 5.86 | 4.15 | 3.17 | 3.42 | 7.57 | 6.84 | 6.59 |
| `XU_DcLk2_CON11` | V | 1849.12 | 1849.37 | 1849.37 | 1874.03 | 1917.97 | 1915.77 | 1914.55 |
| `XH_Co_CON11` | °C | 30.93 | 30.93 | 30.93 | 30.93 | 30.93 | 30.93 | 30.96 |

## Original application workflow

1. Read the ED_V header and all variable-length event records.
2. Construct the requested ED_D filename from project, exact project revision, and language.
3. Search a sibling `ED_D` folder, the ED_V folder, and the configured ED_D path.
4. If exact lookup fails in GUI mode, offer other languages/browsing. Batch mode returns error 3.
5. Parse ED_D; derive sibling ED_T by replacing `ED_D` with `ED_T`.
6. Join each raw event to ED_D on `(process_id, event_id)` and code tables on the four raw error codes.
7. Join the event to its `ENV_BLOCK`, then attach/sort every matching `ENV_SIGNAL`.
8. Populate the event grid. Selecting a row reconstructs the ring-buffer time columns and environment values.

Notable original features include multiple ED_V files, packages, raw view, five user-selected environment columns, filtering, search, ranking, printing, Google Maps/Earth, STSNT export, repair-text dialogs, and configuration import/export.

### Original command-line modes

- `/RAW <ED_V>` — raw tab-separated dump without ED_D.
- `/OP <ED_V> <ED_D>` — open the GUI with explicit data/definition files.
- `/TXT /L=EN <ED_V> <ED_D_directory> [/OUT=...]` — hierarchical text output.
- `/XML /L=EN <ED_V> <ED_D_directory> [/OUT=...]` — XML output.
- `/TSV /L=EN <ED_V> <ED_D_directory> [/FILT=event | /ENV] [/OUT=...]` — tab-separated output.

`Tsv2Excel.exe input.tsv [destination]` opens Microsoft Excel through Office COM, imports the TSV as text, autofits columns, and saves a copy as XLSX.

## Original weaknesses corrected in the replacement

- The original compares OTI schema version (`2.3.0.0`) to ED protocol (`2.0.0.0`) but does not reliably enforce the project revision (`1.5.4.019`). The replacement reports project mismatch explicitly.
- The original Excel export contains only visible event-grid columns; it does not export the seven environment samples. The replacement adds a complete environment sheet.
- Original Excel export depends on installed Microsoft Office COM and hard-codes the autofilter only through column `AK`, despite the 45-column table extending to `AS`.
- The original silently treats end-of-stream as successful termination and discards the four-byte file trailer. The replacement validates truncation and reports the trailer.
- Missing event definitions produce blank rows. The replacement displays raw process/event IDs and may infer an environment block only when process and payload size select a unique block.
- The original protocol-2.2 ODBS block selection is derived from the first event's process. The replacement resolves it per event.
- Definition provenance and exact/inferred status are visible in the replacement UI and workbook.

## Generated deliverables

- `reverse-engineering/decompiled/TDSView/` — decompiled original TDSView project.
- `reverse-engineering/decompiled/Tsv2Excel/` — decompiled TSV converter.
- `reverse-engineering/runtime-original-printwindow.png` — original executable running against the supplied file.
- `tdsview-rebuilt/` — clean Python decoder, GUI, CLI, tests, and documentation.
- `tdsview-rebuilt/output/39240_20260814_complete.xlsx` — complete `.020`-based report.

