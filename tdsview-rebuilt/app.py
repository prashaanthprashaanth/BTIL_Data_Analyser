from __future__ import annotations

import argparse
from collections import Counter
from datetime import datetime, time
import queue
import threading
import tkinter as tk
from pathlib import Path
from tkinter import filedialog, messagebox, ttk

from tds_decoder import decode_dataset, environment_rows, find_definition, read_edv
from tds_decoder.export import export_html, export_xlsx


DEFAULT_EDD_PATH = Path(
    r"F:\DEE RS ED Backup\OTI_FIles for BT Prop\1415\OTI Files\ED_D_IR_PRP___001_005_004_020_EN.oti"
)


def matching_edt_path(edd_path: Path) -> Path | None:
    sibling = edd_path.with_name(edd_path.name.replace("ED_D", "ED_T", 1))
    return sibling if sibling.is_file() else None


class EnvironmentGrid(ttk.Frame):
    BLUE = "#0B5CAB"
    DARK_BLUE = "#063B73"
    HEADER = BLUE
    ALT_ROW = "#EAF3FF"
    GRID = "#9CBFE5"
    CHANGED = "#B7D8FF"

    def __init__(self, parent):
        super().__init__(parent)
        self.rows = []
        self.selected_index: int | None = None
        self.row_height = 28
        self.total_width = 0
        self.content_height = 0
        self.header_canvas = tk.Canvas(
            self, background=self.HEADER, height=28, highlightthickness=0
        )
        self.canvas = tk.Canvas(
            self,
            background="white",
            highlightthickness=1,
            highlightbackground=self.GRID,
            highlightcolor=self.BLUE,
            takefocus=True,
            cursor="hand2",
        )
        self.vscroll = ttk.Scrollbar(self, orient="vertical", command=self.canvas.yview)
        self.hscroll = ttk.Scrollbar(self, orient="horizontal", command=self._xview)
        self.canvas.configure(yscrollcommand=self.vscroll.set, xscrollcommand=self._xscroll_changed)
        self.header_canvas.grid(row=0, column=0, sticky="ew")
        self.canvas.grid(row=1, column=0, sticky="nsew")
        self.vscroll.grid(row=1, column=1, sticky="ns")
        self.hscroll.grid(row=2, column=0, sticky="ew")
        self.rowconfigure(1, weight=1)
        self.columnconfigure(0, weight=1)
        self.canvas.bind("<MouseWheel>", self._wheel)
        self.canvas.bind("<Shift-MouseWheel>", self._horizontal_wheel)
        self.header_canvas.bind("<Shift-MouseWheel>", self._horizontal_wheel)
        self.canvas.bind("<Button-1>", self._click_row)
        self.canvas.bind("<Up>", lambda _event: self._move_selection(-1))
        self.canvas.bind("<Down>", lambda _event: self._move_selection(1))
        self.canvas.bind("<Prior>", lambda _event: self._page_selection(-1))
        self.canvas.bind("<Next>", lambda _event: self._page_selection(1))
        self.canvas.bind("<Home>", lambda _event: self._select_index(0))
        self.canvas.bind("<End>", lambda _event: self._select_index(len(self.rows) - 1))
        self.canvas.bind("<Left>", lambda _event: self._keyboard_xscroll(-1))
        self.canvas.bind("<Right>", lambda _event: self._keyboard_xscroll(1))
        self.canvas.bind("<KP_Up>", lambda _event: self._move_selection(-1))
        self.canvas.bind("<KP_Down>", lambda _event: self._move_selection(1))
        self.canvas.bind("<KP_Prior>", lambda _event: self._page_selection(-1))
        self.canvas.bind("<KP_Next>", lambda _event: self._page_selection(1))
        self.canvas.bind("<KP_Home>", lambda _event: self._select_index(0))
        self.canvas.bind("<KP_End>", lambda _event: self._select_index(len(self.rows) - 1))
        self.canvas.bind("<KP_Left>", lambda _event: self._keyboard_xscroll(-1))
        self.canvas.bind("<KP_Right>", lambda _event: self._keyboard_xscroll(1))
        self.canvas.bind("<KP_8>", lambda _event: self._move_selection(-1))
        self.canvas.bind("<KP_2>", lambda _event: self._move_selection(1))
        self.canvas.bind("<KP_9>", lambda _event: self._page_selection(-1))
        self.canvas.bind("<KP_3>", lambda _event: self._page_selection(1))
        self.canvas.bind("<KP_7>", lambda _event: self._select_index(0))
        self.canvas.bind("<KP_1>", lambda _event: self._select_index(len(self.rows) - 1))
        self.canvas.bind("<KP_4>", lambda _event: self._keyboard_xscroll(-1))
        self.canvas.bind("<KP_6>", lambda _event: self._keyboard_xscroll(1))

    def _xview(self, *args):
        self.canvas.xview(*args)
        self.header_canvas.xview(*args)

    def _xscroll_changed(self, first, last):
        self.hscroll.set(first, last)
        self.header_canvas.xview_moveto(first)

    def _wheel(self, event):
        self.canvas.yview_scroll(-1 if event.delta > 0 else 1, "units")

    def _horizontal_wheel(self, event):
        self.canvas.xview_scroll(-1 if event.delta > 0 else 1, "units")

    def _keyboard_xscroll(self, direction):
        self._xview("scroll", direction, "units")
        return "break"

    def _click_row(self, event):
        self.canvas.focus_set()
        if not self.rows:
            return "break"
        row_index = int(self.canvas.canvasy(event.y) // self.row_height)
        self._select_index(row_index)
        return "break"

    def _move_selection(self, direction):
        if not self.rows:
            return "break"
        current = self.selected_index if self.selected_index is not None else 0
        self._select_index(current + direction)
        return "break"

    def _page_selection(self, direction):
        if not self.rows:
            return "break"
        visible_rows = max(1, self.canvas.winfo_height() // self.row_height - 1)
        current = self.selected_index if self.selected_index is not None else 0
        self._select_index(current + (direction * visible_rows))
        return "break"

    def _select_index(self, row_index):
        if not self.rows:
            return "break"
        self.selected_index = max(0, min(int(row_index), len(self.rows) - 1))
        self.canvas.delete("selection")
        top = self.selected_index * self.row_height
        self.canvas.create_rectangle(
            2,
            top + 2,
            max(3, self.total_width - 2),
            top + self.row_height - 2,
            outline=self.BLUE,
            width=3,
            tags=("selection",),
        )
        self.canvas.tag_raise("selection")
        self._ensure_selection_visible()
        return "break"

    def _ensure_selection_visible(self):
        if self.selected_index is None or self.content_height <= 0:
            return
        top = self.selected_index * self.row_height
        bottom = top + self.row_height
        visible_top = self.canvas.canvasy(0)
        visible_bottom = self.canvas.canvasy(self.canvas.winfo_height())
        if top < visible_top:
            self.canvas.yview_moveto(top / self.content_height)
        elif bottom > visible_bottom:
            target = max(0, bottom - self.canvas.winfo_height())
            self.canvas.yview_moveto(target / self.content_height)

    def clear(self, message: str = "Select an event to view its environment data"):
        self.rows = []
        self.selected_index = None
        self.total_width = 0
        self.content_height = 0
        self.header_canvas.delete("all")
        self.canvas.delete("all")
        self.canvas.create_text(24, 24, text=message, anchor="nw", fill=self.DARK_BLUE, font=("Segoe UI", 10))
        self.canvas.configure(scrollregion=(0, 0, 800, 100))

    def render(self, rows):
        self.header_canvas.delete("all")
        self.canvas.delete("all")
        if not rows:
            self.clear("No environment block can be decoded for this event")
            return
        self.rows = list(rows)
        self.selected_index = None
        offsets = self.rows[0].offsets_ms
        headers = ["Variable Name", "Description", "Unit"] + [f"{value}ms" for value in offsets]
        # Environment values are the primary diagnostic view, so keep the
        # metadata columns compact enough to expose all seven time samples.
        widths = [140, 175, 40] + [48] * len(offsets)
        row_height = self.row_height
        total_width = sum(widths)
        total_height = row_height * (len(self.rows) + 1)
        self.total_width = total_width
        self.content_height = row_height * len(self.rows)

        x = 0
        for index, (header, width) in enumerate(zip(headers, widths)):
            self.header_canvas.create_rectangle(x, 0, x + width, row_height, fill=self.HEADER, outline=self.GRID)
            self.header_canvas.create_text(
                x + width / 2,
                row_height / 2,
                text=header,
                anchor="center",
                fill="white",
                font=("Segoe UI", 9, "bold"),
            )
            x += width

        for row_index, row in enumerate(self.rows):
            base = self.ALT_ROW if row_index % 2 == 1 else "white"
            values = [row.name, row.description, row.unit] + row.values
            x = 0
            for column, (value, width) in enumerate(zip(values, widths)):
                fill = base
                if column >= 3 and column - 3 < len(row.changed) and row.changed[column - 3]:
                    fill = self.CHANGED
                y = row_index * row_height
                self.canvas.create_rectangle(x, y, x + width, y + row_height, fill=fill, outline=self.GRID)
                self.canvas.create_text(
                    x + (width / 2 if column >= 2 else 5),
                    y + row_height / 2,
                    text=value,
                    anchor="center" if column >= 2 else "w",
                    width=width - 10,
                    fill=self.DARK_BLUE,
                    font=("Segoe UI", 9),
                )
                x += width
        self.header_canvas.configure(scrollregion=(0, 0, total_width, row_height))
        self.canvas.configure(scrollregion=(0, 0, total_width, total_height - row_height))
        self.header_canvas.xview_moveto(0)
        self.canvas.xview_moveto(0)
        self.canvas.yview_moveto(0)
        if self.rows:
            self._select_index(0)


class TDSViewApplication:
    EVENT_COLUMNS = (
        "reference",
        "start",
        "end",
        "duration",
        "description",
        "code0",
        "event",
    )

    def __init__(self, root: tk.Tk):
        self.root = root
        self.root.title("TDS VCU Event & Environment Viewer")
        self.root.geometry("1500x820")
        self.root.minsize(1050, 600)
        self.root.configure(background="white")
        self.dataset = None
        self.edv_path: Path | None = None
        self.edd_path: Path | None = DEFAULT_EDD_PATH if DEFAULT_EDD_PATH.is_file() else None
        self.edt_path: Path | None = matching_edt_path(self.edd_path) if self.edd_path else None
        self.visible_records = []
        self.record_by_iid = {}
        self.messages: queue.Queue = queue.Queue()
        self.sort_column = "reference"
        self.sort_reverse = False
        self.depth_dialog_open = False
        self.depth_selected_fault_keys: list[tuple[int, int]] = []
        self.depth_selected_parameter_keys: list[tuple[str, str, str]] = []
        self.depth_start_time: datetime | None = None
        self.depth_end_time: datetime | None = None
        self.depth_row_records = {}
        self._build_ui()
        self.root.after_idle(self._maximize_window)
        self.root.after(100, self._poll_messages)

    def _build_ui(self):
        blue = "#0B5CAB"
        dark_blue = "#063B73"
        light_blue = "#EAF3FF"
        mid_blue = "#B7D8FF"
        style = ttk.Style()
        if "clam" in style.theme_names():
            style.theme_use("clam")
        style.configure("TFrame", background="white")
        style.configure("TLabel", background="white", foreground=dark_blue)
        style.configure(
            "TButton",
            background=blue,
            foreground="white",
            bordercolor=dark_blue,
            focusthickness=1,
            focuscolor=mid_blue,
            padding=(8, 5),
        )
        style.map(
            "TButton",
            background=[("active", dark_blue), ("pressed", dark_blue), ("disabled", mid_blue)],
            foreground=[("disabled", "white")],
        )
        style.configure(
            "TEntry",
            fieldbackground="white",
            foreground=dark_blue,
            bordercolor=blue,
            lightcolor=blue,
            darkcolor=blue,
        )
        style.configure(
            "Treeview",
            rowheight=24,
            font=("Segoe UI", 9),
            background="white",
            fieldbackground="white",
            foreground=dark_blue,
            bordercolor=mid_blue,
        )
        style.map("Treeview", background=[("selected", blue)], foreground=[("selected", "white")])
        style.configure(
            "Treeview.Heading",
            font=("Segoe UI", 9, "bold"),
            background=blue,
            foreground="white",
            relief="flat",
            anchor="center",
        )
        style.map("Treeview.Heading", background=[("active", dark_blue)])
        style.configure("TPanedwindow", background=mid_blue)
        style.configure("TScrollbar", background=blue, troughcolor=light_blue, bordercolor="white", arrowcolor="white")
        style.configure("TNotebook", background="white", bordercolor=mid_blue)
        style.configure("TNotebook.Tab", padding=(14, 6), font=("Segoe UI", 10, "bold"))

        title = ttk.Frame(self.root, padding=(10, 7, 10, 2))
        title.pack(fill="x")
        ttk.Label(
            title,
            text="MITRAC TDS / VCU Event Decoder",
            font=("Segoe UI", 16, "bold"),
            anchor="center",
        ).pack(fill="x")

        toolbar = ttk.Frame(self.root, padding=(10, 3, 10, 6))
        toolbar.pack(fill="x")
        toolbar_buttons = ttk.Frame(toolbar)
        toolbar_buttons.pack(anchor="center")
        ttk.Button(toolbar_buttons, text="Open ED_V", command=self.open_edv).pack(side="left", padx=4)
        ttk.Button(toolbar_buttons, text="Select ED_D OTI", command=self.select_oti).pack(side="left", padx=4)
        self.export_button = ttk.Button(toolbar_buttons, text="Export Complete Excel", command=self.export_excel, state="disabled")
        self.export_button.pack(side="left", padx=4)
        self.html_export_button = ttk.Button(
            toolbar_buttons,
            text="Export HTML Report",
            command=self.export_html_report,
            state="disabled",
        )
        self.html_export_button.pack(side="left", padx=4)
        self.details_button = ttk.Button(toolbar_buttons, text="Event Details / Repair", command=self.show_details, state="disabled")
        self.details_button.pack(side="left", padx=4)

        self.notebook = ttk.Notebook(self.root)
        self.notebook.pack(fill="both", expand=True, padx=10)
        self.viewer_tab = ttk.Frame(self.notebook)
        self.depth_tab = ttk.Frame(self.notebook)
        self.notebook.add(self.viewer_tab, text="Event Viewer")
        self.notebook.add(self.depth_tab, text="Depth Analysis")
        self.notebook.bind("<<NotebookTabChanged>>", self._tab_changed)

        self.coverage_text = tk.StringVar(value="TOTAL FAULT COUNT: 0")
        ttk.Label(
            self.viewer_tab,
            textvariable=self.coverage_text,
            font=("Segoe UI", 11, "bold"),
            anchor="center",
            padding=(4, 2, 4, 5),
        ).pack(fill="x")

        path_frame = ttk.Frame(self.viewer_tab, padding=(0, 0, 0, 6))
        path_frame.pack(fill="x")
        initial_path_text = (
            f"Default ED_D: {self.edd_path}"
            if self.edd_path
            else "Open an ED_V file and select an ED_D OTI definition"
        )
        self.path_text = tk.StringVar(value=initial_path_text)
        ttk.Label(path_frame, textvariable=self.path_text, anchor="center").pack(fill="x")
        self.warning_text = tk.StringVar()
        self.warning_label = tk.Label(
            path_frame,
            textvariable=self.warning_text,
            anchor="center",
            justify="center",
            background=light_blue,
            foreground=dark_blue,
            padx=6,
            pady=4,
        )

        pane = ttk.Panedwindow(self.viewer_tab, orient="horizontal")
        self.main_pane = pane
        pane.pack(fill="both", expand=True)
        left = ttk.Frame(pane)
        right = ttk.Frame(pane)
        pane.add(left, weight=4)
        pane.add(right, weight=5)

        search_bar = ttk.Frame(left, padding=(0, 0, 4, 5))
        search_bar.pack(fill="x")
        ttk.Label(search_bar, text="Filter events:").pack(side="left")
        self.search = tk.StringVar()
        search_entry = ttk.Entry(search_bar, textvariable=self.search)
        search_entry.pack(side="left", fill="x", expand=True, padx=(6, 6))
        self.search.trace_add("write", lambda *_: self._apply_filter())

        tree_frame = ttk.Frame(left)
        tree_frame.pack(fill="both", expand=True)
        self.event_tree = ttk.Treeview(
            tree_frame,
            columns=self.EVENT_COLUMNS,
            show="headings",
            selectmode="browse",
        )
        labels = {
            "reference": "Ref Nr",
            "start": "Start time",
            "end": "End time",
            "duration": "Duration",
            "description": "Dist Text",
            "code0": "ECode 0",
            "event": "Event Name",
        }
        widths = {
            "reference": 62,
            "start": 126,
            "end": 126,
            "duration": 78,
            "description": 340,
            "code0": 72,
            "event": 190,
        }
        for column in self.EVENT_COLUMNS:
            self.event_tree.heading(
                column,
                text=labels[column],
                anchor="center",
                command=lambda selected=column: self._sort(selected),
            )
            self.event_tree.column(
                column,
                width=widths[column],
                minwidth=55,
                anchor="center" if column != "description" else "w",
                stretch=column in {"description", "event"},
            )
        tree_y = ttk.Scrollbar(tree_frame, orient="vertical", command=self.event_tree.yview)
        tree_x = ttk.Scrollbar(tree_frame, orient="horizontal", command=self.event_tree.xview)
        self.event_tree.configure(yscrollcommand=tree_y.set, xscrollcommand=tree_x.set)
        self.event_tree.grid(row=0, column=0, sticky="nsew")
        tree_y.grid(row=0, column=1, sticky="ns")
        tree_x.grid(row=1, column=0, sticky="ew")
        tree_frame.rowconfigure(0, weight=1)
        tree_frame.columnconfigure(0, weight=1)
        self.event_tree.tag_configure("alternate", background=light_blue)
        self.event_tree.tag_configure("unknown", background=mid_blue)
        self.event_tree.bind("<<TreeviewSelect>>", self._selected)
        self.event_tree.bind("<Double-1>", lambda _event: self.show_details())

        self.selected_text = tk.StringVar(value="Environment Data")
        ttk.Label(
            right,
            textvariable=self.selected_text,
            font=("Segoe UI", 11, "bold"),
            anchor="center",
            padding=(6, 3),
        ).pack(fill="x")
        legend = ttk.Frame(right, padding=(6, 0, 6, 4))
        legend.pack(fill="x")
        tk.Label(
            legend,
            text="  changed from previous sample  ",
            background=mid_blue,
            foreground=dark_blue,
        ).pack(side="left")
        ttk.Label(legend, text="  Samples are displayed oldest → trigger (0 ms) → newest").pack(side="left")
        ttk.Label(
            right,
            text="Select a parameter row, then use ↑  ↓  Page Up  Page Down  Home  End",
            anchor="center",
            padding=(4, 0, 4, 4),
        ).pack(fill="x")
        self.environment_grid = EnvironmentGrid(right)
        self.environment_grid.pack(fill="both", expand=True)
        self.environment_grid.clear()

        self._build_depth_analysis_tab()

        status = ttk.Frame(self.root, padding=(10, 5))
        status.pack(fill="x")
        self.status_text = tk.StringVar(value="Ready")
        ttk.Label(status, textvariable=self.status_text).pack(side="left")
        self.metadata_text = tk.StringVar()
        ttk.Label(status, textvariable=self.metadata_text).pack(side="right")
        self.root.after(400, self._set_initial_split)

    def _build_depth_analysis_tab(self):
        header = ttk.Frame(self.depth_tab, padding=(0, 6, 0, 6))
        header.pack(fill="x")
        self.depth_status_text = tk.StringVar(value="Select faults and environment parameters to build depth analysis")
        ttk.Label(
            header,
            textvariable=self.depth_status_text,
            font=("Segoe UI", 11, "bold"),
        ).pack(side="left")
        self.depth_select_button = ttk.Button(
            header,
            text="Select Faults / Parameters",
            command=self.open_depth_analysis_dialog,
            state="disabled",
        )
        self.depth_select_button.pack(side="right")

        table_frame = ttk.Frame(self.depth_tab)
        table_frame.pack(fill="both", expand=True)
        self.depth_table = ttk.Treeview(table_frame, show="headings", selectmode="browse")
        depth_y = ttk.Scrollbar(table_frame, orient="vertical", command=self.depth_table.yview)
        depth_x = ttk.Scrollbar(table_frame, orient="horizontal", command=self.depth_table.xview)
        self.depth_table.configure(yscrollcommand=depth_y.set, xscrollcommand=depth_x.set)
        self.depth_table.grid(row=0, column=0, sticky="nsew")
        depth_y.grid(row=0, column=1, sticky="ns")
        depth_x.grid(row=1, column=0, sticky="ew")
        table_frame.rowconfigure(0, weight=1)
        table_frame.columnconfigure(0, weight=1)
        self.depth_table.tag_configure("alternate", background="#EAF3FF")
        self._clear_depth_analysis_table()

    def _clear_depth_analysis_table(self):
        self.depth_row_records.clear()
        self.depth_table.delete(*self.depth_table.get_children())
        self.depth_table.configure(columns=("date_time", "fault"))
        self.depth_table.heading("date_time", text="Date Time")
        self.depth_table.heading("fault", text="Fault")
        self.depth_table.column("date_time", width=165, minwidth=120, anchor="center")
        self.depth_table.column("fault", width=420, minwidth=220, anchor="w")

    def _tab_changed(self, _event=None):
        if not hasattr(self, "notebook") or self.notebook.select() != str(self.depth_tab):
            return
        if not self.dataset:
            messagebox.showwarning("Open ED_V first", "Open an ED_V file before using Depth Analysis.")
            self.notebook.select(self.viewer_tab)
            return
        if not self.depth_selected_fault_keys or not self.depth_selected_parameter_keys:
            self.root.after(100, self.open_depth_analysis_dialog)

    @staticmethod
    def _fault_key(record):
        return (record.process_id, record.event_id)

    def _fault_label(self, key: tuple[int, int], count: int | None = None) -> str:
        process_id, event_id = key
        definition = self.dataset.definitions.events.get(key) if self.dataset else None
        prefix = f"{count:05d} | " if count is not None else ""
        name = definition.name if definition else "Definition unavailable"
        description = definition.description if definition else ""
        return f"{prefix}{process_id:02X}:{event_id:03X} | {name} | {description}"

    @staticmethod
    def _parameter_key(row):
        return (row.name, row.description, row.unit)

    @staticmethod
    def _parameter_heading(key: tuple[str, str, str]) -> str:
        name, _description, unit = key
        return f"{name} [{unit}]" if unit else name

    @staticmethod
    def _trigger_value(row) -> str:
        if 0 in row.offsets_ms:
            return row.values[row.offsets_ms.index(0)]
        return next((value for value in row.values if value != ""), "")

    @staticmethod
    def _parse_depth_datetime(value: str, *, end_of_day: bool = False) -> datetime | None:
        text = value.strip()
        if not text:
            return None
        formats = [
            "%Y-%m-%d %H:%M:%S",
            "%Y-%m-%d %H:%M",
            "%Y-%m-%d",
            "%d-%m-%Y %H:%M:%S",
            "%d-%m-%Y %H:%M",
            "%d-%m-%Y",
        ]
        for pattern in formats:
            try:
                parsed = datetime.strptime(text, pattern)
                if pattern in {"%Y-%m-%d", "%d-%m-%Y"}:
                    return datetime.combine(parsed.date(), time.max if end_of_day else time.min)
                return parsed
            except ValueError:
                pass
        raise ValueError("Use YYYY-MM-DD HH:MM:SS or DD-MM-YYYY HH:MM:SS")

    def _depth_date_default_text(self) -> tuple[str, str]:
        if not self.dataset or not self.dataset.edv.records:
            return "", ""
        starts = [record.start_time for record in self.dataset.edv.records if record.start_time]
        if not starts:
            return "", ""
        return min(starts).strftime("%Y-%m-%d %H:%M:%S"), max(starts).strftime("%Y-%m-%d %H:%M:%S")

    def _depth_filtered_records(
        self,
        start_time: datetime | None,
        end_time: datetime | None,
        fault_keys: set[tuple[int, int]] | None = None,
    ):
        if not self.dataset:
            return []
        records = []
        for record in self.dataset.edv.records:
            if start_time and record.start_time < start_time:
                continue
            if end_time and record.start_time > end_time:
                continue
            if fault_keys is not None and self._fault_key(record) not in fault_keys:
                continue
            records.append(record)
        return records

    def _depth_fault_choices(self, start_time: datetime | None, end_time: datetime | None):
        counts = Counter(self._fault_key(record) for record in self._depth_filtered_records(start_time, end_time))
        choices = []
        for key, count in counts.items():
            label = self._fault_label(key, count)
            choices.append(
                {
                    "key": key,
                    "count": count,
                    "label": label,
                    "search": label.lower(),
                }
            )
        choices.sort(key=lambda item: (-item["count"], item["key"][0], item["key"][1]))
        return choices

    def _depth_parameter_choices(
        self,
        fault_keys: set[tuple[int, int]],
        start_time: datetime | None,
        end_time: datetime | None,
    ):
        counts = Counter()
        for record in self._depth_filtered_records(start_time, end_time, fault_keys):
            for row in environment_rows(record):
                counts[self._parameter_key(row)] += 1
        choices = []
        for key, count in counts.items():
            name, description, unit = key
            label = self._parameter_heading(key)
            choices.append(
                {
                    "key": key,
                    "count": count,
                    "label": label,
                    "name": name,
                    "description": description,
                    "unit": unit,
                    "search": f"{label} {description}".lower(),
                }
            )
        choices.sort(key=lambda item: (-item["count"], item["name"], item["description"], item["unit"]))
        return choices

    def _refresh_depth_analysis_table(self):
        self._clear_depth_analysis_table()
        if not self.dataset or not self.depth_selected_fault_keys or not self.depth_selected_parameter_keys:
            self.depth_status_text.set("Select faults and environment parameters to build depth analysis")
            return

        fault_keys = set(self.depth_selected_fault_keys)
        records = sorted(
            self._depth_filtered_records(self.depth_start_time, self.depth_end_time, fault_keys),
            key=lambda record: (record.start_time, record.reference_number),
        )
        columns = ["date_time", "fault"] + [f"param_{index}" for index, _key in enumerate(self.depth_selected_parameter_keys)]
        self.depth_table.configure(columns=columns)
        self.depth_table.heading("date_time", text="Date Time")
        self.depth_table.heading("fault", text="Fault")
        self.depth_table.column("date_time", width=165, minwidth=125, anchor="center")
        self.depth_table.column("fault", width=420, minwidth=240, anchor="w")
        for index, parameter_key in enumerate(self.depth_selected_parameter_keys):
            column = f"param_{index}"
            heading = self._parameter_heading(parameter_key)
            self.depth_table.heading(column, text=heading)
            self.depth_table.column(column, width=max(120, min(260, len(heading) * 8)), minwidth=90, anchor="center")

        for row_index, record in enumerate(records):
            definition = record.definition
            fault_label = (
                f"{record.process_id:02X}:{record.event_id:03X} | {definition.name if definition else 'Definition unavailable'}"
            )
            environment_by_key = {self._parameter_key(row): row for row in environment_rows(record)}
            values = [record.start_time.strftime("%Y-%m-%d %H:%M:%S"), fault_label]
            for parameter_key in self.depth_selected_parameter_keys:
                row = environment_by_key.get(parameter_key)
                values.append(self._trigger_value(row) if row else "")
            iid = f"depth-{row_index}"
            self.depth_row_records[iid] = record
            self.depth_table.insert(
                "",
                "end",
                iid=iid,
                values=values,
                tags=("alternate",) if row_index % 2 else (),
            )

        date_range = ""
        if self.depth_start_time or self.depth_end_time:
            start = self.depth_start_time.strftime("%Y-%m-%d %H:%M:%S") if self.depth_start_time else "start"
            end = self.depth_end_time.strftime("%Y-%m-%d %H:%M:%S") if self.depth_end_time else "end"
            date_range = f" | {start} to {end}"
        self.depth_status_text.set(
            f"Depth Analysis: {len(records)} records, {len(self.depth_selected_fault_keys)} faults, "
            f"{len(self.depth_selected_parameter_keys)} parameters{date_range}"
        )

    def _set_initial_split(self):
        width = self.main_pane.winfo_width()
        if width > 100:
            # Keep the fault table visible through the complete Dist Text
            # column, while reserving room for the environment time samples.
            left_width = max(745, width - 710)
            left_width = min(left_width, int(width * 0.58))
            self.main_pane.sashpos(0, left_width)

    def _maximize_window(self):
        """Use the available screen so the diagnostic tables start unclipped."""
        try:
            self.root.state("zoomed")
        except tk.TclError:
            screen_width = self.root.winfo_screenwidth()
            screen_height = self.root.winfo_screenheight()
            self.root.geometry(f"{screen_width}x{screen_height}+0+0")
        self.root.after(250, self._set_initial_split)

    @staticmethod
    def _duration(record):
        if record.end_time is None:
            return ""
        delta = record.end_time - record.start_time
        if delta.total_seconds() < 0:
            return "Negative!"
        seconds = delta.seconds
        prefix = f"{delta.days}." if delta.days else ""
        return f"{prefix}{seconds // 3600:02d}:{(seconds % 3600) // 60:02d}:{seconds % 60:02d}"

    def select_oti(self):
        chosen = filedialog.askopenfilename(
            title="Select ED_D OTI definition",
            filetypes=[("ED_D OTI definitions", "ED_D*.oti"), ("OTI files", "*.oti"), ("All files", "*.*")],
        )
        if not chosen:
            return
        self.edd_path = Path(chosen)
        self.edt_path = matching_edt_path(self.edd_path)
        if self.edv_path:
            self._start_load()
        else:
            self.path_text.set(f"ED_D: {self.edd_path}")

    def open_edv(self):
        chosen = filedialog.askopenfilename(
            title="Open ED_V VCU event file",
            filetypes=[("ED_V files", "ED_V*.*"), ("All files", "*.*")],
        )
        if not chosen:
            return
        self.load_paths(Path(chosen), self.edd_path)

    def load_paths(self, edv_path: Path, edd_path: Path | None = None, edt_path: Path | None = None):
        self.edv_path = edv_path
        if edd_path:
            self.edd_path = edd_path
            self.edt_path = edt_path if edt_path else matching_edt_path(self.edd_path)
        if not self.edd_path:
            self.status_text.set("Reading ED_V header and locating the nearest OTI definition...")
            self.root.update_idletasks()
            try:
                header = read_edv(self.edv_path).header
                search_roots = [
                    self.edv_path.parent,
                    Path.home() / "Desktop",
                    Path.home() / "Downloads",
                ]
                for search_root in search_roots:
                    if not search_root.exists():
                        continue
                    found, _exact = find_definition(header, search_root)
                    if found:
                        self.edd_path = found
                        break
            except Exception as exc:
                messagebox.showerror("Could not read ED_V", str(exc))
                return
        if not self.edd_path:
            messagebox.showwarning(
                "ED_D definition required",
                "No compatible ED_D OTI file was found. Select the correct ED_D definition.",
            )
            self.select_oti()
            return
        if edt_path:
            self.edt_path = edt_path
        elif not self.edt_path:
            self.edt_path = matching_edt_path(self.edd_path)
        self._start_load()

    def _start_load(self):
        if not self.edv_path or not self.edd_path:
            return
        self.status_text.set("Decoding ED_V records and environment data...")
        self.export_button.configure(state="disabled")
        self.html_export_button.configure(state="disabled")
        self.details_button.configure(state="disabled")
        self.depth_select_button.configure(state="disabled")

        def worker():
            try:
                dataset = decode_dataset(self.edv_path, self.edd_path, self.edt_path)
                self.messages.put(("loaded", dataset))
            except Exception as exc:
                self.messages.put(("error", exc))

        threading.Thread(target=worker, daemon=True).start()

    def _poll_messages(self):
        try:
            while True:
                kind, payload = self.messages.get_nowait()
                if kind == "loaded":
                    self._loaded(payload)
                elif kind == "error":
                    self.status_text.set("Decode failed")
                    if self.dataset:
                        self.export_button.configure(state="normal")
                        self.html_export_button.configure(state="normal")
                        self.details_button.configure(state="normal")
                        self.depth_select_button.configure(state="normal")
                    messagebox.showerror("Decode failed", str(payload))
                elif kind == "progress":
                    self.status_text.set(str(payload))
                elif kind == "exported":
                    report_type, output = payload
                    self.status_text.set(f"{report_type} saved: {output}")
                    messagebox.showinfo("Export complete", f"{report_type} saved to:\n{output}")
                    self.export_button.configure(state="normal")
                    self.html_export_button.configure(state="normal")
        except queue.Empty:
            pass
        self.root.after(100, self._poll_messages)

    def _loaded(self, dataset):
        self.dataset = dataset
        self.edd_path = dataset.definitions.source_path
        self.depth_selected_fault_keys = []
        self.depth_selected_parameter_keys = []
        self.depth_start_time = None
        self.depth_end_time = None
        self._clear_depth_analysis_table()
        self.depth_status_text.set("Select faults and environment parameters to build depth analysis")
        self.visible_records = list(dataset.edv.records)
        self.path_text.set(f"ED_V: {dataset.edv.source_path}    |    ED_D: {dataset.definitions.source_path}")
        if dataset.warnings:
            self.warning_text.set("   • " + "\n   • ".join(dataset.warnings))
            self.warning_label.pack(fill="x", pady=(5, 0))
        else:
            self.warning_label.pack_forget()
        self.metadata_text.set(
            f"Project: {dataset.edv.header.project_name}  |  Version: {dataset.edv.header.project_version}  |  "
            f"Vehicle: {dataset.edv.header.vehicle_name}  |  ODBS: {dataset.edv.header.odbs_address}"
        )
        self.status_text.set(
            f"Loaded {len(dataset.edv.records)} events; {dataset.mapped_event_count} named; "
            f"{dataset.environment_event_count} environments decoded"
        )
        self.export_button.configure(state="normal")
        self.html_export_button.configure(state="normal")
        self.details_button.configure(state="normal")
        self.depth_select_button.configure(state="normal")
        self._apply_filter()

    def _record_values(self, record):
        definition = record.definition
        return (
            f"{record.reference_number:05d}",
            record.start_time.strftime("%Y-%m-%d %H:%M:%S"),
            record.end_time.strftime("%Y-%m-%d %H:%M:%S") if record.end_time else "",
            self._duration(record),
            definition.description if definition else f"Definition unavailable ({record.process_id:02X}:{record.event_id:03X})",
            f"{record.error_codes[0]:04X}",
            definition.name if definition else "",
        )

    def _apply_filter(self):
        if not self.dataset:
            return
        query = self.search.get().strip().lower()
        records = list(self.dataset.edv.records)
        if query:
            records = [
                record
                for record in records
                if query
                in " ".join(
                    [
                        str(record.reference_number),
                        record.start_time.isoformat(" "),
                        f"{record.process_id:02X}:{record.event_id:03X}",
                        f"{record.error_codes[0]:04X}",
                        record.definition.name if record.definition else "",
                        record.definition.description if record.definition else "",
                    ]
                ).lower()
            ]
        self.visible_records = records
        self._populate_tree()

    def _populate_tree(self):
        selected = self.event_tree.selection()
        selected_iid = selected[0] if selected else None
        self.event_tree.delete(*self.event_tree.get_children())
        self.record_by_iid.clear()
        for row_index, record in enumerate(self.visible_records):
            iid = str(record.unique_reference)
            self.record_by_iid[iid] = record
            self.event_tree.insert(
                "",
                "end",
                iid=iid,
                values=self._record_values(record),
                tags=("unknown",) if record.definition is None else (("alternate",) if row_index % 2 else ()),
            )
        total = len(self.dataset.edv.records)
        if len(self.visible_records) == total:
            self.coverage_text.set(f"TOTAL FAULT COUNT: {total}")
        else:
            self.coverage_text.set(f"TOTAL FAULT COUNT: {total}    |    DISPLAYED: {len(self.visible_records)}")
        if selected_iid and selected_iid in self.record_by_iid:
            self.event_tree.selection_set(selected_iid)
            self.event_tree.see(selected_iid)
        elif self.visible_records:
            iid = str(self.visible_records[0].unique_reference)
            self.event_tree.selection_set(iid)
            self.event_tree.focus(iid)
            self.event_tree.see(iid)

    def _sort(self, column):
        self.sort_reverse = not self.sort_reverse if self.sort_column == column else False
        self.sort_column = column
        indices = {name: index for index, name in enumerate(self.EVENT_COLUMNS)}
        position = indices[column]
        self.visible_records.sort(
            key=lambda record: self._record_values(record)[position], reverse=self.sort_reverse
        )
        self._populate_tree()

    def _selected(self, _event=None):
        selected = self.event_tree.selection()
        if not selected or not self.dataset:
            return
        record = self.record_by_iid.get(selected[0])
        if not record:
            return
        definition = record.definition
        label = definition.description if definition else f"Unknown event {record.process_id:02X}:{record.event_id:03X}"
        block = record.environment_block.block_id if record.environment_block else "no block"
        self.selected_text.set(
            f"{record.reference_number:05d}  |  {label}  |  {block}  |  {record.environment_mapping}"
        )
        self.environment_grid.render(environment_rows(record))

    def _selected_record(self):
        selected = self.event_tree.selection()
        return self.record_by_iid.get(selected[0]) if selected else None

    def show_details(self):
        if not self.dataset:
            return
        record = self._selected_record()
        if not record:
            return
        definition = record.definition
        code_lines = []
        for index, (value, code) in enumerate(zip(record.error_codes, record.code_definitions)):
            suffix = ""
            if code:
                suffix = f"  {code.name}  {code.description}".rstrip()
            code_lines.append(f"Error code {index}: {value:04X}{suffix}")
        repair = self.dataset.texts.details_for(definition.name) if definition else ""
        text = "\n".join(
            [
                f"Reference: {record.reference_number:05d}",
                f"Start: {record.start_time}",
                f"End: {record.end_time or ''}",
                f"Process / event: {record.process_id:02X} / {record.event_id:03X}",
                f"Event name: {definition.name if definition else 'Definition unavailable'}",
                f"Description: {definition.description if definition else ''}",
                f"Environment: {record.environment_block.block_id if record.environment_block else ''}",
                f"Mapping: {record.environment_mapping}",
                f"Raw record offset: 0x{record.file_offset:X}",
                "",
                *code_lines,
                "",
                repair or "No repair/cause text is available in the loaded ED_T file.",
            ]
        )
        window = tk.Toplevel(self.root)
        window.title(f"Event {record.reference_number:05d} details")
        window.geometry("850x650")
        viewer = tk.Text(window, wrap="word", font=("Segoe UI", 10), padx=12, pady=12)
        viewer.insert("1.0", text)
        viewer.configure(state="disabled")
        scroll = ttk.Scrollbar(window, command=viewer.yview)
        viewer.configure(yscrollcommand=scroll.set)
        viewer.pack(side="left", fill="both", expand=True)
        scroll.pack(side="right", fill="y")

    def open_depth_analysis_dialog(self):
        if self.depth_dialog_open:
            return
        if not self.dataset:
            messagebox.showwarning("Open ED_V first", "Open an ED_V file before using Depth Analysis.")
            return

        self.depth_dialog_open = True
        dialog = tk.Toplevel(self.root)
        dialog.title("Depth Analysis Selection")
        dialog.geometry("1000x650")
        dialog.transient(self.root)
        dialog.grab_set()

        selected_faults = list(self.depth_selected_fault_keys)
        selected_parameter_set = set(self.depth_selected_parameter_keys)
        default_start, default_end = self._depth_date_default_text()
        start_var = tk.StringVar(
            value=self.depth_start_time.strftime("%Y-%m-%d %H:%M:%S") if self.depth_start_time else default_start
        )
        end_var = tk.StringVar(
            value=self.depth_end_time.strftime("%Y-%m-%d %H:%M:%S") if self.depth_end_time else default_end
        )
        search_var = tk.StringVar()
        fault_combo_var = tk.StringVar()
        date_status = tk.StringVar()
        parameter_search_var = tk.StringVar()
        parameter_status = tk.StringVar(value="Select faults first, then choose environment parameters")

        current_start: datetime | None = None
        current_end: datetime | None = None
        fault_choices = []
        filtered_fault_choices = []
        choice_by_label = {}
        parameter_choices = []
        parameter_key_by_iid = {}

        tabs = ttk.Notebook(dialog)
        tabs.pack(fill="both", expand=True, padx=10, pady=10)
        fault_tab = ttk.Frame(tabs, padding=8)
        parameter_tab = ttk.Frame(tabs, padding=8)
        tabs.add(fault_tab, text="1. Fault Selection")
        tabs.add(parameter_tab, text="2. Environment Parameters")
        tabs.tab(parameter_tab, state="disabled")

        fault_tab.columnconfigure(0, weight=3)
        fault_tab.columnconfigure(1, weight=1)
        fault_tab.rowconfigure(0, weight=1)

        fault_left = ttk.Frame(fault_tab)
        fault_left.grid(row=0, column=0, sticky="nsew", padx=(0, 10))
        fault_left.columnconfigure(0, weight=1)

        ttk.Label(fault_left, text="Search fault:").grid(row=0, column=0, sticky="w")
        search_entry = ttk.Entry(fault_left, textvariable=search_var)
        search_entry.grid(row=1, column=0, sticky="ew", pady=(3, 8))

        ttk.Label(fault_left, text="Matching faults by descending occurrence:").grid(row=2, column=0, sticky="w")
        fault_combo = ttk.Combobox(fault_left, textvariable=fault_combo_var, state="readonly")
        fault_combo.grid(row=3, column=0, sticky="ew", pady=(3, 8))

        selected_frame = ttk.Frame(fault_left)
        selected_frame.grid(row=4, column=0, sticky="nsew")
        fault_left.rowconfigure(4, weight=1)
        selected_list = tk.Listbox(selected_frame, selectmode="extended", height=12, exportselection=False)
        selected_scroll = ttk.Scrollbar(selected_frame, orient="vertical", command=selected_list.yview)
        selected_list.configure(yscrollcommand=selected_scroll.set)
        selected_list.grid(row=0, column=0, sticky="nsew")
        selected_scroll.grid(row=0, column=1, sticky="ns")
        selected_frame.rowconfigure(0, weight=1)
        selected_frame.columnconfigure(0, weight=1)

        fault_buttons = ttk.Frame(fault_left, padding=(0, 8, 0, 0))
        fault_buttons.grid(row=5, column=0, sticky="ew")

        date_frame = ttk.LabelFrame(fault_tab, text="Date / Time Range")
        date_frame.grid(row=0, column=1, sticky="nsew")
        date_frame.columnconfigure(0, weight=1)
        ttk.Label(date_frame, text="From:").grid(row=0, column=0, sticky="w", padx=8, pady=(8, 2))
        start_entry = ttk.Entry(date_frame, textvariable=start_var)
        start_entry.grid(row=1, column=0, sticky="ew", padx=8)
        ttk.Label(date_frame, text="To:").grid(row=2, column=0, sticky="w", padx=8, pady=(8, 2))
        end_entry = ttk.Entry(date_frame, textvariable=end_var)
        end_entry.grid(row=3, column=0, sticky="ew", padx=8)
        ttk.Label(date_frame, text="Format: YYYY-MM-DD HH:MM:SS").grid(row=4, column=0, sticky="w", padx=8, pady=(8, 2))
        ttk.Label(date_frame, textvariable=date_status, wraplength=250).grid(row=6, column=0, sticky="ew", padx=8, pady=(8, 0))

        parameter_tab.columnconfigure(0, weight=1)
        parameter_tab.rowconfigure(3, weight=1)
        ttk.Label(parameter_tab, text="Search environment parameter:").grid(row=0, column=0, sticky="w")
        parameter_search_entry = ttk.Entry(parameter_tab, textvariable=parameter_search_var)
        parameter_search_entry.grid(row=1, column=0, sticky="ew", pady=(3, 8))
        ttk.Label(parameter_tab, textvariable=parameter_status).grid(row=2, column=0, sticky="w", pady=(0, 6))

        parameter_frame = ttk.Frame(parameter_tab)
        parameter_frame.grid(row=3, column=0, sticky="nsew")
        parameter_tree = ttk.Treeview(
            parameter_frame,
            columns=("pick", "name", "unit", "description", "count"),
            show="headings",
            selectmode="browse",
        )
        for column, label, width, anchor in [
            ("pick", "Pick", 55, "center"),
            ("name", "Parameter", 220, "w"),
            ("unit", "Unit", 80, "center"),
            ("description", "Description", 420, "w"),
            ("count", "Rows", 70, "center"),
        ]:
            parameter_tree.heading(column, text=label)
            parameter_tree.column(column, width=width, minwidth=50, anchor=anchor, stretch=column in {"name", "description"})
        parameter_y = ttk.Scrollbar(parameter_frame, orient="vertical", command=parameter_tree.yview)
        parameter_x = ttk.Scrollbar(parameter_frame, orient="horizontal", command=parameter_tree.xview)
        parameter_tree.configure(yscrollcommand=parameter_y.set, xscrollcommand=parameter_x.set)
        parameter_tree.grid(row=0, column=0, sticky="nsew")
        parameter_y.grid(row=0, column=1, sticky="ns")
        parameter_x.grid(row=1, column=0, sticky="ew")
        parameter_frame.rowconfigure(0, weight=1)
        parameter_frame.columnconfigure(0, weight=1)
        parameter_tree.tag_configure("alternate", background="#EAF3FF")

        parameter_buttons = ttk.Frame(parameter_tab, padding=(0, 8, 0, 0))
        parameter_buttons.grid(row=4, column=0, sticky="ew")

        def close_dialog(cancelled: bool = True):
            self.depth_dialog_open = False
            try:
                dialog.grab_release()
            except tk.TclError:
                pass
            dialog.destroy()
            if cancelled and (not self.depth_selected_fault_keys or not self.depth_selected_parameter_keys):
                if self.notebook.select() == str(self.depth_tab):
                    self.notebook.select(self.viewer_tab)

        def read_date_range(show_error: bool = False):
            try:
                start_time = self._parse_depth_datetime(start_var.get(), end_of_day=False)
                end_time = self._parse_depth_datetime(end_var.get(), end_of_day=True)
                if start_time and end_time and start_time > end_time:
                    raise ValueError("From date must be before To date")
                return start_time, end_time
            except ValueError as exc:
                date_status.set(str(exc))
                if show_error:
                    messagebox.showerror("Invalid date range", str(exc), parent=dialog)
                return None

        def refresh_selected_faults():
            count_by_key = {choice["key"]: choice["count"] for choice in fault_choices}
            selected_list.delete(0, tk.END)
            for key in selected_faults:
                selected_list.insert(tk.END, self._fault_label(key, count_by_key.get(key, 0)))

        def filter_fault_choices(*_):
            nonlocal filtered_fault_choices
            terms = search_var.get().strip().lower().split()
            if terms:
                filtered_fault_choices = [
                    choice
                    for choice in fault_choices
                    if all(term in choice["search"] for term in terms)
                ]
            else:
                filtered_fault_choices = list(fault_choices)
            values = [choice["label"] for choice in filtered_fault_choices]
            fault_combo.configure(values=values)
            if values:
                if fault_combo_var.get() not in values:
                    fault_combo_var.set(values[0])
            else:
                fault_combo_var.set("")

        def refresh_fault_choices(show_error: bool = False):
            nonlocal current_start, current_end, fault_choices, filtered_fault_choices, choice_by_label
            parsed = read_date_range(show_error)
            if parsed is None:
                return False
            current_start, current_end = parsed
            fault_choices = self._depth_fault_choices(current_start, current_end)
            choice_by_label = {choice["label"]: choice for choice in fault_choices}
            total_records = len(self._depth_filtered_records(current_start, current_end))
            date_status.set(f"{total_records} event records and {len(fault_choices)} fault types in this range")
            filter_fault_choices()
            refresh_selected_faults()
            return True

        def add_current_fault(_event=None):
            choice = choice_by_label.get(fault_combo_var.get())
            if choice is None and filtered_fault_choices:
                choice = filtered_fault_choices[0]
            if not choice:
                return "break"
            key = choice["key"]
            if key not in selected_faults:
                selected_faults.append(key)
                refresh_selected_faults()
            return "break"

        def remove_selected_faults():
            for index in reversed(selected_list.curselection()):
                selected_faults.pop(index)
            refresh_selected_faults()

        def render_parameter_tree():
            parameter_key_by_iid.clear()
            parameter_tree.delete(*parameter_tree.get_children())
            terms = parameter_search_var.get().strip().lower().split()
            visible_count = 0
            for index, choice in enumerate(parameter_choices):
                if terms and not all(term in choice["search"] for term in terms):
                    continue
                iid = f"param-{index}"
                parameter_key_by_iid[iid] = choice["key"]
                parameter_tree.insert(
                    "",
                    "end",
                    iid=iid,
                    values=(
                        "[x]" if choice["key"] in selected_parameter_set else "[ ]",
                        choice["name"],
                        choice["unit"],
                        choice["description"],
                        choice["count"],
                    ),
                    tags=("alternate",) if visible_count % 2 else (),
                )
                visible_count += 1
            parameter_status.set(
                f"{visible_count} visible parameters; {len(selected_parameter_set)} selected"
            )

        def build_parameter_choices():
            nonlocal parameter_choices
            if not selected_faults:
                messagebox.showwarning("Select faults", "Select one or more faults before choosing parameters.", parent=dialog)
                return
            if not refresh_fault_choices(show_error=True):
                return
            fault_key_set = set(selected_faults)
            parameter_choices = self._depth_parameter_choices(fault_key_set, current_start, current_end)
            available_keys = {choice["key"] for choice in parameter_choices}
            selected_parameter_set.intersection_update(available_keys)
            if not parameter_choices:
                messagebox.showwarning(
                    "No parameters",
                    "No environment parameters were found for the selected faults and date range.",
                    parent=dialog,
                )
                return
            tabs.tab(parameter_tab, state="normal")
            render_parameter_tree()
            tabs.select(parameter_tab)
            parameter_search_entry.focus_set()

        def toggle_parameter(iid: str):
            key = parameter_key_by_iid.get(iid)
            if not key:
                return
            if key in selected_parameter_set:
                selected_parameter_set.remove(key)
            else:
                selected_parameter_set.add(key)
            render_parameter_tree()

        def toggle_clicked_parameter(event):
            iid = parameter_tree.identify_row(event.y)
            column = parameter_tree.identify_column(event.x)
            if iid and column == "#1":
                toggle_parameter(iid)
                return "break"
            return None

        def toggle_focused_parameter(_event=None):
            focused = parameter_tree.focus()
            if focused:
                toggle_parameter(focused)
            return "break"

        def select_all_visible():
            for key in parameter_key_by_iid.values():
                selected_parameter_set.add(key)
            render_parameter_tree()

        def clear_visible():
            for key in parameter_key_by_iid.values():
                selected_parameter_set.discard(key)
            render_parameter_tree()

        def final_submit():
            if not selected_faults:
                messagebox.showwarning("Select faults", "Select one or more faults.", parent=dialog)
                tabs.select(fault_tab)
                return
            if not selected_parameter_set:
                messagebox.showwarning("Select parameters", "Select one or more environment parameters.", parent=dialog)
                return
            ordered_parameters = [
                choice["key"]
                for choice in parameter_choices
                if choice["key"] in selected_parameter_set
            ]
            if not ordered_parameters:
                messagebox.showwarning("Select parameters", "Selected parameters are not available in this range.", parent=dialog)
                return
            self.depth_selected_fault_keys = list(selected_faults)
            self.depth_selected_parameter_keys = ordered_parameters
            self.depth_start_time = current_start
            self.depth_end_time = current_end
            self._refresh_depth_analysis_table()
            self.notebook.select(self.depth_tab)
            close_dialog(cancelled=False)

        ttk.Button(fault_buttons, text="Add Fault", command=add_current_fault).pack(side="left", padx=(0, 6))
        ttk.Button(fault_buttons, text="Remove Selected", command=remove_selected_faults).pack(side="left", padx=(0, 6))
        ttk.Button(fault_buttons, text="Next: Environment Parameters", command=build_parameter_choices).pack(side="right")
        ttk.Button(date_frame, text="Apply Date Range", command=lambda: refresh_fault_choices(show_error=True)).grid(
            row=5, column=0, sticky="ew", padx=8, pady=(8, 0)
        )

        ttk.Button(parameter_buttons, text="Back to Fault Selection", command=lambda: tabs.select(fault_tab)).pack(side="left")
        ttk.Button(parameter_buttons, text="Select All Visible", command=select_all_visible).pack(side="left", padx=(8, 0))
        ttk.Button(parameter_buttons, text="Clear Visible", command=clear_visible).pack(side="left", padx=(8, 0))
        ttk.Button(parameter_buttons, text="Final Submit", command=final_submit).pack(side="right")

        search_var.trace_add("write", filter_fault_choices)
        parameter_search_var.trace_add("write", lambda *_: render_parameter_tree())
        search_entry.bind("<Return>", add_current_fault)
        fault_combo.bind("<Return>", add_current_fault)
        selected_list.bind("<Delete>", lambda _event: remove_selected_faults())
        start_entry.bind("<Return>", lambda _event: refresh_fault_choices(show_error=True))
        end_entry.bind("<Return>", lambda _event: refresh_fault_choices(show_error=True))
        parameter_tree.bind("<ButtonRelease-1>", toggle_clicked_parameter)
        parameter_tree.bind("<space>", toggle_focused_parameter)

        dialog.protocol("WM_DELETE_WINDOW", close_dialog)
        refresh_fault_choices(show_error=False)
        search_entry.focus_set()

    def export_excel(self):
        if not self.dataset:
            return
        default = self.dataset.edv.source_path.stem + "_complete.xlsx"
        chosen = filedialog.asksaveasfilename(
            title="Export complete event and environment report",
            initialfile=default,
            defaultextension=".xlsx",
            filetypes=[("Excel workbook", "*.xlsx")],
        )
        if not chosen:
            return
        self.export_button.configure(state="disabled")
        self.html_export_button.configure(state="disabled")
        self.status_text.set("Preparing complete Excel export...")

        def worker():
            try:
                output = export_xlsx(
                    self.dataset,
                    chosen,
                    progress=lambda value: self.messages.put(("progress", value)),
                )
                self.messages.put(("exported", ("Excel report", output)))
            except Exception as exc:
                self.messages.put(("error", exc))
                self.messages.put(("progress", "Export failed"))

        threading.Thread(target=worker, daemon=True).start()

    def export_html_report(self):
        if not self.dataset:
            return
        default = self.dataset.edv.source_path.stem + "_complete.html"
        chosen = filedialog.asksaveasfilename(
            title="Export standalone HTML event and environment report",
            initialfile=default,
            defaultextension=".html",
            filetypes=[("HTML report", "*.html"), ("All files", "*.*")],
        )
        if not chosen:
            return
        self.export_button.configure(state="disabled")
        self.html_export_button.configure(state="disabled")
        self.status_text.set("Preparing standalone HTML report...")

        def worker():
            try:
                output = export_html(
                    self.dataset,
                    chosen,
                    progress=lambda value: self.messages.put(("progress", value)),
                )
                self.messages.put(("exported", ("HTML report", output)))
            except Exception as exc:
                self.messages.put(("error", exc))
                self.messages.put(("progress", "Export failed"))

        threading.Thread(target=worker, daemon=True).start()


def parse_arguments():
    parser = argparse.ArgumentParser(add_help=False)
    parser.add_argument("edv", nargs="?")
    parser.add_argument("--edd")
    parser.add_argument("--edt")
    return parser.parse_args()


def main():
    args = parse_arguments()
    root = tk.Tk()
    application = TDSViewApplication(root)
    if args.edd:
        application.edd_path = Path(args.edd)
        if args.edt:
            application.edt_path = Path(args.edt)
        else:
            sibling = application.edd_path.with_name(application.edd_path.name.replace("ED_D", "ED_T", 1))
            application.edt_path = sibling if sibling.is_file() else None
        if not args.edv:
            application.path_text.set(f"ED_D: {application.edd_path}")
    if args.edv:
        root.after(
            150,
            lambda: application.load_paths(
                Path(args.edv),
                application.edd_path,
                application.edt_path,
            ),
        )
    root.mainloop()


if __name__ == "__main__":
    main()
