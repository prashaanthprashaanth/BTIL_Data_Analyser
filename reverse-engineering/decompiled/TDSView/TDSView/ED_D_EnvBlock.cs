using System.Collections;

namespace TDSView
{
	public class ED_D_EnvBlock
	{
		public string env_block_id;

		public string env_block_descr;

		public string cycle_time;

		public ArrayList envSignal;

		public ED_D_EnvBlock()
		{
			envSignal = new ArrayList();
		}

		public int GetRecord(string inStr)
		{
			int result = 0;
			char[] separator = new char[1] { ';' };
			char[] trimChars = new char[1] { '"' };
			string[] array = inStr.Split(separator);
			if (array.Length == 3)
			{
				env_block_id = array[0].Trim(trimChars);
				env_block_descr = array[1].Trim(trimChars);
				cycle_time = array[2];
			}
			else
			{
				result = 1;
			}
			return result;
		}
	}
}
