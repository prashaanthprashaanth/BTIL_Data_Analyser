using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using TDSView.Properties;

namespace TDSView
{
	public class UserSettings
	{
		public int SaveSettings(string fileName)
		{
			Stream stream = File.Create(fileName);
			if (stream != null)
			{
				StreamWriter streamWriter = new StreamWriter(stream, Encoding.ASCII);
				streamWriter.WriteLine("<?xml version='1.0' encoding='UTF-8'?>");
				streamWriter.WriteLine("<!-- Settings Description for TDSView -->");
				streamWriter.WriteLine("<TDSViewSettings> ");
				WriteXMLLine(streamWriter, "alternateRowColor", Settings.Default.alternateRowColor);
				WriteXMLLine(streamWriter, "ColLatitudeHi", Settings.Default.ColLatitudeHi);
				WriteXMLLine(streamWriter, "ColLatitudeLo", Settings.Default.ColLatitudeLo);
				WriteXMLLine(streamWriter, "ColLongitudeHi", Settings.Default.ColLongitudeHi);
				WriteXMLLine(streamWriter, "ColLongitudeLo", Settings.Default.ColLongitudeLo);
				WriteXMLLine(streamWriter, "ToolLanguage", Settings.Default.ToolLanguage);
				WriteXMLLine(streamWriter, "colVisibleColumns", Settings.Default.colVisibleColumns);
				WriteXMLLine(streamWriter, "ed_d_path", Settings.Default.ed_d_path);
				WriteXMLLine(streamWriter, "prePostEnvVarVisible", Settings.Default.prePostEnvVarVisible);
				WriteXMLLine(streamWriter, "language", Settings.Default.language);
				WriteXMLLine(streamWriter, "errorcode_as_hex", Settings.Default.errorcode_as_hex);
				WriteXMLLine(streamWriter, "stsnt_save_path", Settings.Default.stsnt_save_path);
				FontConverter fontConverter = new FontConverter();
				string value = fontConverter.ConvertToString(Settings.Default.fontGrid);
				WriteXMLLine(streamWriter, "fontGrid", value);
				WriteXMLLine(streamWriter, "showNegativeTimeWarning", Settings.Default.showNegativeTimeWarning);
				WriteXMLLine(streamWriter, "showMissingEDTWarning", Settings.Default.showMissingEDTWarning);
				WriteXMLLine(streamWriter, "DataGridViewFormColumns", Settings.Default.DataGridViewFormColumns);
				WriteXMLLine(streamWriter, "ColUser1", Settings.Default.ColUser1);
				WriteXMLLine(streamWriter, "ColUser2", Settings.Default.ColUser2);
				WriteXMLLine(streamWriter, "ColUser3", Settings.Default.ColUser3);
				WriteXMLLine(streamWriter, "ColUser4", Settings.Default.ColUser4);
				WriteXMLLine(streamWriter, "ColUser5", Settings.Default.ColUser5);
				WriteXMLLine(streamWriter, "takePositionFromEnv", Settings.Default.takePositionFromEnv);
				WriteXMLLine(streamWriter, "CSVFilterSignalName", Settings.Default.CSVFilterSignalName);
				WriteXMLLine(streamWriter, "SortColumn", Settings.Default.SortColumn);
				WriteXMLLine(streamWriter, "SortAscending", Settings.Default.SortAscending);
				WriteXMLLine(streamWriter, "StartStopTimeMilliSec", Settings.Default.StartStopTimeMilliSec);
				WriteXMLLine(streamWriter, "UseLocalPcTime", Settings.Default.UseLocalPcTime);
				WriteXMLLine(streamWriter, "pathToSTSNT", Settings.Default.pathToSTSNT);
				WriteXMLLine(streamWriter, "TimeZoneSel", Settings.Default.TimeZoneSel);
				WriteXMLLine(streamWriter, "TimeZoneShift", Settings.Default.TimeZoneShift);
				WriteXMLLine(streamWriter, "keepGapBetweenEvents", Settings.Default.keepGapBetweenEvents);
				WriteXMLLine(streamWriter, "suppressVersionConflictWarning", Settings.Default.suppressVersionConflictWarning);
				WriteXMLLine(streamWriter, "edvOverviewRoot", Settings.Default.edvOverviewRoot);
				WriteXMLLine(streamWriter, "edvOverviewTreeView", Settings.Default.edvOverviewTreeView);
				WriteXMLLine(streamWriter, "EnvGridHideDescription", Settings.Default.EnvGridHideDescription);
				streamWriter.WriteLine("</TDSViewSettings>");
				streamWriter.Close();
				stream.Close();
			}
			return 0;
		}

		private void WriteXMLLine(StreamWriter mw, string tag, bool value)
		{
			mw.WriteLine(" <" + tag + " value=\"" + value + "\"/>");
		}

		private void WriteXMLLine(StreamWriter mw, string tag, string value)
		{
			mw.WriteLine(" <" + tag + " value=\"" + value + "\"/>");
		}

		private void WriteXMLLine(StreamWriter mw, string tag, double value)
		{
			mw.WriteLine(" <" + tag + " value=\"" + value + "\"/>");
		}

		private void WriteXMLLine(StreamWriter mw, string tag, int value)
		{
			mw.WriteLine(" <" + tag + " value=\"" + value + "\"/>");
		}

		private void WriteXMLLine(StreamWriter mw, string tag, ulong value)
		{
			mw.WriteLine(" <" + tag + " value=\"" + value + "\"/>");
		}

		private void WriteXMLLine(StreamWriter mw, string tag, StringCollection value)
		{
			StringEnumerator enumerator = value.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					mw.WriteLine(" <" + tag + " value=\"" + current + "\"/>");
				}
			}
			finally
			{
				if (enumerator is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}
		}

		public int GetSettings(string fileName)
		{
			int result = 0;
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.Load(fileName);
				XmlElement documentElement = xmlDocument.DocumentElement;
				if (documentElement.Name.Equals("TDSViewSettings"))
				{
					foreach (SettingsProperty property in Settings.Default.Properties)
					{
						_ = property.Name;
					}
					Settings.Default.DataGridViewFormColumns.Clear();
					StringCollection stringCollection = new StringCollection();
					foreach (XmlNode childNode in documentElement.ChildNodes)
					{
						XmlElement xmlElement = (XmlElement)childNode;
						if (xmlElement.Name.Equals("alternateRowColor"))
						{
							Settings.Default.alternateRowColor = xmlElement.GetAttribute("value").ToString().Equals("True");
						}
						else if (xmlElement.Name.Equals("ColLatitudeHi"))
						{
							Settings.Default.ColLatitudeHi = xmlElement.GetAttribute("value");
						}
						if (xmlElement.Name.Equals("ColLatitudeLo"))
						{
							Settings.Default.ColLatitudeLo = xmlElement.GetAttribute("value");
						}
						if (xmlElement.Name.Equals("ColLongitudeHi"))
						{
							Settings.Default.ColLongitudeHi = xmlElement.GetAttribute("value");
						}
						if (xmlElement.Name.Equals("ColLongitudeLo"))
						{
							Settings.Default.ColLongitudeLo = xmlElement.GetAttribute("value");
						}
						if (xmlElement.Name.Equals("ToolLanguage"))
						{
							Settings.Default.ToolLanguage = xmlElement.GetAttribute("value");
						}
						if (xmlElement.Name.Equals("colVisibleColumns"))
						{
							Settings.Default.colVisibleColumns = ulong.Parse(xmlElement.GetAttribute("value"));
						}
						if (xmlElement.Name.Equals("ed_d_path"))
						{
							Settings.Default.ed_d_path = xmlElement.GetAttribute("value");
						}
						if (xmlElement.Name.Equals("prePostEnvVarVisible"))
						{
							Settings.Default.prePostEnvVarVisible = xmlElement.GetAttribute("value").ToString().Equals("True");
						}
						if (xmlElement.Name.Equals("language"))
						{
							Settings.Default.language = xmlElement.GetAttribute("value");
						}
						if (xmlElement.Name.Equals("errorcode_as_hex"))
						{
							Settings.Default.errorcode_as_hex = xmlElement.GetAttribute("value").ToString().Equals("True");
						}
						if (xmlElement.Name.Equals("stsnt_save_path"))
						{
							Settings.Default.stsnt_save_path = xmlElement.GetAttribute("value");
						}
						if (xmlElement.Name.Equals("fontGrid"))
						{
							FontConverter fontConverter = new FontConverter();
							Settings.Default.fontGrid = fontConverter.ConvertFromString(xmlElement.GetAttribute("value")) as Font;
						}
						if (xmlElement.Name.Equals("showNegativeTimeWarning"))
						{
							Settings.Default.showNegativeTimeWarning = xmlElement.GetAttribute("value").ToString().Equals("True");
						}
						if (xmlElement.Name.Equals("showMissingEDTWarning"))
						{
							Settings.Default.showMissingEDTWarning = xmlElement.GetAttribute("value").ToString().Equals("True");
						}
						if (xmlElement.Name.Equals("DataGridViewFormColumns"))
						{
							stringCollection.Add(xmlElement.GetAttribute("value"));
						}
						if (xmlElement.Name.Equals("ColUser1"))
						{
							Settings.Default.ColUser1 = xmlElement.GetAttribute("value");
						}
						if (xmlElement.Name.Equals("ColUser2"))
						{
							Settings.Default.ColUser2 = xmlElement.GetAttribute("value");
						}
						if (xmlElement.Name.Equals("ColUser3"))
						{
							Settings.Default.ColUser3 = xmlElement.GetAttribute("value");
						}
						if (xmlElement.Name.Equals("ColUser4"))
						{
							Settings.Default.ColUser4 = xmlElement.GetAttribute("value");
						}
						if (xmlElement.Name.Equals("ColUser5"))
						{
							Settings.Default.ColUser5 = xmlElement.GetAttribute("value");
						}
						if (xmlElement.Name.Equals("takePositionFromEnv"))
						{
							Settings.Default.takePositionFromEnv = xmlElement.GetAttribute("value").ToString().Equals("True");
						}
						if (xmlElement.Name.Equals("CSVFilterSignalName"))
						{
							Settings.Default.CSVFilterSignalName = xmlElement.GetAttribute("value");
						}
						if (xmlElement.Name.Equals("SortColumn"))
						{
							Settings.Default.SortColumn = xmlElement.GetAttribute("value");
						}
						if (xmlElement.Name.Equals("SortAscending"))
						{
							Settings.Default.SortAscending = xmlElement.GetAttribute("value").ToString().Equals("True");
						}
						if (xmlElement.Name.Equals("StartStopTimeMilliSec"))
						{
							Settings.Default.StartStopTimeMilliSec = xmlElement.GetAttribute("value").ToString().Equals("True");
						}
						if (xmlElement.Name.Equals("UseLocalPcTime"))
						{
							Settings.Default.UseLocalPcTime = xmlElement.GetAttribute("value").ToString().Equals("True");
						}
						if (xmlElement.Name.Equals("pathToSTSNT"))
						{
							Settings.Default.pathToSTSNT = xmlElement.GetAttribute("value");
						}
						if (xmlElement.Name.Equals("TimeZoneSel"))
						{
							Settings.Default.TimeZoneSel = int.Parse(xmlElement.GetAttribute("value"));
						}
						if (xmlElement.Name.Equals("TimeZoneShift"))
						{
							Settings.Default.TimeZoneShift = double.Parse(xmlElement.GetAttribute("value"));
						}
						if (xmlElement.Name.Equals("keepGapBetweenEvents"))
						{
							Settings.Default.keepGapBetweenEvents = xmlElement.GetAttribute("value").ToString().Equals("True");
						}
						if (xmlElement.Name.Equals("suppressVersionConflictWarning"))
						{
							Settings.Default.suppressVersionConflictWarning = xmlElement.GetAttribute("value").ToString().Equals("True");
						}
						if (xmlElement.Name.Equals("edvOverviewRoot"))
						{
							Settings.Default.edvOverviewRoot = xmlElement.GetAttribute("value");
						}
						if (xmlElement.Name.Equals("edvOverviewTreeView"))
						{
							Settings.Default.edvOverviewTreeView = xmlElement.GetAttribute("value").ToString().Equals("True");
						}
						if (xmlElement.Name.Equals("EnvGridHideDescription"))
						{
							Settings.Default.EnvGridHideDescription = xmlElement.GetAttribute("value").ToString().Equals("True");
						}
					}
					Settings.Default.DataGridViewFormColumns = stringCollection;
				}
				else
				{
					MessageBox.Show(TVStr.ThisIsNoConfig, TVStr.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + TVStr.SettingsBackToDefault, TVStr.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				result = 1;
			}
			return result;
		}
	}
}
