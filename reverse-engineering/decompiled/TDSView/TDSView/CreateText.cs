using System.IO;
using System.Security;

namespace TDSView
{
	public class CreateText
	{
		private ED ed;

		private bool isXML;

		private bool nameIsGiven;

		private string lang;

		public CreateText(ED evdata, bool xml, bool givenName, string language)
		{
			ed = evdata;
			isXML = xml;
			nameIsGiven = givenName;
			lang = language;
		}

		public string GetOutfileName(string ed_d_fileName)
		{
			return Path.ChangeExtension(ed_d_fileName, "TXT");
		}

		public int CreateTextFile(string textFileName)
		{
			return GenTextOutput(textFileName, isXML);
		}

		private int GenTextOutput(string textFileName, bool xml)
		{
			int result = 0;
			string text = ".TXT";
			if (xml)
			{
				text = ".XML";
			}
			textFileName = Path.GetDirectoryName(textFileName) + "\\" + Path.GetFileNameWithoutExtension(textFileName);
			string text2 = "";
			if (!xml && !nameIsGiven)
			{
				text2 = Path.GetExtension(((ED_V)ed.edvl[0]).fileName);
				text2 = text2.Substring(1);
				text2 = "_" + text2;
			}
			string text3 = "";
			if (ed.edvl.Count > 1)
			{
				text3 = "_AndOthers";
			}
			Stream stream = File.Create(textFileName + text2 + text3 + text);
			if (stream != null)
			{
				StreamWriter streamWriter = new StreamWriter(stream, TDSViewUtil.GetEncoding(lang));
				if (xml)
				{
					streamWriter.WriteLine("<?xml version='1.0' encoding='UTF-8'?>");
					streamWriter.WriteLine("<!-- TDSView XML output -->");
					streamWriter.WriteLine("");
					WriteNode(streamWriter, xml, "EVENTFILE", start: true, 0);
				}
				foreach (ED_V item in ed.edvl)
				{
					WriteNode(streamWriter, xml, "HEADER", start: true, 1);
					WriteValue(streamWriter, xml, "BYTE_ORDER_CONTROL", "0x" + item.header.byte_order_control.ToString("X4"), 2);
					WriteValue(streamWriter, xml, "FILE_ID", "0x" + item.header.file_id, 2);
					WriteValue(streamWriter, xml, "PREFIX", item.header.prefix, 2);
					WriteValue(streamWriter, xml, "ED_VERSION", item.header.ed_version, 2);
					WriteValue(streamWriter, xml, "PRJ_NAME", item.header.prj_name, 2);
					WriteValue(streamWriter, xml, "PRJ_VERSION", item.header.prj_version, 2);
					WriteValue(streamWriter, xml, "DIAGNOSIS_SYSTEM", item.header.diagnosis_system, 2);
					WriteValue(streamWriter, xml, "ODBS_ADR", item.header.odbs_adr, 2);
					WriteValue(streamWriter, xml, "VEHICLE_NAME", item.header.vehicle_name, 2);
					WriteValue(streamWriter, xml, "DATE_READ_OUT", item.header.date_read_out, 2);
					WriteValue(streamWriter, xml, "TIME_READ_OUT", item.header.time_read_out, 2);
					WriteValue(streamWriter, xml, "EVENT_COPY", item.header.event_copy, 2);
					WriteValue(streamWriter, xml, "PROPERTY_OPTIONS1", "0x" + item.header.property_option[1].ToString("X2") + item.header.property_option[0].ToString("X2"), 2);
					WriteValue(streamWriter, xml, "PROPERTY_OPTIONS2", "0x" + item.header.property_option[3].ToString("X2") + item.header.property_option[2].ToString("X2"), 2);
					WriteValue(streamWriter, xml, "PROPERTY_OPTIONS3", "0x" + item.header.property_option[5].ToString("X2") + item.header.property_option[4].ToString("X2"), 2);
					WriteValue(streamWriter, xml, "PROPERTY_OPTIONS4", "0x" + item.header.property_option[7].ToString("X2") + item.header.property_option[6].ToString("X2"), 2);
					WriteNode(streamWriter, xml, "HEADER", start: false, 1);
					streamWriter.WriteLine("");
					foreach (ED_V_Data datum in item.data)
					{
						WriteNode(streamWriter, xml, "EVENT", start: true, 1);
						if (xml)
						{
							WriteNode(streamWriter, xml, "DATA", start: true, 2);
						}
						WriteValue(streamWriter, xml, "REFERENCE_NR", datum.reference_nr.ToString(), 3);
						WriteValue(streamWriter, xml, "EVENT_ID", datum.event_id.ToString(), 3);
						WriteValue(streamWriter, xml, "PROCESS_ID", datum.process_id.ToString(), 3);
						WriteValue(streamWriter, xml, "LIMIT", datum.limit.ToString(), 3);
						WriteValue(streamWriter, xml, "VEHICLE_POS", datum.vehicle_pos.ToString(), 3);
						WriteValue(streamWriter, xml, "SUBSYSTEM_NR", datum.subsystem_nr.ToString(), 3);
						WriteValue(streamWriter, xml, "SUBSYSTEM_NAME", item.ed_d.SubSystemName(datum.subsystem_nr), 3);
						WriteValue(streamWriter, xml, "SUBSYSTEM_DESCR", item.ed_d.SubSystemDescr(datum.subsystem_nr), 3);
						WriteValue(streamWriter, xml, "LOCATION", datum.location.ToString(), 3);
						WriteValue(streamWriter, xml, "PRIO", datum.prio.ToString(), 3);
						WriteValue(streamWriter, xml, "PRIO_NAME", item.ed_d.GetPriorityString(datum.prio), 3);
						WriteValue(streamWriter, xml, "ERRORCODE_0", "0X" + datum.errorcode_0.ToString("X4"), 3);
						if (datum.code0 != null)
						{
							WriteValue(streamWriter, xml, "ERRORCODE_0_NAME", datum.code0.code_name, 3);
							WriteValue(streamWriter, xml, "ERRORCODE_0_DESC", datum.code0.code_descr, 3);
						}
						else
						{
							WriteValue(streamWriter, xml, "ERRORCODE_0_NAME", "", 3);
							WriteValue(streamWriter, xml, "ERRORCODE_0_DESC", "", 3);
						}
						WriteValue(streamWriter, xml, "ERRORCODE_1", "0x" + datum.errorcode_1.ToString("X4"), 3);
						if (datum.code1 != null)
						{
							WriteValue(streamWriter, xml, "ERRORCODE_1_NAME", datum.code1.code_name, 3);
							WriteValue(streamWriter, xml, "ERRORCODE_1_DESC", datum.code1.code_descr, 3);
						}
						else
						{
							WriteValue(streamWriter, xml, "ERRORCODE_1_NAME", "", 3);
							WriteValue(streamWriter, xml, "ERRORCODE_1_DESC", "", 3);
						}
						WriteValue(streamWriter, xml, "ERRORCODE_2", "0x" + datum.errorcode_2.ToString("X4"), 3);
						if (datum.code2 != null)
						{
							WriteValue(streamWriter, xml, "ERRORCODE_2_NAME", datum.code2.code_name, 3);
							WriteValue(streamWriter, xml, "ERRORCODE_2_DESC", datum.code2.code_descr, 3);
						}
						else
						{
							WriteValue(streamWriter, xml, "ERRORCODE_2_NAME", "", 3);
							WriteValue(streamWriter, xml, "ERRORCODE_2_DESC", "", 3);
						}
						WriteValue(streamWriter, xml, "ERRORCODE_3", "0x" + datum.errorcode_3.ToString("X4"), 3);
						if (datum.code3 != null)
						{
							WriteValue(streamWriter, xml, "ERRORCODE_3_NAME", datum.code3.code_name, 3);
							WriteValue(streamWriter, xml, "ERRORCODE_3_DESC", datum.code3.code_descr, 3);
						}
						else
						{
							WriteValue(streamWriter, xml, "ERRORCODE_3_NAME", "", 3);
							WriteValue(streamWriter, xml, "ERRORCODE_3_DESC", "", 3);
						}
						if (datum.eventDescr != null)
						{
							WriteValue(streamWriter, xml, "EVENT_NAME", datum.eventDescr.event_name, 3);
							WriteValue(streamWriter, xml, "EVENT_DESCR", datum.eventDescr.event_descr, 3);
						}
						else
						{
							WriteValue(streamWriter, xml, "EVENT_NAME", "", 3);
							WriteValue(streamWriter, xml, "EVENT_DESCR", "", 3);
						}
						WriteValue(streamWriter, xml, "ACKNOW_0", datum.acknow_0.ToString(), 3);
						WriteValue(streamWriter, xml, "ACKNOW_1", datum.acknow_1.ToString(), 3);
						WriteValue(streamWriter, xml, "ACKNOW_2", datum.acknow_2.ToString(), 3);
						WriteValue(streamWriter, xml, "ERR_CODE_MISM", datum.err_code_mism.ToString(), 3);
						WriteValue(streamWriter, xml, "ACTIVE", datum.active.ToString(), 3);
						WriteValue(streamWriter, xml, "DELETED", datum.deleted.ToString(), 3);
						WriteValue(streamWriter, xml, "UPLOADED", datum.uploaded.ToString(), 3);
						WriteValue(streamWriter, xml, "EVENT_CNT", datum.event_cnt.ToString(), 3);
						WriteValue(streamWriter, xml, "START_TIME", TDSViewUtil.TimeDate2Str(datum.start_time, datum.start_time.Millisecond), 3);
						if (datum.end_time_sec != uint.MaxValue)
						{
							WriteValue(streamWriter, xml, "END_TIME", TDSViewUtil.TimeDate2Str(datum.end_time, datum.end_time.Millisecond), 3);
						}
						else
						{
							WriteValue(streamWriter, xml, "END_TIME", "", 3);
						}
						if (item.header.ed_version_int >= 2200)
						{
							WriteValue(streamWriter, xml, "LATITUDE", datum.latitude.ToString(), 3);
							WriteValue(streamWriter, xml, "LONGITUDE", datum.longitude.ToString(), 3);
							WriteValue(streamWriter, xml, "ALTITUDE", datum.altitude.ToString(), 3);
							WriteValue(streamWriter, xml, "SPEED", datum.speed.ToString(), 3);
							WriteValue(streamWriter, xml, "HEADING", datum.heading.ToString(), 3);
							WriteValue(streamWriter, xml, "UTC_TIME", datum.UTC_time.ToString(), 3);
							WriteValue(streamWriter, xml, "ODOMETER", datum.odometer.ToString(), 3);
							WriteValue(streamWriter, xml, "TRIP", datum.trip.ToString(), 3);
						}
						if (datum.eventDescr != null)
						{
							WriteValue(streamWriter, xml, "REP_PRIO_ACTIVE", datum.eventDescr.rep_prio_active, 3);
							WriteValue(streamWriter, xml, "REP_PRIO_PASSIVE", datum.eventDescr.rep_prio_passive, 3);
							WriteValue(streamWriter, xml, "EXTRA_PRIO", datum.eventDescr.extra_prio, 3);
							WriteValue(streamWriter, xml, "EXTRA_ATTRIB1", datum.eventDescr.extra_attrib1, 3);
							WriteValue(streamWriter, xml, "EXTRA_ATTRIB2", datum.eventDescr.extra_attrib2, 3);
							WriteValue(streamWriter, xml, "EXTRA_ATTRIB3", datum.eventDescr.extra_attrib3, 3);
							WriteValue(streamWriter, xml, "EXTRA_ATTRIB4", datum.eventDescr.extra_attrib4, 3);
							WriteValue(streamWriter, xml, "EXTRA_ATTRIB5", datum.eventDescr.extra_attrib5, 3);
						}
						else
						{
							WriteValue(streamWriter, xml, "REP_PRIO_ACTIVE", "", 3);
							WriteValue(streamWriter, xml, "REP_PRIO_PASSIVE", "", 3);
							WriteValue(streamWriter, xml, "EXTRA_PRIO", "", 3);
							WriteValue(streamWriter, xml, "EXTRA_ATTRIB1", "", 3);
							WriteValue(streamWriter, xml, "EXTRA_ATTRIB2", "", 3);
							WriteValue(streamWriter, xml, "EXTRA_ATTRIB3", "", 3);
							WriteValue(streamWriter, xml, "EXTRA_ATTRIB4", "", 3);
							WriteValue(streamWriter, xml, "EXTRA_ATTRIB5", "", 3);
						}
						if (xml)
						{
							WriteNode(streamWriter, xml, "DATA", start: false, 2);
						}
						WriteNode(streamWriter, xml, "EnvData", start: true, 2);
						if (datum.envBlock != null)
						{
							foreach (ED_D_EnvSignal item2 in datum.envBlock.envSignal)
							{
								string text4 = "";
								int trx_block_idx_start = datum.trx_block_idx_start;
								int blkOffset = datum.env_data_cnt_start * trx_block_idx_start * 2;
								text4 += item.GetEnvValue(item2, blkOffset, datum.envDataArr, valueOnly: true, hexAllowed: true);
								string text5 = text4;
								text4 = text5 + " [" + item2.disp_dim + "] [" + item2.disp_type + ";" + item2.coeff_A + ";" + item2.coeff_B + ";" + item2.env_type + ";" + item2.disp_format + ";" + item2.disp_normal + "]";
								WriteValue(streamWriter, xml, item2.env_name, text4, 3);
							}
							if (datum.envBlockODBSGrp != null)
							{
								foreach (ED_D_EnvSignal item3 in datum.envBlockODBSGrp.envSignal)
								{
									string text4 = "";
									int dbs_trx_block_idx_start = datum.dbs_trx_block_idx_start;
									int blkOffset2 = datum.dbs_env_data_cnt_start * dbs_trx_block_idx_start * 2;
									text4 += item.GetEnvValue(item3, blkOffset2, datum.dbs_envDataArr, valueOnly: true, hexAllowed: true);
									string text6 = text4;
									text4 = text6 + " [" + item3.disp_dim + "] [" + item3.disp_type + ";" + item3.coeff_A + ";" + item3.coeff_B + ";" + item3.env_type + ";" + item3.disp_format + ";" + item3.disp_normal + "]";
									WriteValue(streamWriter, xml, item3.env_name, text4, 3);
								}
							}
						}
						WriteNode(streamWriter, xml, "EnvData", start: false, 2);
						WriteNode(streamWriter, xml, "EVENT", start: false, 1);
						streamWriter.WriteLine("");
					}
				}
				WriteNode(streamWriter, xml, "EVENTFILE", start: false, 0);
				streamWriter.Close();
			}
			else
			{
				result = 5;
			}
			return result;
		}

		private int WriteValue(StreamWriter writer, bool xml, string key, string value, int indentLevel)
		{
			string text = "";
			for (int i = 0; i < indentLevel; i++)
			{
				text += "  ";
			}
			if (xml)
			{
				string text2 = SecurityElement.Escape(value);
				writer.WriteLine(text + "<" + key + " value=\"" + text2 + "\"/>");
			}
			else
			{
				writer.WriteLine(key + "=" + value);
			}
			return 0;
		}

		private int WriteNode(StreamWriter writer, bool xml, string node, bool start, int indentLevel)
		{
			string text = "";
			for (int i = 0; i < indentLevel; i++)
			{
				text += "  ";
			}
			if (xml)
			{
				if (start)
				{
					writer.WriteLine(text + "<" + node + ">");
				}
				else
				{
					writer.WriteLine(text + "</" + node + ">");
				}
			}
			else if (start)
			{
				writer.WriteLine("<" + node + ">");
			}
			return 0;
		}
	}
}
