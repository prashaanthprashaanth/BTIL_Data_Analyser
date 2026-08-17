using System.IO;
using TDSView.Properties;

namespace TDSView
{
	public class CreateRawDataCsv
	{
		private ED_V evl;

		public string GetOutfileName(string ed_v_fileName)
		{
			return Path.ChangeExtension(ed_v_fileName, "TSV");
		}

		private int GenTextOutput(string textFileName)
		{
			int result = 0;
			bool flag = false;
			Stream stream = File.Create(textFileName + "_.EDVRAW");
			if (stream != null)
			{
				StreamWriter streamWriter = new StreamWriter(stream);
				string text = "VEHICLE_NAME\tPROJECT_VERSION\tREFERENCE_NR\tEVENT_ID\tPROCESS_ID\tLIMIT\tVEHICLE_POS\tSUBSYSTEM_NR\tLOCATION\tPRIO\tERRORCODE_0\tERRORCODE_1\tERRORCODE_2\tERRORCODE_3\tACKNOW_0\tACKNOW_1\tACKNOW_2\tACKNOW_3\tERR_CODE_MISM\tACTIVE\tDELETED\tUPLOADED\tEVENT_CNT\tSTART_TIME\tEND_TIME\tenv_data_cnt_start\tblock_cnt_start\told_block_idx_start\ttrx_block_idx_start\tenv_data_cnt_end\tblock_cnt_end\told_block_idx_end\ttrx_block_idx_end";
				if (evl.header.ed_version_int >= 2200)
				{
					text += "\tLATITUDE\tLONGITUDE\tALTITUDE\tSPEED\tHEADING\tUTC_TIME\tODOMETER\tTRIP\tdbs_env_data_cnt_start\tdbs_block_cnt_start\tdbs_old_block_idx_start\tdbs_trx_block_idx_start\tdbs_env_data_cnt_end\tdbs_block_cnt_end\tdbs_old_block_idx_end\tdbs_trx_block_idx_end";
				}
				string vehicle_name = evl.header.vehicle_name;
				string prj_version = evl.header.prj_version;
				foreach (ED_V_Data datum in evl.data)
				{
					string text2 = "";
					text2 = text2 + vehicle_name + "\t";
					text2 = text2 + prj_version + "\t";
					text2 = text2 + datum.reference_nr + "\t";
					text2 = text2 + datum.event_id + "\t";
					text2 = text2 + datum.process_id + "\t";
					text2 = text2 + datum.limit + "\t";
					text2 = text2 + datum.vehicle_pos + "\t";
					text2 = text2 + datum.subsystem_nr + "\t";
					text2 = text2 + datum.location + "\t";
					text2 = text2 + datum.prio + "\t";
					text2 = text2 + datum.errorcode_0.ToString("X4") + "\t";
					text2 = text2 + datum.errorcode_1.ToString("X4") + "\t";
					text2 = text2 + datum.errorcode_2.ToString("X4") + "\t";
					text2 = text2 + datum.errorcode_3.ToString("X4") + "\t";
					text2 = text2 + datum.acknow_0 + "\t";
					text2 = text2 + datum.acknow_1 + "\t";
					text2 = text2 + datum.acknow_2 + "\t";
					text2 = text2 + datum.acknow_3 + "\t";
					text2 = text2 + datum.err_code_mism + "\t";
					text2 = text2 + datum.active + "\t";
					text2 = text2 + datum.deleted + "\t";
					text2 = text2 + datum.uploaded + "\t";
					text2 = text2 + datum.event_cnt + "\t";
					text2 = text2 + TDSViewUtil.TimeDate2Str(datum.start_time, datum.start_time.Millisecond) + "\t";
					text2 = ((datum.end_time_sec == uint.MaxValue) ? (text2 + "\t") : (text2 + TDSViewUtil.TimeDate2Str(datum.end_time, datum.end_time.Millisecond) + "\t"));
					text2 = text2 + datum.env_data_cnt_start + "\t";
					text2 = text2 + datum.block_cnt_start + "\t";
					text2 = text2 + datum.old_block_idx_start + "\t";
					text2 = text2 + datum.trx_block_idx_start + "\t";
					text2 = text2 + datum.env_data_cnt_end + "\t";
					text2 = text2 + datum.block_cnt_end + "\t";
					text2 = text2 + datum.old_block_idx_end + "\t";
					text2 = text2 + datum.trx_block_idx_end + "\t";
					if (evl.header.ed_version_int >= 2200)
					{
						text2 = text2 + datum.latitude + "\t";
						text2 = text2 + datum.longitude + "\t";
						text2 = text2 + datum.altitude + "\t";
						text2 = text2 + datum.speed + "\t";
						text2 = text2 + datum.heading + "\t";
						text2 = text2 + datum.UTC_time + "\t";
						text2 = text2 + datum.odometer + "\t";
						text2 = text2 + datum.trip + "\t";
						text2 = text2 + datum.dbs_env_data_cnt_start + "\t";
						text2 = text2 + datum.dbs_block_cnt_start + "\t";
						text2 = text2 + datum.dbs_old_block_idx_start + "\t";
						text2 = text2 + datum.dbs_trx_block_idx_start + "\t";
						text2 = text2 + datum.dbs_env_data_cnt_end + "\t";
						text2 = text2 + datum.dbs_block_cnt_end + "\t";
						text2 = text2 + datum.dbs_old_block_idx_end + "\t";
						text2 = text2 + datum.dbs_trx_block_idx_end + "\t";
					}
					if (!flag)
					{
						streamWriter.WriteLine(text);
						flag = true;
					}
					streamWriter.WriteLine(text2);
				}
				streamWriter.Close();
				if (!flag)
				{
					result = 9;
				}
			}
			else
			{
				result = 5;
			}
			return result;
		}

		public int CreateTextFile(string textFileName)
		{
			evl = new ED_V(Settings.Default.UseLocalPcTime, Settings.Default.TimeZoneShift);
			int num = evl.Read_ED_V_File(textFileName);
			if (num == 0)
			{
				num = GenTextOutput(textFileName);
			}
			return num;
		}
	}
}
