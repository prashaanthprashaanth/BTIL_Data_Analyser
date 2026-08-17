using System;
using System.Windows.Forms;

namespace TDSView
{
	internal static class Program
	{
		[STAThread]
		private static int Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(defaultValue: false);
			MainForm mainForm = new MainForm(args);
			Application.Run(mainForm);
			return mainForm.returnValue;
		}
	}
}
