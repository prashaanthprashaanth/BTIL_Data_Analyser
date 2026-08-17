namespace TDSView
{
	public class ED_T_META
	{
		public string fileType;

		public string fileVersion;

		public int GetRecord(string inStr)
		{
			string[] array = TDSViewUtil.Line(inStr);
			int result = 0;
			char[] trimChars = new char[1] { '"' };
			if (array.Length == 2)
			{
				fileType = array[0].Trim(trimChars);
				fileVersion = array[1].Trim(trimChars);
			}
			else
			{
				result = 1;
			}
			return result;
		}
	}
}
