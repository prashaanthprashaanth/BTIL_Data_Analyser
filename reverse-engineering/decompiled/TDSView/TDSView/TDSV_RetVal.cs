namespace TDSView
{
	public class TDSV_RetVal
	{
		public enum ReturnValue
		{
			OK,
			FILE_NOT_FOUND,
			INVALID_FILE
		}

		public ReturnValue rV;

		public string str;
	}
}
