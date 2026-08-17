using System.Collections;
using System.Globalization;
using System.IO;

namespace TDSView
{
	public class ED_D
	{
		private enum Ed_elem
		{
			nil_e,
			meta_e,
			project_e,
			devAddress_e,
			pcl_e,
			subSystem_e,
			vehicle_e,
			event_location_e,
			code0_e,
			code1_e,
			code2_e,
			code3_e,
			priority_e,
			event_e,
			env_block_e,
			env_signal_e
		}

		public string fileName;

		public int eddVersion;

		public ArrayList meta;

		public ArrayList project;

		public ArrayList pcl;

		public ArrayList subSystem;

		public ArrayList devAddress;

		public ArrayList vehicle;

		public ArrayList event_location;

		public ArrayList code0;

		public ArrayList code1;

		public ArrayList code2;

		public ArrayList code3;

		public ArrayList priority;

		public ArrayList eventED;

		public ArrayList env_block;

		public ArrayList env_signal;

		public ED_D()
		{
			fileName = "";
			eddVersion = 2000;
			meta = new ArrayList();
			project = new ArrayList();
			devAddress = new ArrayList();
			pcl = new ArrayList();
			subSystem = new ArrayList();
			vehicle = new ArrayList();
			event_location = new ArrayList();
			code0 = new ArrayList();
			code1 = new ArrayList();
			code2 = new ArrayList();
			code3 = new ArrayList();
			priority = new ArrayList();
			eventED = new ArrayList();
			env_block = new ArrayList();
			env_signal = new ArrayList();
		}

		public void Clear()
		{
			fileName = "";
			meta.Clear();
			project.Clear();
			devAddress.Clear();
			pcl.Clear();
			subSystem.Clear();
			vehicle.Clear();
			event_location.Clear();
			code0.Clear();
			code1.Clear();
			code2.Clear();
			code3.Clear();
			priority.Clear();
			eventED.Clear();
			env_block.Clear();
			env_signal.Clear();
		}

		public ED_D_EnvBlock getOdbsEnvBlkFromProcessId(string processId)
		{
			foreach (ED_D_PCL item in pcl)
			{
				if (!item.process_id.Equals(processId))
				{
					continue;
				}
				foreach (ED_D_DEV_ADDRESS item2 in devAddress)
				{
					if (!item2.odbs_name.Equals(item.odbs_name))
					{
						continue;
					}
					foreach (ED_D_EnvBlock item3 in env_block)
					{
						if (item3.env_block_id.Equals(item2.env_block_name))
						{
							return item3;
						}
					}
				}
			}
			return null;
		}

		public int Read_ED_D_File(string fileName)
		{
			int num = 0;
			int num2 = 0;
			Ed_elem ed_elem = Ed_elem.nil_e;
			StreamReader streamReader = File.OpenText(fileName);
			this.fileName = fileName;
			string text = "";
			while ((text = streamReader.ReadLine()) != null)
			{
				text = text.Trim();
				if (text.Equals("<META>"))
				{
					ed_elem = Ed_elem.meta_e;
					continue;
				}
				if (text.Equals("<PROJECT>"))
				{
					ed_elem = Ed_elem.project_e;
					continue;
				}
				if (text.Equals("<DEV_ADDRESS>"))
				{
					ed_elem = Ed_elem.devAddress_e;
					continue;
				}
				if (text.Equals("<PCL>"))
				{
					ed_elem = Ed_elem.pcl_e;
					continue;
				}
				if (text.Equals("<VEHICLE>"))
				{
					ed_elem = Ed_elem.vehicle_e;
					continue;
				}
				if (text.Equals("<SUBSYSTEM>"))
				{
					ed_elem = Ed_elem.subSystem_e;
					continue;
				}
				if (text.Equals("<EVENT_LOCATION>"))
				{
					ed_elem = Ed_elem.event_location_e;
					continue;
				}
				if (text.Equals("<CODE_0>"))
				{
					ed_elem = Ed_elem.code0_e;
					continue;
				}
				if (text.Equals("<CODE_1>"))
				{
					ed_elem = Ed_elem.code1_e;
					continue;
				}
				if (text.Equals("<CODE_2>"))
				{
					ed_elem = Ed_elem.code2_e;
					continue;
				}
				if (text.Equals("<CODE_3>"))
				{
					ed_elem = Ed_elem.code3_e;
					continue;
				}
				if (text.Equals("<PRIORITY>"))
				{
					ed_elem = Ed_elem.priority_e;
					continue;
				}
				if (text.Equals("<EVENT>"))
				{
					ed_elem = Ed_elem.event_e;
					continue;
				}
				if (text.Equals("<ENV_BLOCK>"))
				{
					ed_elem = Ed_elem.env_block_e;
					continue;
				}
				if (text.Equals("<ENV_SIGNAL>"))
				{
					ed_elem = Ed_elem.env_signal_e;
					continue;
				}
				if (text.StartsWith("<") && text.EndsWith(">"))
				{
					ed_elem = Ed_elem.nil_e;
					continue;
				}
				switch (ed_elem)
				{
				case Ed_elem.meta_e:
				{
					ED_D_META eD_D_META = new ED_D_META();
					if (eD_D_META.GetRecord(text) > 0)
					{
						num++;
					}
					meta.Add(eD_D_META);
					eddVersion = eD_D_META.ed_version_int;
					num2++;
					break;
				}
				case Ed_elem.project_e:
				{
					ED_D_PROJECT eD_D_PROJECT = new ED_D_PROJECT();
					if (eD_D_PROJECT.GetRecord(text) > 0)
					{
						num++;
					}
					project.Add(eD_D_PROJECT);
					num2++;
					break;
				}
				case Ed_elem.devAddress_e:
				{
					ED_D_DEV_ADDRESS eD_D_DEV_ADDRESS = new ED_D_DEV_ADDRESS();
					if (eD_D_DEV_ADDRESS.GetRecord(text, eddVersion) > 0)
					{
						num++;
					}
					devAddress.Add(eD_D_DEV_ADDRESS);
					num2++;
					break;
				}
				case Ed_elem.pcl_e:
				{
					ED_D_PCL eD_D_PCL = new ED_D_PCL();
					if (eD_D_PCL.GetRecord(text) > 0)
					{
						num++;
					}
					pcl.Add(eD_D_PCL);
					num2++;
					break;
				}
				case Ed_elem.subSystem_e:
				{
					ED_D_Subsystem eD_D_Subsystem = new ED_D_Subsystem();
					if (eD_D_Subsystem.GetRecord(text) > 0)
					{
						num++;
					}
					subSystem.Add(eD_D_Subsystem);
					num2++;
					break;
				}
				case Ed_elem.vehicle_e:
				{
					ED_D_Vehicle eD_D_Vehicle = new ED_D_Vehicle();
					if (eD_D_Vehicle.GetRecord(text) > 0)
					{
						num++;
					}
					num2++;
					vehicle.Add(eD_D_Vehicle);
					break;
				}
				case Ed_elem.event_location_e:
				{
					ED_D_Event_Location eD_D_Event_Location = new ED_D_Event_Location();
					if (eD_D_Event_Location.GetRecord(text) > 0)
					{
						num++;
					}
					num2++;
					event_location.Add(eD_D_Event_Location);
					break;
				}
				case Ed_elem.code0_e:
				{
					ED_D_Code eD_D_Code3 = new ED_D_Code();
					if (eD_D_Code3.GetRecord(text) > 0)
					{
						num++;
					}
					num2++;
					code0.Add(eD_D_Code3);
					break;
				}
				case Ed_elem.code1_e:
				{
					ED_D_Code eD_D_Code2 = new ED_D_Code();
					if (eD_D_Code2.GetRecord(text) > 0)
					{
						num++;
					}
					num2++;
					code1.Add(eD_D_Code2);
					break;
				}
				case Ed_elem.code2_e:
				{
					ED_D_Code eD_D_Code = new ED_D_Code();
					if (eD_D_Code.GetRecord(text) > 0)
					{
						num++;
					}
					num2++;
					code2.Add(eD_D_Code);
					break;
				}
				case Ed_elem.code3_e:
				{
					ED_D_Code eD_D_Code4 = new ED_D_Code();
					if (eD_D_Code4.GetRecord(text) > 0)
					{
						num++;
					}
					num2++;
					code3.Add(eD_D_Code4);
					break;
				}
				case Ed_elem.priority_e:
				{
					ED_D_Priority eD_D_Priority = new ED_D_Priority();
					if (eD_D_Priority.GetRecord(text) > 0)
					{
						num++;
					}
					num2++;
					priority.Add(eD_D_Priority);
					break;
				}
				case Ed_elem.event_e:
				{
					ED_D_Event eD_D_Event = new ED_D_Event();
					if (eD_D_Event.GetRecord(text, eddVersion) > 0)
					{
						num++;
					}
					num2++;
					eventED.Add(eD_D_Event);
					break;
				}
				case Ed_elem.env_block_e:
				{
					ED_D_EnvBlock eD_D_EnvBlock = new ED_D_EnvBlock();
					if (eD_D_EnvBlock.GetRecord(text) > 0)
					{
						num++;
					}
					num2++;
					env_block.Add(eD_D_EnvBlock);
					break;
				}
				case Ed_elem.env_signal_e:
				{
					ED_D_EnvSignal eD_D_EnvSignal = new ED_D_EnvSignal();
					if (eD_D_EnvSignal.GetRecord(text) > 0)
					{
						num++;
					}
					num2++;
					env_signal.Add(eD_D_EnvSignal);
					break;
				}
				}
			}
			if (num2 > 10)
			{
				if (meta.Count == 0)
				{
					ED_D_META eD_D_META2 = new ED_D_META();
					eD_D_META2.ed_version_int = 2000;
					eD_D_META2.meta_filetype = "";
					eD_D_META2.meta_fileversion = "002.000.000.000";
					meta.Add(eD_D_META2);
					eddVersion = eD_D_META2.ed_version_int;
				}
			}
			else
			{
				num++;
			}
			return num;
		}

		public string SubSystemName(int subSystemNr)
		{
			string text = "";
			foreach (ED_D_Subsystem item in subSystem)
			{
				if (int.Parse(item.subsystem_id, NumberStyles.AllowHexSpecifier) == subSystemNr)
				{
					text = item.subsystem_name;
					break;
				}
			}
			if (text.Equals(""))
			{
				text = subSystemNr.ToString();
			}
			return text;
		}

		public ED_D_Code ECode0Descr(int eCode)
		{
			ED_D_Code result = null;
			foreach (ED_D_Code item in code0)
			{
				if (int.Parse(item.errorcode, NumberStyles.AllowHexSpecifier) == eCode)
				{
					result = item;
					break;
				}
			}
			return result;
		}

		public ED_D_Code ECode1Descr(int eCode)
		{
			ED_D_Code result = null;
			foreach (ED_D_Code item in code1)
			{
				if (int.Parse(item.errorcode, NumberStyles.AllowHexSpecifier) == eCode)
				{
					result = item;
					break;
				}
			}
			return result;
		}

		public ED_D_Code ECode2Descr(int eCode)
		{
			ED_D_Code result = null;
			foreach (ED_D_Code item in code2)
			{
				if (int.Parse(item.errorcode, NumberStyles.AllowHexSpecifier) == eCode)
				{
					result = item;
					break;
				}
			}
			return result;
		}

		public ED_D_Code ECode3Descr(int eCode)
		{
			ED_D_Code result = null;
			foreach (ED_D_Code item in code3)
			{
				if (int.Parse(item.errorcode, NumberStyles.AllowHexSpecifier) == eCode)
				{
					result = item;
					break;
				}
			}
			return result;
		}

		public string SubSystemDescr(int subSystemNr)
		{
			string text = "";
			foreach (ED_D_Subsystem item in subSystem)
			{
				if (int.Parse(item.subsystem_id, NumberStyles.AllowHexSpecifier) == subSystemNr)
				{
					text = item.subsystem_descr;
					break;
				}
			}
			if (text.Equals(""))
			{
				text = subSystemNr.ToString();
			}
			return text;
		}

		public string GetPriorityString(byte prio)
		{
			string text = "";
			foreach (ED_D_Priority item in priority)
			{
				byte b = byte.Parse(item.prio, NumberStyles.AllowHexSpecifier);
				if (b == prio)
				{
					text = item.prio_name;
					break;
				}
			}
			if (text.Equals(""))
			{
				text = prio.ToString();
			}
			return text;
		}
	}
}
