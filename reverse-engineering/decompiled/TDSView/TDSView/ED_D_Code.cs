namespace TDSView
{
	public class ED_D_Code
	{
		public string errorcode;

		public string code_name;

		public string code_descr;

		public int GetRecord(string inStr)
		{
			int result = 0;
			char[] separator = new char[1] { ';' };
			char[] trimChars = new char[1] { '"' };
			string[] array = inStr.Split(separator);
			if (array.Length == 3)
			{
				errorcode = array[0];
				code_name = array[1].Trim(trimChars);
				code_descr = array[2].Trim(trimChars);
			}
			else
			{
				result = 1;
			}
			return result;
		}
	}
}
