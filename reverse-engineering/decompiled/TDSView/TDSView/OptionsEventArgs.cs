using System;

namespace TDSView
{
	public class OptionsEventArgs : EventArgs
	{
		public string rootDir { get; set; }

		public bool treeSel { get; set; }
	}
}
