namespace TDSView
{
	public class ED_T_TEXT
	{
		public string name;

		public string pers;

		public string date;

		public string text;

		public string scope;

		public string type;

		public ED_T_TEXT(string actScope, string actType)
		{
			scope = actScope;
			type = actType;
		}

		public int GetRecord(string inStr)
		{
			string[] array = TDSViewUtil.Line(inStr);
			int result = 0;
			char[] trimChars = new char[1] { '"' };
			if (array.Length == 4)
			{
				name = array[0].Trim(trimChars);
				pers = array[1].Trim(trimChars);
				date = array[2].Trim(trimChars);
				text = array[3].Trim(trimChars);
			}
			else
			{
				result = 1;
			}
			return result;
		}
	}
}
