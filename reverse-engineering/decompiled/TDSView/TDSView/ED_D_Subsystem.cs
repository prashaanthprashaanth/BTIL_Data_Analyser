namespace TDSView
{
	public class ED_D_Subsystem
	{
		public string subsystem_id;

		public string subsystem_name;

		public string subsystem_descr;

		public int GetRecord(string inStr)
		{
			int result = 0;
			char[] separator = new char[1] { ';' };
			char[] trimChars = new char[1] { '"' };
			string[] array = inStr.Split(separator);
			if (array.Length == 3)
			{
				subsystem_id = array[0];
				subsystem_name = array[1].Trim(trimChars);
				subsystem_descr = array[2].Trim(trimChars);
			}
			else
			{
				result = 1;
			}
			return result;
		}
	}
}
