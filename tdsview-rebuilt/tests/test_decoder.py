from __future__ import annotations

import os
import sys
import unittest
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

from tds_decoder import decode_dataset, decode_environment_value, environment_rows
from tds_decoder.models import EnvironmentSignal
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
