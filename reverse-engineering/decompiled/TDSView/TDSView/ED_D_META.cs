namespace TDSView
{
	public class ED_D_META
	{
		public string meta_filetype;

		public string meta_fileversion;

		public int ed_version_int;

		public ED_D_META()
		{
			ed_version_int = 2000;
		}

		public int GetRecord(string inStr)
		{
			int result = 0;
			char[] separator = new char[1] { ';' };
			char[] trimChars = new char[1] { '"' };
			string[] array = inStr.Split(separator);
			if (array.Length == 2)
			{
				meta_filetype = array[0].Trim(trimChars);
				meta_fileversion = array[1].Trim(trimChars);
				char[] separator2 = new char[1] { '.' };
				array = meta_fileversion.Split(separator2);
				if (array.Length == 4)
				{
					ed_version_int = int.Parse(array[0]) * 1000 + int.Parse(array[1]) * 100 + int.Parse(array[2]) * 10 + int.Parse(array[3]);
				}
			}
			else
			{
				result = 1;
			}
			return result;
		}
	}
}
