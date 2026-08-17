using System;
using System.IO;

namespace TDSView
{
	public class CreateCsv
	{
		private ED ed;

		private string lang;

		public CreateCsv(ED evdata, string language)
		{
			ed = evdata;
			lang = language;
		}

		public string GetOutfileName(string ed_v_fileName)
		{
			return Path.ChangeExtension(ed_v_fileName, "TSV");
		}

		private int GenTextOutput(string textFileName, string signalNameFilter)
		{
			int result = 0;
			bool flag = false;
			string extension = Path.GetExtension(((ED_V)ed.edvl[0]).fileName);
			extension = extension.Substring(1);
			string text = "";
			if (ed.edvl.Count > 1)
			{
				text = "_AndOthers";
			}
			Stream stream = File.Create(textFileName + "_" + extension + text + ".TSV");
			if (stream != null)
			{
				StreamWriter streamWriter = new StreamWriter(stream, TDSViewUtil.GetEncoding(lang));
				foreach (ED_V item in ed.edvl)
				{
					string text2 = "REFERENCE_NR\tEVENT_ID\tPROCESS_ID\tLIMIT\tVEHICLE_POS\tSUBSYSTEM_NR\tSUBSYSTEM_NAME\tSUBSYSTEM_DESCR\tLOCATION\tPRIO\tPRIO_NAME\tERRORCODE_0\tERRORCODE_0_NAME\tERRORCODE_0_DESC\tERRORCODE_1\tERRORCODE_1_NAME\tERRORCODE_1_DESC\tERRORCODE_2\tERRORCODE_2_NAME\tERRORCODE_2_DESC\tERRORCODE_3\tERRORCODE_3_NAME\tERRORCODE_3_DESC\tEVENT_NAME\tEVENT_DESCR\tACKNOW_0\tACKNOW_1\tACKNOW_2\tERR_CODE_MISM\tACTIVE\tDELETED\tUPLOADED\tEVENT_CNT\tSTART_TIME\tEND_TIME\tDURATION\tVEHICLE_NAME\tPROJECT_VERSION";
					if (item.header.ed_version_int >= 2200)
					{
						text2 += "\tLATITUDE\tLONGITUDE\tALTITUDE\tSPEED\tHEADING\tUTC_TIME\tODOMETER\tTRIP";
					}
					text2 += "\tREP_PRIO_ACTIVE\tREP_PRIO_PASSIVE\tEXTRA_PRIO\tEXTRA_ATTRIB1\tEXTRA_ATTRIB2\tEXTRA_ATTRIB3\tEXTRA_ATTRIB4\tEXTRA_ATTRIB5";
					string vehicle_name = item.header.vehicle_name;
					string prj_version = item.header.prj_version;
					bool flag2 = signalNameFilter.Equals("$ALL$");
					bool flag3 = signalNameFilter.Equals("$ALL_ENV$");
					foreach (ED_V_Data datum in item.data)
					{
						if (datum.eventDescr == null || (!datum.eventDescr.event_name.Equals(signalNameFilter) && !flag2 && !flag3))
						{
							continue;
						}
						string text3 = datum.reference_nr + "\t";
						text3 = text3 + datum.event_id + "\t";
						text3 = text3 + datum.process_id + "\t";
						text3 = text3 + datum.limit + "\t";
						text3 = text3 + datum.vehicle_pos + "\t";
						text3 = text3 + datum.subsystem_nr + "\t";
						text3 = text3 + item.ed_d.SubSystemName(datum.subsystem_nr) + "\t";
						text3 = text3 + item.ed_d.SubSystemDescr(datum.subsystem_nr) + "\t";
						text3 = text3 + datum.location + "\t";
						text3 = text3 + datum.prio + "\t";
						text3 = text3 + item.ed_d.GetPriorityString(datum.prio) + "\t";
						text3 = text3 + datum.errorcode_0.ToString("X4") + "\t";
						if (datum.code0 != null)
						{
							string text4 = text3;
							text3 = text4 + datum.code0.code_name + "\t" + datum.code0.code_descr + "\t";
						}
						else
						{
							text3 += "\t\t";
						}
						text3 = text3 + datum.errorcode_1.ToString("X4") + "\t";
						if (datum.code1 != null)
						{
							text3 = text3 + datum.code1.code_name + "\t";
							text3 = text3 + datum.code1.code_descr + "\t";
						}
						else
						{
							text3 += "\t\t";
						}
						text3 = text3 + datum.errorcode_2.ToString("X4") + "\t";
						if (datum.code2 != null)
						{
							text3 = text3 + datum.code2.code_name + "\t";
							text3 = text3 + datum.code2.code_descr + "\t";
						}
						else
						{
							text3 += "\t\t";
						}
						text3 = text3 + datum.errorcode_3.ToString("X4") + "\t";
						if (datum.code3 != null)
						{
							text3 = text3 + datum.code3.code_name + "\t";
							text3 = text3 + datum.code3.code_descr + "\t";
						}
						else
						{
							text3 += "\t\t";
						}
						if (datum.eventDescr != null)
						{
							text3 = text3 + datum.eventDescr.event_name + "\t";
							text3 = text3 + datum.eventDescr.event_descr + "\t";
						}
						else
						{
							text3 += (text3 += "\t\t");
						}
						text3 = text3 + datum.acknow_0 + "\t";
						text3 = text3 + datum.acknow_1 + "\t";
						text3 = text3 + datum.acknow_2 + "\t";
						text3 = text3 + datum.err_code_mism + "\t";
						text3 = text3 + datum.active + "\t";
						text3 = text3 + datum.deleted + "\t";
						text3 = text3 + datum.uploaded + "\t";
						text3 = text3 + datum.event_cnt + "\t";
						text3 = text3 + TDSViewUtil.TimeDate2Str(datum.start_time, datum.start_time.Millisecond) + "\t";
						text3 = ((datum.end_time_sec == uint.MaxValue) ? (text3 + "\t") : (text3 + TDSViewUtil.TimeDate2Str(datum.end_time, datum.end_time.Millisecond) + "\t"));
						if (datum.end_time_sec != uint.MaxValue)
						{
							if (datum.start_time.CompareTo(datum.end_time) <= 0)
							{
								TimeSpan tSpan = datum.end_time - datum.start_time;
								text3 = text3 + TDSViewUtil.GetSpanString(tSpan, startStopTimeMilliSec: true) + "\t";
							}
							else
							{
								text3 += "Negative!\t";
							}
						}
						else
						{
							text3 += "\t";
						}
						text3 = text3 + vehicle_name + "\t";
						text3 = text3 + prj_version + "\t";
						if (datum.eventDescr != null)
						{
							text3 = text3 + datum.eventDescr.rep_prio_active + "\t";
							text3 = text3 + datum.eventDescr.rep_prio_passive + "\t";
							text3 = text3 + datum.eventDescr.extra_prio + "\t";
							text3 = text3 + datum.eventDescr.extra_attrib1 + "\t";
							text3 = text3 + datum.eventDescr.extra_attrib2 + "\t";
							text3 = text3 + datum.eventDescr.extra_attrib3 + "\t";
							text3 = text3 + datum.eventDescr.extra_attrib4 + "\t";
							text3 = text3 + datum.eventDescr.extra_attrib5 + "\t";
						}
						else
						{
							text3 += (text3 += "\t\t\t\t\t\t\t\t");
						}
						if (item.header.ed_version_int >= 2200)
						{
							text3 = text3 + datum.latitude + "\t";
							text3 = text3 + datum.longitude + "\t";
							text3 = text3 + datum.altitude + "\t";
							text3 = text3 + datum.speed + "\t";
							text3 = text3 + datum.heading + "\t";
							text3 = text3 + datum.UTC_time + "\t";
							text3 = text3 + datum.odometer + "\t";
							text3 = text3 + datum.trip + "\t";
						}
						if (datum.envBlock != null && !flag2 && !flag3)
						{
							foreach (ED_D_EnvSignal item2 in datum.envBlock.envSignal)
							{
								if (!flag)
								{
									text2 = text2 + item2.env_name + "\t";
								}
								int trx_block_idx_start = datum.trx_block_idx_start;
								int blkOffset = datum.env_data_cnt_start * trx_block_idx_start * 2;
								string envValue = item.GetEnvValue(item2, blkOffset, datum.envDataArr, valueOnly: true, hexAllowed: true);
								text3 = text3 + envValue + "\t";
							}
						}
						if (datum.envBlock != null && flag3)
						{
							foreach (ED_D_EnvSignal item3 in datum.envBlock.envSignal)
							{
								text3 = text3 + item3.env_name + "\t";
								int trx_block_idx_start2 = datum.trx_block_idx_start;
								int blkOffset2 = datum.env_data_cnt_start * trx_block_idx_start2 * 2;
								string envValue = item.GetEnvValue(item3, blkOffset2, datum.envDataArr, valueOnly: true, hexAllowed: true);
								text3 = text3 + envValue + "\t";
							}
							if (datum.envBlockODBSGrp != null)
							{
								foreach (ED_D_EnvSignal item4 in datum.envBlockODBSGrp.envSignal)
								{
									text3 = text3 + item4.env_name + "\t";
									int dbs_trx_block_idx_start = datum.dbs_trx_block_idx_start;
									int blkOffset3 = datum.dbs_env_data_cnt_start * dbs_trx_block_idx_start * 2;
									string envValue = item.GetEnvValue(item4, blkOffset3, datum.dbs_envDataArr, valueOnly: true, hexAllowed: true);
									text3 = text3 + envValue + "\t";
								}
							}
						}
						if (!flag)
						{
							streamWriter.WriteLine(text2);
							flag = true;
						}
						streamWriter.WriteLine(text3);
					}
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

		public int CreateTextFile(string textFileName, string signalNameFilter)
		{
			return GenTextOutput(textFileName, signalNameFilter);
		}
	}
}
