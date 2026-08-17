namespace TDSView
{
	public class ED_D_DEV_ADDRESS
	{
		public string odbs_name;

		public string odbs_address;

		public string odbs_address_ipt;

		public string odbs_descr;

		public string env_block_name;

		public int GetRecord(string inStr, int eddVersion)
		{
			int result = 0;
			char[] separator = new char[1] { ';' };
			char[] trimChars = new char[1] { '"' };
			string[] array = inStr.Split(separator);
			if (array.Length == 5)
			{
				odbs_name = array[0].Trim(trimChars);
				odbs_address = array[1];
				odbs_address_ipt = array[2];
				odbs_descr = array[3].Trim(trimChars);
				env_block_name = array[4].Trim(trimChars);
			}
			else if (array.Length == 3)
			{
				odbs_name = array[0].Trim(trimChars);
				odbs_address = array[1];
				odbs_descr = array[2].Trim(trimChars);
			}
			else
			{
				result = 1;
			}
			return result;
		}
	}
}
