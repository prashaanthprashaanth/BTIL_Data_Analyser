namespace TDSView
{
	public class ED_D_PROJECT
	{
		public string prj_name;

		public string prj_version;

		public string diagnosis_system;

		public string prj_flags1;

		public string prj_flags2;

		public string prj_language;

		public string prj_descr;

		public int GetRecord(string inStr)
		{
			int result = 0;
			char[] separator = new char[1] { ';' };
			char[] trimChars = new char[1] { '"' };
			string[] array = inStr.Split(separator);
			if (array.Length == 7)
			{
				prj_name = array[0].Trim(trimChars);
				prj_version = array[1].Trim(trimChars);
				diagnosis_system = array[2].Trim(trimChars);
				prj_flags1 = array[3];
				prj_flags2 = array[4];
				prj_language = array[5].Trim(trimChars);
				prj_descr = array[6].Trim(trimChars);
			}
			else
			{
				result = 1;
			}
			return result;
		}
	}
}
