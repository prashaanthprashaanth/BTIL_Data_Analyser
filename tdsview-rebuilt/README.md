# TDS VCU Event & Environment Viewer

This is a maintainable replacement for Bombardier `TDSView.exe`. It decodes MITRAC TDS `ED_V` event files with their `ED_D`/`ED_T` OTI definitions and focuses on the complete pre-trigger/post-trigger environment data.

## Run the desktop viewer

Double-click `run_tdsview.bat`, then choose an `ED_V` file. The program searches the Desktop and Downloads folders for the closest compatible English ED_D OTI definition. The exact ED_D file in use and every version mismatch are shown at the top of the window.

You can also start with explicit files:

```powershell
py -3 app.py "C:\path\ED_V_file.9240" --edd "C:\path\ED_D_project_version_EN.oti"
```

The interface provides:

- the complete event list with search and sorting;
- the selected event's environment block and all ring-buffer samples;
- selectable environment-parameter rows with arrow-key, keypad, Page Up/Page Down, Home, and End navigation;
- blue highlighting when a value changes from the previous sample;
- raw process/event IDs and all four error codes;
- ED_T repair, cause, and remedy text;
- clear labels for exact versus inferred environment mappings;
- complete Excel export;
- standalone HTML report export for upload to an internal/OEM web server.

## Complete Excel export

Use **Export Complete Excel** in the viewer, or run:

```powershell
py -3 -m tds_decoder.cli "C:\path\ED_V_file.9240" `
  --edd "C:\path\ED_D_file.oti" `
  --xlsx "C:\path\complete-report.xlsx"
```

## Standalone HTML export

Use **Export HTML Report** in the viewer to create a single `.html` file containing the event list, all environment samples, fault summary, repair text, file information, OTI provenance, and warnings. The file has no external dependencies and can be uploaded to a web server for browser viewing.

Command line:

```powershell
py -3 -m tds_decoder.cli "C:\path\ED_V_file.9240" `
  --edd "C:\path\ED_D_file.oti" `
  --html "C:\path\complete-report.html"
```

Both the Excel workbook and HTML report contain:

1. **Event List** — the original 45-column fault/event table.
2. **Environment Data** — every variable and every time sample, with changed values highlighted.
3. **Fault Summary** — occurrence counts by raw process/event key.
4. **Repair Text** — ED_T repair/cause/remedy text for used events.
5. **File Information** — protocol, project, vehicle, OTI provenance, coverage, and warnings.

## Command-line inspection

Print one event's environment data without opening the GUI:

```powershell
py -3 -m tds_decoder.cli "C:\path\ED_V_file.9240" `
  --edd "C:\path\ED_D_file.oti" --event 253
```

Python 3.10+ and `openpyxl` are required. On the analysed machine, Python 3.14, Tk 8.6, and openpyxl 3.1.5 are already available.

## Definition-version rule

Event names, descriptions, environment layouts, and repair instructions come from OTI—not from the ED_V payload. An exact project revision is always preferred. If it is unavailable, the viewer can use a nearby revision, but it keeps the mismatch visible because labels can change between revisions.

See [the reverse-engineering report](../reverse-engineering/ANALYSIS.md) for the recovered file layout and original application behavior.
