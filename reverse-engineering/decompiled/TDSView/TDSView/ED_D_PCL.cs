namespace TDSView
{
	public class ED_D_PCL
	{
		public string process_id;

		public string pcl_name;

		public string pcl_descr;

		public string odbs_name;

		public int GetRecord(string inStr)
		{
			int result = 0;
			char[] separator = new char[1] { ';' };
			char[] trimChars = new char[1] { '"' };
			string[] array = inStr.Split(separator);
			if (array.Length == 4)
			{
				process_id = array[0];
				pcl_name = array[1].Trim(trimChars);
				pcl_descr = array[2].Trim(trimChars);
				odbs_name = array[3].Trim(trimChars);
			}
			else if (array.Length == 3)
			{
				process_id = array[0];
				pcl_name = array[1].Trim(trimChars);
				pcl_descr = array[2].Trim(trimChars);
				odbs_name = "";
			}
			else
			{
				result = 1;
			}
			return result;
		}
	}
}
