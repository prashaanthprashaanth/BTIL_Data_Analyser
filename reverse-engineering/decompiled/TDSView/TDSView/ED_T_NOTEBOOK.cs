namespace TDSView
{
	public class ED_T_NOTEBOOK
	{
		public string name;

		public string ntb_pers;

		public string ntb_date;

		public string ntb_text;

		public int GetRecord(string inStr)
		{
			string[] array = TDSViewUtil.Line(inStr);
			int result = 0;
			char[] trimChars = new char[1] { '"' };
			if (array.Length == 4)
			{
				name = array[0].Trim(trimChars);
				ntb_pers = array[1].Trim(trimChars);
				ntb_date = array[2].Trim(trimChars);
				ntb_text = array[3].Trim(trimChars);
			}
			else
			{
				result = 1;
			}
			return result;
		}
	}
}
