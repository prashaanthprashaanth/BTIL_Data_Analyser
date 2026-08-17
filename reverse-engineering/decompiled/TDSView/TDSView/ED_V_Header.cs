namespace TDSView
{
	public class ED_V_Header
	{
		public ushort byte_order_control;

		public string file_id;

		public string prefix;

		public string ed_version;

		public int ed_version_int;

		public string prj_name;

		public string prj_version;

		public string diagnosis_system;

		public string odbs_adr;

		public string vehicle_name;

		public string date_read_out;

		public string time_read_out;

		public string event_copy;

		public byte[] property_option;

		public ED_V_Header()
		{
			property_option = new byte[8];
		}
	}
}
