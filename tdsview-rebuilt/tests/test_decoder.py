from __future__ import annotations

import os
import sys
import tempfile
import unittest
from datetime import datetime
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

from tds_decoder import decode_dataset, decode_environment_value, environment_rows
from tds_decoder.export import export_html
from tds_decoder.models import (
    DecodedDataset,
    EDVFile,
    EDVHeader,
    EnvironmentBlock,
    EnvironmentSignal,
    EventDefinition,
    EventRecord,
    OtiDefinitions,
    TextDefinitions,
)
from tds_decoder.oti import split_oti_line


def signal(**overrides):
    values = dict(
        block_id="TEST",
        word=1,
        bit=None,
        byte_selector="",
        data_type="W",
        name="signal",
        description="",
        display_type="A",
        coefficient_a=0.5,
        coefficient_b=1.0,
        unit="V",
        normal_value="",
        decimals=2,
    )
    values.update(overrides)
    return EnvironmentSignal(**values)


class UnitTests(unittest.TestCase):
    def test_quote_aware_oti_split(self):
        self.assertEqual(
            split_oti_line('F5;0AC;"Name";"Text; with semicolon";A'),
            ["F5", "0AC", "Name", "Text; with semicolon", "A"],
        )

    def test_scaled_signed_word(self):
        self.assertEqual(decode_environment_value(signal(), 0, b"\xfe\xff"), "0.00")

    def test_bit_in_high_byte(self):
        bit = signal(data_type="B", display_type="", bit=9)
        self.assertEqual(decode_environment_value(bit, 0, b"\x00\x02"), "1")

    def test_html_export_contains_escaped_sections_and_environment(self):
        definition = EventDefinition(
            process_id=1,
            event_id=2,
            name="EV_Test",
            description="Brake <fault> & test",
            environment_block_id="BLK",
        )
        block = EnvironmentBlock(
            block_id="BLK",
            description="Test block",
            cycle_time_us=128000,
            signals=[
                signal(
                    block_id="BLK",
                    name="XI<1>",
                    description="Current & amps",
                    data_type="W",
                    display_type="O",
                )
            ],
        )
        record = EventRecord(
            file_offset=0,
            unique_reference=7,
            reference_number=1,
            event_id=2,
            limit=0,
            vehicle_position=0,
            process_id=1,
            subsystem_number=0,
            location=0,
            priority=0,
            error_codes=(0x000A, 0, 0, 0),
            acknowledgements=(0, 0, 0, 0),
            error_code_mismatch=0,
            active=1,
            deleted=0,
            uploaded=0,
            event_count=1,
            start_seconds=0,
            start_relative=0,
            start_time=datetime(2026, 1, 1, 12, 0, 0),
            end_seconds=0,
            end_relative=0,
            end_time=None,
            environment_words_start=1,
            environment_block_count_start=2,
            environment_old_index_start=0,
            environment_trigger_index_start=1,
            environment_data=b"\x04\x00\x05\x00",
            definition=definition,
            environment_block=block,
            environment_mapping="exact",
        )
        dataset = DecodedDataset(
            edv=EDVFile(
                source_path=Path("ED_V_sample.9240"),
                header=EDVHeader(
                    byte_order_control=0,
                    file_id="ED_V",
                    prefix="ED",
                    ed_version="2200",
                    project_name="IR_PRP",
                    project_version="001.005.004.020",
                    diagnosis_system="TDS",
                    odbs_address="ODBS1",
                    vehicle_name="BTIL39240",
                    readout_date="2026-01-01",
                    readout_time="12:00:00",
                    event_copy="",
                    property_options=b"",
                ),
                records=[record],
                trailer=b"\x01\x02\x03\x04",
            ),
            definitions=OtiDefinitions(
                source_path=Path("ED_D_IR_PRP___001_005_004_020_EN.oti"),
                project_name="IR_PRP",
                project_version="001.005.004.020",
                events={(1, 2): definition},
                environment_blocks={"BLK": block},
            ),
            texts=TextDefinitions(repairs={"EV_Test": "Check <relay> & reset"}),
            warnings=["definition warning <check>"],
        )
        with tempfile.TemporaryDirectory() as directory:
            output = export_html(dataset, Path(directory) / "report.html")
            content = output.read_text(encoding="utf-8")

        self.assertIn('<section id="event-list">', content)
        self.assertIn('<section id="environment-data">', content)
        self.assertIn("Brake &lt;fault&gt; &amp; test", content)
        self.assertIn("XI&lt;1&gt;", content)
        self.assertIn("Check &lt;relay&gt; &amp; reset", content)
        self.assertIn('class="changed"', content)


class SuppliedSampleIntegrationTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.edv = Path(os.environ.get("TDS_SAMPLE_EDV", ""))
        cls.edd = Path(os.environ.get("TDS_SAMPLE_EDD", ""))
        if not cls.edv.is_file() or not cls.edd.is_file():
            raise unittest.SkipTest("set TDS_SAMPLE_EDV and TDS_SAMPLE_EDD")
        cls.dataset = decode_dataset(cls.edv, cls.edd)

    def test_header_and_coverage(self):
        self.assertEqual(self.dataset.edv.header.project_version, "001.005.004.019")
        self.assertEqual(self.dataset.edv.header.vehicle_name, "IRPRP39240")
        self.assertEqual(len(self.dataset.edv.records), 550)
        self.assertEqual(self.dataset.mapped_event_count, 550)
        self.assertEqual(self.dataset.environment_event_count, 550)
        self.assertEqual(self.dataset.edv.trailer.hex(), "a02b384d")

    def test_new_event_and_environment(self):
        record = self.dataset.edv.records[252]
        self.assertEqual(record.reference_number, 253)
        self.assertEqual(record.definition.name, "D_UAcLoFrDt_CON18")
        self.assertEqual(record.environment_block.block_id, "S_CvDgEnvGrL_1")
        rows = {row.name: row for row in environment_rows(record)}
        self.assertEqual(rows["XU_Ln_CON18"].offsets_ms, [-512, -384, -256, -128, 0, 128, 256])
        self.assertEqual(
            rows["XU_Ln_CON18"].values,
            ["3.47", "3.40", "3.37", "3.34", "3.31", "3.29", "3.26"],
        )

    def test_reference_521_values(self):
        record = self.dataset.edv.records[520]
        rows = {row.name: row for row in environment_rows(record)}
        self.assertEqual(
            rows["XI_Ln_CON11"].values,
            ["5.86", "4.15", "3.17", "3.42", "7.57", "6.84", "6.59"],
        )


if __name__ == "__main__":
    unittest.main()
