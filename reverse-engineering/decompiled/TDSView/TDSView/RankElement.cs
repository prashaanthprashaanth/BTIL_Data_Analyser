namespace TDSView
{
	public class RankElement
	{
		public ushort event_cnt;

		public ushort event_id;

		public ushort prc_id;

		public string eventDescr;

		public string signalName;

		public string eCode0;

		public RankElement()
		{
			event_cnt = 0;
			event_id = 0;
			prc_id = 0;
			eventDescr = "";
			signalName = "";
			eCode0 = "";
		}
	}
}
