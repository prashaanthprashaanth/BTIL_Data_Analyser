using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TDSView
{
	public class TDSViewUtil
	{
		private enum SHELL_VIEW
		{
			LARGEICON = 28713,
			SMALLICON,
			LIST,
			REPORT,
			THUMBNAIL,
			TILE
		}

		private const int WM_COMMAND = 273;

		private const string shellDll_defView = "SHELLDLL_DefView";

		private const int WaitForShell_Milliseconds = 500;

		public static string ofdTitle;

		public static string GetSpanString(TimeSpan tSpan, bool startStopTimeMilliSec)
		{
			string text = "";
			if (tSpan.Days != 0)
			{
				text = tSpan.Days + ".";
			}
			text = text + tSpan.Hours.ToString("d2") + ":";
			text = text + tSpan.Minutes.ToString("d2") + ":";
			text += tSpan.Seconds.ToString("d2");
			if (startStopTimeMilliSec)
			{
				text = text + "." + tSpan.Milliseconds.ToString("d3");
			}
			return text;
		}

		public static Encoding GetEncoding(string language)
		{
			if (language.Equals("ZH"))
			{
				return Encoding.UTF8;
			}
			return Encoding.GetEncoding("ISO-8859-1");
		}

		public static string TimeDate2Str(DateTime dateTime)
		{
			return dateTime.ToString(CultureInfo.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern).Replace("Z", "");
		}

		public static string TimeDate2Str(DateTime dateTime, int milliSec)
		{
			return dateTime.ToString(CultureInfo.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern).Replace("Z", "") + "." + milliSec.ToString("D3");
		}

		public static DateTime Str2TimeDate(string timeDateStr)
		{
			return DateTime.Parse(timeDateStr);
		}

		public static string[] Line(string inStr)
		{
			char[] array = inStr.ToCharArray();
			string[] array2 = new string[20];
			string text = "";
			bool flag = false;
			int num = 0;
			char[] array3 = array;
			foreach (char c in array3)
			{
				if (c == '"')
				{
					flag = !flag;
				}
				else if (flag)
				{
					text += c;
				}
				else if (c == ';')
				{
					array2[num] = text;
					text = "";
					num++;
				}
				else
				{
					text += c;
				}
			}
			array2[num] = text;
			string[] array4 = new string[num + 1];
			Array.Copy(array2, array4, num + 1);
			return array4;
		}

		public static string StripVersion(string versionStr)
		{
			string result = "";
			char[] separator = new char[1] { '.' };
			string[] array = versionStr.Split(separator);
			if (array.Length == 4)
			{
				result = short.Parse(array[0]) + "." + short.Parse(array[1]) + "." + short.Parse(array[2]) + "." + short.Parse(array[3]);
			}
			return result;
		}

		public static int ED_D_FileName(string ed_v_file_name, string prjVersion, string prjName, string language, string ed_d_path_options, out string ed_d_fileName, out string requestedEEDFileName, out string availableLanguage)
		{
			int result = 0;
			string text = "";
			availableLanguage = "";
			string text2 = prjVersion.Replace(".", "_");
			string directoryName = Path.GetDirectoryName(ed_v_file_name);
			string fileName = Path.GetFileName(ed_v_file_name);
			string text3 = prjName + "________";
			text3 = text3.Remove(8);
			fileName = "ED_D_" + text3;
			string searchPattern = fileName + "_" + text2 + "_??.oti";
			fileName = fileName + "_" + text2 + "_" + language + ".oti";
			string text4 = directoryName.Remove(directoryName.Length - 4) + "ED_D\\" + fileName;
			string text5 = directoryName + "\\" + fileName;
			string text6 = ed_d_path_options + "\\" + fileName;
			if (File.Exists(text4))
			{
				text = text4;
			}
			else if (File.Exists(text5))
			{
				text = text5;
			}
			else if (File.Exists(text6))
			{
				text = text6;
			}
			else
			{
				string text7 = directoryName.Remove(directoryName.Length - 4) + "ED_D";
				string text8 = directoryName;
				if (Directory.Exists(text7))
				{
					string[] files = Directory.GetFiles(text7, searchPattern, SearchOption.AllDirectories);
					string[] array = files;
					foreach (string text9 in array)
					{
						availableLanguage = availableLanguage + text9 + ",";
					}
				}
				if (Directory.Exists(text8))
				{
					string[] files2 = Directory.GetFiles(text8, searchPattern, SearchOption.AllDirectories);
					string[] array2 = files2;
					foreach (string text10 in array2)
					{
						availableLanguage = availableLanguage + text10 + ",";
					}
				}
				if (Directory.Exists(ed_d_path_options) && !ed_d_path_options.Equals(text7) && !ed_d_path_options.Equals(text8))
				{
					string[] files3 = Directory.GetFiles(ed_d_path_options, searchPattern, SearchOption.AllDirectories);
					string[] array3 = files3;
					foreach (string text11 in array3)
					{
						availableLanguage = availableLanguage + text11 + ",";
					}
				}
				result = 3;
			}
			ed_d_fileName = text;
			requestedEEDFileName = fileName;
			if (!availableLanguage.Equals(""))
			{
				availableLanguage = availableLanguage.TrimEnd(',');
			}
			return result;
		}

		public static string Get_ED_T_FileName(string ed_d_file_name)
		{
			string text = "";
			string fileName = Path.GetFileName(ed_d_file_name);
			string directoryName = Path.GetDirectoryName(ed_d_file_name);
			fileName = fileName.Replace("ED_D", "ED_T");
			return directoryName + "\\" + fileName;
		}

		public static void ShowInstallationText()
		{
			string text = "Congratulation to choose TDSView.\n";
			string text2 = "Because you didn't pay any fee, don't expect a perfect programm.\n";
			string text3 = "But change requests are welcome every time, except during\n";
			string text4 = "- lunch,\n- dinner,\n- my vacation and\n- the daily PPC/TT-break at 10h00\n";
			string text5 = "\nM. Bossard, PPC/TTAB";
			MessageBox.Show(text + text2 + text3 + text4 + text5, "Welcome to TDSView", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		public static void OfnModify()
		{
			IntPtr intPtr = FindWindowWait(ofdTitle);
			if (!(intPtr == IntPtr.Zero))
			{
				IntPtr hWnd = FindWindowEx(intPtr, IntPtr.Zero, "SHELLDLL_DefView", null);
				SendMessage(hWnd, 273, SHELL_VIEW.REPORT, IntPtr.Zero);
			}
		}

		public static IntPtr FindWindowWait(string windowName)
		{
			Application.DoEvents();
			IntPtr intPtr = FindWindow(null, windowName);
			while (intPtr == IntPtr.Zero)
			{
				Application.DoEvents();
				Thread.Sleep(500);
				intPtr = FindWindow(null, windowName);
			}
			return intPtr;
		}

		[DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern int SendMessage(IntPtr hWnd, int wMsg, SHELL_VIEW wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
	}
}
