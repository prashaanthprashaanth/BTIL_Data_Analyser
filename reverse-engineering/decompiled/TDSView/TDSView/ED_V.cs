using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;

namespace TDSView
{
	public class ED_V
	{
		public delegate bool envDataOvl(string text);

		public string fileName;

		public ED_V_Header header;

		public ArrayList data;

		public BinaryReader binReader;

		public ED_D ed_d;

		public ED_T ed_t;

		public bool useLocalPCTime;

		public double timeShift;

		public envDataOvl envDataOvlMethod;

		public ED_V(bool useLocalTime, double timeShiftFromOptions)
		{
			fileName = "";
			header = new ED_V_Header();
			data = new ArrayList();
			useLocalPCTime = useLocalTime;
			timeShift = timeShiftFromOptions;
		}

		private string ReadString(int byteCount)
		{
			byte[] array = new byte[byteCount];
			array = binReader.ReadBytes(byteCount);
			UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
			string text = unicodeEncoding.GetString(array);
			int num = text.IndexOf("\0");
			if (num >= 0)
			{
				text = text.Remove(num);
			}
			return text;
		}

		private int GetHeader()
		{
			int result = 1;
			header.byte_order_control = binReader.ReadUInt16();
			header.file_id = ReadString(16);
			header.prefix = ReadString(8);
			if (header.prefix.Equals("ED_V"))
			{
				header.ed_version = ReadString(8);
				header.ed_version_int = int.Parse(header.ed_version);
				header.prj_name = ReadString(16);
				header.prj_version = ReadString(30);
				header.diagnosis_system = ReadString(32);
				if (header.ed_version_int >= 2200)
				{
					header.odbs_adr = ReadString(202);
				}
				else
				{
					header.odbs_adr = ReadString(6);
				}
				header.vehicle_name = ReadString(32);
				header.date_read_out = ReadString(16);
				header.time_read_out = ReadString(12);
				header.event_copy = ReadString(2);
				header.property_option = binReader.ReadBytes(8);
				result = 0;
			}
			return result;
		}

		private DateTime ConvertFromUnixTimestamp(uint timestamp)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			if (useLocalPCTime)
			{
				dateTime = dateTime.ToLocalTime();
			}
			return dateTime.AddSeconds(timestamp).AddHours(timeShift);
		}

		private ED_V_Data GetData()
		{
			ED_V_Data eD_V_Data = new ED_V_Data();
			ushort num = binReader.ReadUInt16();
			ushort num2 = binReader.ReadUInt16();
			eD_V_Data.reference_nr = (uint)(num + num2 * 65536);
			eD_V_Data.event_id = binReader.ReadUInt16();
			eD_V_Data.limit = binReader.ReadByte();
			eD_V_Data.vehicle_pos = binReader.ReadByte();
			eD_V_Data.process_id = binReader.ReadByte();
			eD_V_Data.subsystem_nr = binReader.ReadByte();
			eD_V_Data.location = binReader.ReadByte();
			eD_V_Data.prio = binReader.ReadByte();
			eD_V_Data.errorcode_0 = binReader.ReadUInt16();
			eD_V_Data.errorcode_1 = binReader.ReadUInt16();
			eD_V_Data.errorcode_2 = binReader.ReadUInt16();
			eD_V_Data.errorcode_3 = binReader.ReadUInt16();
			eD_V_Data.acknow_0 = binReader.ReadByte();
			eD_V_Data.acknow_1 = binReader.ReadByte();
			eD_V_Data.acknow_2 = binReader.ReadByte();
			eD_V_Data.acknow_3 = binReader.ReadByte();
			eD_V_Data.err_code_mism = binReader.ReadByte();
			eD_V_Data.active = binReader.ReadByte();
			eD_V_Data.deleted = binReader.ReadByte();
			eD_V_Data.uploaded = binReader.ReadByte();
			eD_V_Data.event_cnt = binReader.ReadUInt16();
			eD_V_Data.start_time_sec = binReader.ReadUInt32();
			eD_V_Data.start_time_rel = binReader.ReadUInt16();
			eD_V_Data.start_time = ConvertFromUnixTimestamp(eD_V_Data.start_time_sec);
			double value = (double)(int)eD_V_Data.start_time_rel / 65535.0 * 1000.0;
			eD_V_Data.start_time = eD_V_Data.start_time.AddMilliseconds(value);
			eD_V_Data.end_time_sec = binReader.ReadUInt32();
			if (eD_V_Data.end_time_sec == 0)
			{
				eD_V_Data.end_time_sec = uint.MaxValue;
			}
			eD_V_Data.end_time_rel = binReader.ReadUInt16();
			eD_V_Data.end_time = ConvertFromUnixTimestamp(eD_V_Data.end_time_sec);
			value = (double)(int)eD_V_Data.end_time_rel / 65535.0 * 1000.0;
			eD_V_Data.end_time = eD_V_Data.end_time.AddMilliseconds(value);
			if (header.ed_version_int >= 2200)
			{
				ushort num3 = binReader.ReadUInt16();
				ushort num4 = binReader.ReadUInt16();
				uint value2 = (uint)(num3 + num4 * 65536);
				eD_V_Data.latitude = BitConverter.ToSingle(BitConverter.GetBytes(value2), 0);
				num3 = binReader.ReadUInt16();
				num4 = binReader.ReadUInt16();
				value2 = (uint)(num3 + num4 * 65536);
				eD_V_Data.longitude = BitConverter.ToSingle(BitConverter.GetBytes(value2), 0);
				num3 = binReader.ReadUInt16();
				num4 = binReader.ReadUInt16();
				value2 = (uint)(num3 + num4 * 65536);
				eD_V_Data.altitude = BitConverter.ToSingle(BitConverter.GetBytes(value2), 0);
				num3 = binReader.ReadUInt16();
				num4 = binReader.ReadUInt16();
				value2 = (uint)(num3 + num4 * 65536);
				eD_V_Data.speed = BitConverter.ToSingle(BitConverter.GetBytes(value2), 0);
				num3 = binReader.ReadUInt16();
				num4 = binReader.ReadUInt16();
				value2 = (uint)(num3 + num4 * 65536);
				eD_V_Data.heading = BitConverter.ToSingle(BitConverter.GetBytes(value2), 0);
				num3 = binReader.ReadUInt16();
				num4 = binReader.ReadUInt16();
				eD_V_Data.UTC_time = (uint)(num3 + num4 * 65536);
				num3 = binReader.ReadUInt16();
				num4 = binReader.ReadUInt16();
				eD_V_Data.odometer = (uint)(num3 + num4 * 65536);
				num3 = binReader.ReadUInt16();
				num4 = binReader.ReadUInt16();
				eD_V_Data.trip = (uint)(num3 + num4 * 65536);
				num3 = binReader.ReadUInt16();
				num4 = binReader.ReadUInt16();
				eD_V_Data.reserved_0 = (uint)(num3 + num4 * 65536);
				num3 = binReader.ReadUInt16();
				num4 = binReader.ReadUInt16();
				eD_V_Data.reserved_1 = (uint)(num3 + num4 * 65536);
				num3 = binReader.ReadUInt16();
				num4 = binReader.ReadUInt16();
				eD_V_Data.reserved_2 = (uint)(num3 + num4 * 65536);
				num3 = binReader.ReadUInt16();
				num4 = binReader.ReadUInt16();
				eD_V_Data.reserved_3 = (uint)(num3 + num4 * 65536);
			}
			eD_V_Data.env_data_cnt_start = binReader.ReadByte();
			eD_V_Data.block_cnt_start = binReader.ReadByte();
			eD_V_Data.old_block_idx_start = binReader.ReadByte();
			eD_V_Data.trx_block_idx_start = binReader.ReadByte();
			eD_V_Data.env_data_cnt_end = binReader.ReadByte();
			eD_V_Data.block_cnt_end = binReader.ReadByte();
			eD_V_Data.old_block_idx_end = binReader.ReadByte();
			eD_V_Data.trx_block_idx_end = binReader.ReadByte();
			int num5 = 26;
			if (header.ed_version_int >= 2200)
			{
				eD_V_Data.dbs_env_data_cnt_start = binReader.ReadByte();
				eD_V_Data.dbs_block_cnt_start = binReader.ReadByte();
				eD_V_Data.dbs_old_block_idx_start = binReader.ReadByte();
				eD_V_Data.dbs_trx_block_idx_start = binReader.ReadByte();
				eD_V_Data.dbs_env_data_cnt_end = binReader.ReadByte();
				eD_V_Data.dbs_block_cnt_end = binReader.ReadByte();
				eD_V_Data.dbs_old_block_idx_end = binReader.ReadByte();
				eD_V_Data.dbs_trx_block_idx_end = binReader.ReadByte();
				num5 = 54;
			}
			int num6 = num5 + eD_V_Data.env_data_cnt_start * eD_V_Data.block_cnt_start;
			int num7 = num6 + (eD_V_Data.env_data_cnt_end * eD_V_Data.block_cnt_end - 1);
			int num8 = 2 * (num7 - num5 + 1);
			eD_V_Data.envDataArr = new byte[num8];
			eD_V_Data.envDataArr = binReader.ReadBytes(num8);
			if (header.ed_version_int >= 2200)
			{
				num5 += num8;
				int num9 = num5 + eD_V_Data.dbs_env_data_cnt_start * eD_V_Data.dbs_block_cnt_start;
				int num10 = num9 + (eD_V_Data.dbs_env_data_cnt_end * eD_V_Data.dbs_block_cnt_end - 1);
				int num11 = 2 * (num10 - num5 + 1);
				eD_V_Data.dbs_envDataArr = new byte[num11];
				eD_V_Data.dbs_envDataArr = binReader.ReadBytes(num11);
			}
			return eD_V_Data;
		}

		public void Clear()
		{
			fileName = "";
			data.Clear();
		}

		public void InitCallBack(envDataOvl envDataDlg)
		{
			envDataOvlMethod = envDataDlg;
		}

		public string GetEnvValue(ED_D_EnvSignal evs, int blkOffset, byte[] envData, bool valueOnly, bool hexAllowed)
		{
			string result = "";
			if (envData == null)
			{
				if (envDataOvlMethod != null)
				{
					envDataOvlMethod(TVStr.DiscrepanceEnvData);
				}
				return "????";
			}
			if (envData.Length == 0)
			{
				return "????";
			}
			int num = blkOffset + int.Parse(evs.env_word, NumberStyles.AllowHexSpecifier) * 2 - 2;
			if (num >= envData.Length)
			{
				if (envDataOvlMethod != null)
				{
					envDataOvlMethod(TVStr.DiscrepanceEnvData);
				}
				return "????";
			}
			if (evs.env_type.Equals("B"))
			{
				int num2 = int.Parse(evs.env_bit, NumberStyles.AllowHexSpecifier);
				if (num2 >= 8)
				{
					num++;
					num2 -= 8;
				}
				int num3 = 1;
				num3 <<= num2;
				int num4 = envData[num];
				if (valueOnly)
				{
					result = "0";
					if ((num4 & num3) != 0)
					{
						result = "1";
					}
				}
				else
				{
					result = "OFF";
					if ((num4 & num3) != 0)
					{
						result = "ON";
					}
				}
			}
			else if (evs.env_type.Equals("Y"))
			{
				if (evs.env_byte == "H")
				{
					num++;
				}
				result = ((evs.disp_type == "H") ? ((!hexAllowed) ? envData[num].ToString("D") : (envData[num].ToString("X2") + "H")) : ((evs.disp_type == "O") ? envData[num].ToString() : ((evs.disp_type == "M") ? ((sbyte)envData[num]).ToString() : ((!hexAllowed) ? envData[num].ToString("D") : (envData[num].ToString("X2") + "H")))));
			}
			else if (evs.env_type.Equals("W"))
			{
				if (evs.disp_type == "A" || evs.disp_type == "M")
				{
					short num5 = Convert.ToInt16(envData[num]);
					short num6 = Convert.ToInt16(envData[num + 1]);
					num6 *= 256;
					num5 += num6;
					if (evs.disp_type == "A")
					{
						double num7 = double.Parse(evs.coeff_A, CultureInfo.InvariantCulture);
						double num8 = double.Parse(evs.coeff_B, CultureInfo.InvariantCulture);
						double num9 = Convert.ToDouble(num5);
						result = (num9 * num7 + num8).ToString("F" + evs.disp_format);
					}
					else if (evs.disp_type == "M")
					{
						result = num5.ToString("D");
					}
				}
				else
				{
					int num10 = Convert.ToInt32(envData[num]);
					int num11 = Convert.ToInt32(envData[num + 1]);
					num11 *= 256;
					num10 += num11;
					if (evs.disp_type == "O")
					{
						result = num10.ToString("D");
					}
					else if (evs.disp_type == "H")
					{
						result = ((!hexAllowed) ? num10.ToString("D") : num10.ToString("X4"));
					}
				}
			}
			else if (evs.env_type.Equals("D"))
			{
				int num12 = Convert.ToInt32(envData[num]);
				int num13 = Convert.ToInt32(envData[num + 1]);
				int num14 = Convert.ToInt32(envData[num + 2]);
				int num15 = Convert.ToInt32(envData[num + 3]);
				num13 *= 256;
				num14 *= 65536;
				num15 *= 16777216;
				num12 += num13;
				num12 += num14;
				num12 += num15;
				if (evs.disp_type == "O")
				{
					result = num12.ToString("D");
				}
				else if (evs.disp_type == "H")
				{
					result = ((!hexAllowed) ? num12.ToString("D") : num12.ToString("X16"));
				}
			}
			else
			{
				if (!evs.env_type.Equals("F"))
				{
					return "";
				}
				byte[] value = new byte[4]
				{
					envData[num],
					envData[num + 1],
					envData[num + 2],
					envData[num + 3]
				};
				result = BitConverter.ToSingle(value, 0).ToString("F02");
			}
			return result;
		}

		public int Read_ED_V_File(string fileName)
		{
			int num = 0;
			bool flag = false;
			if (File.Exists(fileName))
			{
				bool flag2 = false;
				this.fileName = fileName;
				binReader = new BinaryReader(File.Open(fileName, FileMode.Open));
				try
				{
					num = GetHeader();
					int num2 = 1;
					if (num == 0)
					{
						flag2 = true;
						while (true)
						{
							ED_V_Data eD_V_Data = GetData();
							eD_V_Data.uniqueRef = num2++;
							data.Add(eD_V_Data);
						}
					}
				}
				catch (EndOfStreamException ex)
				{
					if (1 == 0 || !flag2)
					{
						num = 1;
					}
					_ = ex.GetType().Name;
				}
				finally
				{
					binReader.Close();
				}
			}
			else
			{
				num = 2;
			}
			return num;
		}

		public int Read_ED_V_FileHeader(string fileName)
		{
			int result = 0;
			bool flag = false;
			if (File.Exists(fileName))
			{
				this.fileName = fileName;
				binReader = new BinaryReader(File.Open(fileName, FileMode.Open));
				try
				{
					result = GetHeader();
				}
				catch (EndOfStreamException ex)
				{
					if (1 == 0)
					{
						result = 1;
					}
					_ = ex.GetType().Name;
				}
				finally
				{
					binReader.Close();
				}
			}
			else
			{
				result = 2;
			}
			return result;
		}
	}
}
