namespace TDSView
{
	public class ED_D_Event
	{
		public string process_id;

		public string event_id;

		public string event_name;

		public string event_descr;

		public string rep_prio_active;

		public string rep_prio_passive;

		public string extra_prio;

		public string extra_attrib1;

		public string extra_attrib2;

		public string env_block_id;

		public string extra_attrib3;

		public string extra_attrib4;

		public string extra_attrib5;

		public string subsys_id;

		public int GetRecord(string inStr, int eddVersion)
		{
			string[] array = TDSViewUtil.Line(inStr);
			int result = 0;
			char[] trimChars = new char[1] { '"' };
			int num = 0;
			if (array.Length == 10)
			{
				process_id = array[0];
				event_id = array[1];
				event_name = array[2].Trim(trimChars);
				event_descr = array[3].Trim(trimChars);
				rep_prio_active = array[4];
				rep_prio_passive = array[5];
				extra_prio = array[6];
				extra_attrib1 = array[7];
				extra_attrib2 = array[8];
				env_block_id = array[9].Trim(trimChars);
			}
			else if (array.Length == 11)
			{
				process_id = array[num++];
				event_id = array[num++];
				event_name = array[num++].Trim(trimChars);
				event_descr = array[num++].Trim(trimChars);
				rep_prio_active = array[num++];
				rep_prio_passive = array[num++];
				extra_prio = array[num++];
				extra_attrib1 = array[num++];
				extra_attrib2 = array[num++];
				env_block_id = array[num++];
				subsys_id = array[num++];
			}
			else if (array.Length > 11)
			{
				process_id = array[num++];
				event_id = array[num++];
				event_name = array[num++].Trim(trimChars);
				event_descr = array[num++].Trim(trimChars);
				rep_prio_active = array[num++];
				rep_prio_passive = array[num++];
				extra_prio = array[num++];
				extra_attrib1 = array[num++];
				extra_attrib2 = array[num++];
				extra_attrib3 = array[num++];
				extra_attrib4 = array[num++];
				extra_attrib5 = array[num++];
				env_block_id = array[num++];
				subsys_id = array[num++];
			}
			else
			{
				result = 1;
			}
			return result;
		}
	}
}
