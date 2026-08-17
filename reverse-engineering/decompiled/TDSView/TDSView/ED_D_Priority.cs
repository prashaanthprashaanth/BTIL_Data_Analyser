namespace TDSView
{
	public class ED_D_Priority
	{
		public string prio;

		public string prio_name;

		public string prio_descr;

		public int GetRecord(string inStr)
		{
			int result = 0;
			char[] separator = new char[1] { ';' };
			char[] trimChars = new char[1] { '"' };
			string[] array = inStr.Split(separator);
			if (array.Length == 3)
			{
				prio = array[0];
				prio_name = array[1].Trim(trimChars);
				prio_descr = array[2].Trim(trimChars);
			}
			else
			{
				result = 1;
			}
			return result;
		}
	}
}
