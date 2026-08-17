using System;

namespace TDSView
{
	public class SelectedEDVFileEventArgs : EventArgs
	{
		public string[] Name { get; set; }

		public bool addIt { get; set; }
	}
}
