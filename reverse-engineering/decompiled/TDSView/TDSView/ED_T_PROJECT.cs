namespace TDSView
{
	public class ED_T_PROJECT
	{
		public string prj_name;

		public string prj_version;

		public string prj_language;

		public string prj_descr;

		public int GetRecord(string inStr)
		{
			int result = 0;
			char[] separator = new char[1] { ';' };
			char[] trimChars = new char[1] { '"' };
			string[] array = inStr.Split(separator);
			if (array.Length == 4)
			{
				prj_name = array[0].Trim(trimChars);
				prj_version = array[1].Trim(trimChars);
				prj_language = array[2].Trim(trimChars);
				prj_descr = array[3].Trim(trimChars);
			}
			else
			{
				result = 1;
			}
			return result;
		}
	}
}
