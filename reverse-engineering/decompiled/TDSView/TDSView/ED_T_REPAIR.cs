namespace TDSView
{
	public class ED_T_REPAIR
	{
		public string name;

		public string rep_pers;

		public string rep_date;

		public string rep_text;

		public int GetRecord(string inStr)
		{
			string[] array = TDSViewUtil.Line(inStr);
			int result = 0;
			char[] trimChars = new char[1] { '"' };
			if (array.Length == 4)
			{
				name = array[0].Trim(trimChars);
				rep_pers = array[1].Trim(trimChars);
				rep_date = array[2].Trim(trimChars);
				rep_text = array[3].Trim(trimChars);
			}
			else
			{
				result = 1;
			}
			return result;
		}
	}
}
