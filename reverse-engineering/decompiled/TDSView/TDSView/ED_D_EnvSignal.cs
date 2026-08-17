namespace TDSView
{
	public class ED_D_EnvSignal
	{
		public string env_block_id;

		public string env_word;

		public string env_bit;

		public string env_byte;

		public string env_type;

		public string env_name;

		public string env_descr;

		public string disp_type;

		public string coeff_A;

		public string coeff_B;

		public string disp_dim;

		public string disp_normal;

		public string disp_format;

		public int GetRecord(string inStr)
		{
			int result = 0;
			char[] trimChars = new char[1] { '"' };
			string[] array = TDSViewUtil.Line(inStr);
			if (array.Length == 13)
			{
				env_block_id = array[0].Trim(trimChars);
				env_word = array[1];
				env_bit = array[2];
				env_byte = array[3];
				env_type = array[4];
				env_name = array[5].Trim(trimChars);
				env_descr = array[6].Trim(trimChars);
				disp_type = array[7];
				coeff_A = array[8];
				coeff_B = array[9];
				disp_dim = array[10].Trim(trimChars);
				disp_normal = array[11];
				disp_format = array[12];
			}
			else
			{
				result = 1;
			}
			return result;
		}
	}
}
