namespace TDSView
{
	public class ED_D_Event_Location
	{
		public string location;

		public string location_name;

		public string location_descr;

		public int GetRecord(string inStr)
		{
			int result = 0;
			char[] separator = new char[1] { ';' };
			char[] trimChars = new char[1] { '"' };
			string[] array = inStr.Split(separator);
			if (array.Length == 3)
			{
				location = array[0];
				location_name = array[1].Trim(trimChars);
				location_descr = array[2].Trim(trimChars);
			}
			else
			{
				result = 1;
			}
			return result;
		}
	}
}
