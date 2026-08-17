using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TDSView
{
	public class CreateSTSNT
	{
		public delegate bool genSTSNTDialog(ArrayList tlistX, bool createDirectOutput);

		private ED ed;

		public ArrayList outList;

		public ArrayList timeList;

		public genSTSNTDialog genSTSNTDialogMethod;

		public string stsntFileNameM;

		public CreateSTSNT(ED evdata)
		{
			ed = evdata;
			outList = new ArrayList();
			timeList = new ArrayList();
		}

		public void InitCallBack(genSTSNTDialog genSTSNTDlg)
		{
			genSTSNTDialogMethod = genSTSNTDlg;
		}

		private int GetIndexFromOutlist(string variableName)
		{
			int result = 0;
			foreach (ED_V_OutListEntry @out in outList)
			{
				if (@out.event_name.Equals(variableName))
				{
					result = @out.inx;
					break;
				}
			}
			return result;
		}

		private bool isInSelectionList(string signalName)
		{
			bool result = false;
			foreach (string selection in ed.selectionList)
			{
				if (selection.Equals(signalName))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		private string GetSTSNTDataType(string env_type, string disp_type)
		{
			string result = "";
			if (env_type.Equals("B"))
			{
				result = "BOOLEAN1";
			}
			else if (env_type.Equals("Y"))
			{
				result = "BYTE";
			}
			else if (env_type.Equals("W"))
			{
				if (disp_type.Equals("A"))
				{
					result = "ANALOG";
				}
				else if (disp_type.Equals("O"))
				{
					result = "GANZOHNE";
				}
				else if (disp_type.Equals("M"))
				{
					result = "GANZMIT";
				}
				else if (disp_type.Equals("H"))
				{
					result = "WORD";
				}
			}
			else if (env_type.Equals("D"))
			{
				if (disp_type.Equals("A"))
				{
					result = "Keine_Ahnung";
				}
				else if (disp_type.Equals("O"))
				{
					result = "Keine_Ahnung";
				}
				else if (disp_type.Equals("M"))
				{
					result = "LONGCARD";
				}
				else if (disp_type.Equals("H"))
				{
					result = "DWORD";
				}
			}
			return result;
		}

		public bool CreateOutList()
		{
			int num = 0;
			outList.Clear();
			foreach (ED_V item in ed.edvl)
			{
				foreach (ED_V_Data datum in item.data)
				{
					bool flag = false;
					if (datum.eventDescr != null)
					{
						flag = isInSelectionList(datum.eventDescr.event_name);
					}
					if (!flag)
					{
						continue;
					}
					bool flag2 = false;
					foreach (ED_V_OutListEntry @out in outList)
					{
						if (datum.eventDescr != null)
						{
							if (@out.event_name.Equals(datum.eventDescr.event_name))
							{
								flag2 = true;
								datum.outputListInx = @out.inx;
								break;
							}
							continue;
						}
						break;
					}
					if (flag2 || datum.eventDescr == null)
					{
						continue;
					}
					num = (datum.outputListInx = num + 1);
					ED_V_OutListEntry eD_V_OutListEntry2 = new ED_V_OutListEntry();
					eD_V_OutListEntry2.event_name = datum.eventDescr.event_name;
					eD_V_OutListEntry2.inx = num;
					eD_V_OutListEntry2.isEvent = true;
					eD_V_OutListEntry2.dataType = "BOOLEAN1";
					outList.Add(eD_V_OutListEntry2);
					bool flag3 = false;
					if (datum.envBlock == null)
					{
						continue;
					}
					foreach (ED_D_EnvSignal item2 in datum.envBlock.envSignal)
					{
						flag3 = false;
						foreach (ED_V_OutListEntry out2 in outList)
						{
							if (out2.event_name.Equals(item2.env_name))
							{
								flag3 = true;
							}
						}
						if (!flag3)
						{
							num++;
							ED_V_OutListEntry eD_V_OutListEntry4 = new ED_V_OutListEntry();
							eD_V_OutListEntry4.event_name = item2.env_name;
							eD_V_OutListEntry4.inx = num;
							eD_V_OutListEntry4.isEvent = false;
							eD_V_OutListEntry4.dataType = GetSTSNTDataType(item2.env_type, item2.disp_type);
							outList.Add(eD_V_OutListEntry4);
						}
					}
				}
			}
			return true;
		}

		private void CreateTimeList(bool keepEnvVarValueBetweenEvents)
		{
			foreach (ED_V item in ed.edvl)
			{
				foreach (ED_V_Data datum in item.data)
				{
					if (datum.eventDescr == null || datum.envBlock == null || !isInSelectionList(datum.eventDescr.event_name) || datum.start_time.CompareTo(datum.end_time) >= 0)
					{
						continue;
					}
					ED_V_TimeListEntry eD_V_TimeListEntry = new ED_V_TimeListEntry();
					eD_V_TimeListEntry.event_name = datum.eventDescr.event_name;
					eD_V_TimeListEntry.time = datum.start_time;
					eD_V_TimeListEntry.value = " 1";
					eD_V_TimeListEntry.inx = datum.outputListInx;
					eD_V_TimeListEntry.isEvent = true;
					timeList.Add(eD_V_TimeListEntry);
					ED_V_TimeListEntry eD_V_TimeListEntry2 = new ED_V_TimeListEntry();
					eD_V_TimeListEntry2.event_name = datum.eventDescr.event_name;
					eD_V_TimeListEntry2.time = datum.end_time;
					eD_V_TimeListEntry2.value = " 0";
					eD_V_TimeListEntry2.inx = datum.outputListInx;
					eD_V_TimeListEntry2.isEvent = true;
					if (datum.end_time.Year < 2100)
					{
						timeList.Add(eD_V_TimeListEntry2);
					}
					int[] array = new int[datum.block_cnt_start];
					int num = datum.old_block_idx_start;
					int num2 = 0;
					for (int i = 0; i < datum.block_cnt_start; i++)
					{
						array[i] = num;
						if (num == datum.trx_block_idx_start)
						{
							num2 = i;
						}
						num++;
						if (num >= datum.block_cnt_start)
						{
							num = 0;
						}
					}
					int num3 = int.Parse(datum.envBlock.cycle_time);
					foreach (ED_D_EnvSignal item2 in datum.envBlock.envSignal)
					{
						int num4 = 1;
						int[] array2 = array;
						int num6;
						foreach (int num5 in array2)
						{
							int blkOffset = datum.env_data_cnt_start * num5 * 2;
							ED_V_TimeListEntry eD_V_TimeListEntry3 = new ED_V_TimeListEntry();
							num6 = (num4 - num2 - 1) * num3 / 1000;
							eD_V_TimeListEntry3.event_name = item2.env_name;
							eD_V_TimeListEntry3.time = datum.start_time.AddMilliseconds(num6);
							eD_V_TimeListEntry3.value = item.GetEnvValue(item2, blkOffset, datum.envDataArr, valueOnly: true, hexAllowed: false);
							eD_V_TimeListEntry3.inx = GetIndexFromOutlist(item2.env_name);
							eD_V_TimeListEntry3.isEvent = false;
							timeList.Add(eD_V_TimeListEntry3);
							num4++;
						}
						ED_V_TimeListEntry eD_V_TimeListEntry4 = new ED_V_TimeListEntry();
						num6 = (num4 - num2 - 1) * num3 / 1000;
						if (!keepEnvVarValueBetweenEvents)
						{
							if (num6 >= 1000)
							{
								num6 = 999;
							}
							eD_V_TimeListEntry4.event_name = item2.env_name;
							eD_V_TimeListEntry4.time = datum.start_time.AddMilliseconds(num6);
							eD_V_TimeListEntry4.value = "0";
							eD_V_TimeListEntry4.inx = GetIndexFromOutlist(item2.env_name);
							eD_V_TimeListEntry.isEvent = false;
							timeList.Add(eD_V_TimeListEntry4);
						}
					}
				}
			}
			IComparer comparer = new CompStartTime();
			timeList.Sort(comparer);
		}

		private bool GenSTSNTOutput(string preDefOutputDir, bool createDirectOutput)
		{
			ED_V eD_V = (ED_V)ed.edvl[0];
			string text = ((!preDefOutputDir.Equals("")) ? preDefOutputDir : Path.GetDirectoryName(eD_V.fileName));
			string extension = Path.GetExtension(eD_V.fileName);
			extension = extension.Substring(1);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(eD_V.fileName);
			string text2 = "";
			if (ed.edvl.Count > 1)
			{
				text2 = "_AndOthers";
			}
			fileNameWithoutExtension = text + "\\" + fileNameWithoutExtension + "_" + extension + text2;
			stsntFileNameM = fileNameWithoutExtension + ".M01";
			Stream stream = File.Create(fileNameWithoutExtension + ".N01");
			Stream stream2 = File.Create(stsntFileNameM);
			bool flag = false;
			if (stream != null && stream2 != null)
			{
				DateTime dateTime = default(DateTime);
				flag = genSTSNTDialogMethod(timeList, createDirectOutput);
				if (flag)
				{
					int num = 1;
					foreach (ED_V_TimeListEntry time in timeList)
					{
						bool flag2 = time.time.CompareTo(ed.stsntStartTime) > 0;
						bool flag3 = time.time.CompareTo(ed.stsntEndTime) < 0;
						if (flag2 && flag3)
						{
							string text3 = time.time.Year + "-" + time.time.Month + "-" + time.time.Day + " " + time.time.Hour + ":" + time.time.Minute + ":" + time.time.Second + ":" + time.time.Millisecond;
							_ = num + " " + time.event_name + " " + text3 + " " + time.value;
							num++;
						}
					}
					StreamWriter streamWriter = new StreamWriter(stream2, Encoding.ASCII);
					string[] array = new string[outList.Count + 1];
					for (int i = 0; i < outList.Count; i++)
					{
						array[i] = "0";
					}
					string text4 = "";
					DateTime dateTime2 = new DateTime(DateTime.MinValue.Ticks);
					DateTime dateTime3 = new DateTime(DateTime.MaxValue.Ticks);
					bool flag4 = true;
					foreach (ED_V_TimeListEntry time2 in timeList)
					{
						dateTime2 = time2.time;
						bool flag5 = dateTime2.CompareTo(ed.stsntStartTime) > 0;
						bool flag6 = dateTime2.CompareTo(ed.stsntEndTime) < 0;
						if (!flag5 || !flag6)
						{
							continue;
						}
						if (flag4)
						{
							dateTime = dateTime2;
							dateTime3 = dateTime;
							flag4 = false;
						}
						if (dateTime2.CompareTo(dateTime3) > 0)
						{
							text4 = ((dateTime3 - dateTime).TotalMilliseconds / 1000.0).ToString();
							for (int i = 0; i < outList.Count; i++)
							{
								text4 = text4 + " " + array[i];
							}
							streamWriter.WriteLine(text4);
							array[time2.inx - 1] = time2.value;
						}
						else
						{
							array[time2.inx - 1] = time2.value;
						}
						dateTime3 = dateTime2;
					}
					text4 = ((dateTime2 - dateTime).TotalMilliseconds / 1000.0).ToString();
					for (int i = 0; i < outList.Count; i++)
					{
						text4 = text4 + " " + array[i];
					}
					if (dateTime2.CompareTo(ed.stsntStartTime) > 0 && dateTime2.CompareTo(ed.stsntEndTime) < 0)
					{
						streamWriter.WriteLine(text4);
					}
					streamWriter.Close();
					StreamWriter streamWriter2 = new StreamWriter(stream, Encoding.ASCII);
					streamWriter2.WriteLine("$MICVIEWPROTOCOLHEADER");
					streamWriter2.WriteLine("$DATETIME " + dateTime.Year + "-" + dateTime.Month.ToString("D2") + "-" + dateTime.Day.ToString("D2") + " " + dateTime.Hour.ToString("D2") + ":" + dateTime.Minute.ToString("D2"));
					streamWriter2.WriteLine("$CYCLETIME 128.0");
					streamWriter2.WriteLine("");
					streamWriter2.WriteLine("$SIGNALS " + outList.Count);
					int num2 = 1;
					foreach (ED_V_OutListEntry @out in outList)
					{
						string text5 = "";
						if (@out.isEvent)
						{
							text5 = " $SELECT";
						}
						streamWriter2.WriteLine(" " + @out.inx + " " + @out.event_name + " $INFO " + @out.dataType + text5);
						num2++;
					}
					streamWriter2.WriteLine("");
					streamWriter2.WriteLine("$STARTTIME " + dateTime.Hour.ToString("D2") + ":" + dateTime.Minute.ToString("D2") + ":" + dateTime.Second.ToString("D2") + "." + dateTime.Millisecond);
					streamWriter2.Close();
				}
				else
				{
					MessageBox.Show(TVStr.CouldNotCreateSpecOutFile, TVStr.Error);
					flag = false;
				}
				timeList.Clear();
			}
			return flag;
		}

		public bool CreateSTSNTFiles(string preDefOutDir, bool createDirectOutput, bool keepEnvVarValueBetweenEvents)
		{
			bool result = false;
			CreateOutList();
			CreateTimeList(keepEnvVarValueBetweenEvents);
			if (timeList.Count > 0)
			{
				result = GenSTSNTOutput(preDefOutDir, createDirectOutput);
			}
			return result;
		}
	}
}
