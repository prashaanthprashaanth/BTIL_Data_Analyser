namespace TDSView
{
	public class ED_D_Vehicle
	{
		public string vehicle_pos;

		public string vehicle_name;

		public string vehicle_descr;

		public int GetRecord(string inStr)
		{
			int result = 0;
			char[] separator = new char[1] { ';' };
			char[] trimChars = new char[1] { '"' };
			string[] array = inStr.Split(separator);
			if (array.Length == 3)
			{
				vehicle_pos = array[0];
				vehicle_name = array[1].Trim(trimChars);
				vehicle_descr = array[2].Trim(trimChars);
			}
			else
			{
				result = 1;
			}
			return result;
		}
	}
}
