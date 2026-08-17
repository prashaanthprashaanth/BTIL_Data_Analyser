using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace TDSView.Properties
{
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
	[CompilerGenerated]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

		public static Settings Default => defaultInstance;

		[DebuggerNonUserCode]
		[DefaultSettingValue("True")]
		[UserScopedSetting]
		public bool alternateRowColor
		{
			get
			{
				return (bool)this["alternateRowColor"];
			}
			set
			{
				this["alternateRowColor"] = value;
			}
		}

		[DefaultSettingValue("511")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public ulong colVisibleColumns
		{
			get
			{
				return (ulong)this["colVisibleColumns"];
			}
			set
			{
				this["colVisibleColumns"] = value;
			}
		}

		[DefaultSettingValue("")]
		[DebuggerNonUserCode]
		[UserScopedSetting]
		public string ed_d_path
		{
			get
			{
				return (string)this["ed_d_path"];
			}
			set
			{
				this["ed_d_path"] = value;
			}
		}

		[DefaultSettingValue("True")]
		[DebuggerNonUserCode]
		[UserScopedSetting]
		public bool prePostEnvVarVisible
		{
			get
			{
				return (bool)this["prePostEnvVarVisible"];
			}
			set
			{
				this["prePostEnvVarVisible"] = value;
			}
		}

		[DebuggerNonUserCode]
		[UserScopedSetting]
		[DefaultSettingValue("False")]
		public bool settingsSaved
		{
			get
			{
				return (bool)this["settingsSaved"];
			}
			set
			{
				this["settingsSaved"] = value;
			}
		}

		[DefaultSettingValue("EN")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public string language
		{
			get
			{
				return (string)this["language"];
			}
			set
			{
				this["language"] = value;
			}
		}

		[DefaultSettingValue("True")]
		[DebuggerNonUserCode]
		[UserScopedSetting]
		public bool errorcode_as_hex
		{
			get
			{
				return (bool)this["errorcode_as_hex"];
			}
			set
			{
				this["errorcode_as_hex"] = value;
			}
		}

		[UserScopedSetting]
		[DefaultSettingValue("")]
		[DebuggerNonUserCode]
		public string stsnt_save_path
		{
			get
			{
				return (string)this["stsnt_save_path"];
			}
			set
			{
				this["stsnt_save_path"] = value;
			}
		}

		[DefaultSettingValue("Microsoft Sans Serif, 8.25pt")]
		[DebuggerNonUserCode]
		[UserScopedSetting]
		public Font fontGrid
		{
			get
			{
				return (Font)this["fontGrid"];
			}
			set
			{
				this["fontGrid"] = value;
			}
		}

		[DebuggerNonUserCode]
		[DefaultSettingValue("True")]
		[UserScopedSetting]
		public bool showNegativeTimeWarning
		{
			get
			{
				return (bool)this["showNegativeTimeWarning"];
			}
			set
			{
				this["showNegativeTimeWarning"] = value;
			}
		}

		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("True")]
		public bool showMissingEDTWarning
		{
			get
			{
				return (bool)this["showMissingEDTWarning"];
			}
			set
			{
				this["showMissingEDTWarning"] = value;
			}
		}

		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		[UserScopedSetting]
		public string ApplicationVersion
		{
			get
			{
				return (string)this["ApplicationVersion"];
			}
			set
			{
				this["ApplicationVersion"] = value;
			}
		}

		[DebuggerNonUserCode]
		[UserScopedSetting]
		public StringCollection DataGridViewFormColumns
		{
			get
			{
				return (StringCollection)this["DataGridViewFormColumns"];
			}
			set
			{
				this["DataGridViewFormColumns"] = value;
			}
		}

		[UserScopedSetting]
		[DefaultSettingValue("User1")]
		[DebuggerNonUserCode]
		public string ColUser1
		{
			get
			{
				return (string)this["ColUser1"];
			}
			set
			{
				this["ColUser1"] = value;
			}
		}

		[DefaultSettingValue("User2")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public string ColUser2
		{
			get
			{
				return (string)this["ColUser2"];
			}
			set
			{
				this["ColUser2"] = value;
			}
		}

		[UserScopedSetting]
		[DefaultSettingValue("User3")]
		[DebuggerNonUserCode]
		public string ColUser3
		{
			get
			{
				return (string)this["ColUser3"];
			}
			set
			{
				this["ColUser3"] = value;
			}
		}

		[DefaultSettingValue("User4")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public string ColUser4
		{
			get
			{
				return (string)this["ColUser4"];
			}
			set
			{
				this["ColUser4"] = value;
			}
		}

		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("User5")]
		public string ColUser5
		{
			get
			{
				return (string)this["ColUser5"];
			}
			set
			{
				this["ColUser5"] = value;
			}
		}

		[DefaultSettingValue("")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public string ColLongitudeHi
		{
			get
			{
				return (string)this["ColLongitudeHi"];
			}
			set
			{
				this["ColLongitudeHi"] = value;
			}
		}

		[DefaultSettingValue("")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public string ColLongitudeLo
		{
			get
			{
				return (string)this["ColLongitudeLo"];
			}
			set
			{
				this["ColLongitudeLo"] = value;
			}
		}

		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string ColLatitudeHi
		{
			get
			{
				return (string)this["ColLatitudeHi"];
			}
			set
			{
				this["ColLatitudeHi"] = value;
			}
		}

		[DefaultSettingValue("")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public string ColLatitudeLo
		{
			get
			{
				return (string)this["ColLatitudeLo"];
			}
			set
			{
				this["ColLatitudeLo"] = value;
			}
		}

		[DefaultSettingValue("False")]
		[DebuggerNonUserCode]
		[UserScopedSetting]
		public bool takePositionFromEnv
		{
			get
			{
				return (bool)this["takePositionFromEnv"];
			}
			set
			{
				this["takePositionFromEnv"] = value;
			}
		}

		[DefaultSettingValue("")]
		[DebuggerNonUserCode]
		[UserScopedSetting]
		public string CSVFilterSignalName
		{
			get
			{
				return (string)this["CSVFilterSignalName"];
			}
			set
			{
				this["CSVFilterSignalName"] = value;
			}
		}

		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		[UserScopedSetting]
		public string SortColumn
		{
			get
			{
				return (string)this["SortColumn"];
			}
			set
			{
				this["SortColumn"] = value;
			}
		}

		[DefaultSettingValue("True")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public bool SortAscending
		{
			get
			{
				return (bool)this["SortAscending"];
			}
			set
			{
				this["SortAscending"] = value;
			}
		}

		[DefaultSettingValue("False")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public bool StartStopTimeMilliSec
		{
			get
			{
				return (bool)this["StartStopTimeMilliSec"];
			}
			set
			{
				this["StartStopTimeMilliSec"] = value;
			}
		}

		[DebuggerNonUserCode]
		[UserScopedSetting]
		[DefaultSettingValue("False")]
		public bool UseLocalPcTime
		{
			get
			{
				return (bool)this["UseLocalPcTime"];
			}
			set
			{
				this["UseLocalPcTime"] = value;
			}
		}

		[DebuggerNonUserCode]
		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string pathToSTSNT
		{
			get
			{
				return (string)this["pathToSTSNT"];
			}
			set
			{
				this["pathToSTSNT"] = value;
			}
		}

		[UserScopedSetting]
		[DebuggerNonUserCode]
		public StringCollection MRUList
		{
			get
			{
				return (StringCollection)this["MRUList"];
			}
			set
			{
				this["MRUList"] = value;
			}
		}

		[UserScopedSetting]
		[DefaultSettingValue("en")]
		[DebuggerNonUserCode]
		public string ToolLanguage
		{
			get
			{
				return (string)this["ToolLanguage"];
			}
			set
			{
				this["ToolLanguage"] = value;
			}
		}

		[DebuggerNonUserCode]
		[DefaultSettingValue("0")]
		[UserScopedSetting]
		public int TimeZoneSel
		{
			get
			{
				return (int)this["TimeZoneSel"];
			}
			set
			{
				this["TimeZoneSel"] = value;
			}
		}

		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("0")]
		public double TimeZoneShift
		{
			get
			{
				return (double)this["TimeZoneShift"];
			}
			set
			{
				this["TimeZoneShift"] = value;
			}
		}

		[DefaultSettingValue("False")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public bool keepGapBetweenEvents
		{
			get
			{
				return (bool)this["keepGapBetweenEvents"];
			}
			set
			{
				this["keepGapBetweenEvents"] = value;
			}
		}

		[DebuggerNonUserCode]
		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string edvOverviewRoot
		{
			get
			{
				return (string)this["edvOverviewRoot"];
			}
			set
			{
				this["edvOverviewRoot"] = value;
			}
		}

		[DebuggerNonUserCode]
		[UserScopedSetting]
		[DefaultSettingValue("True")]
		public bool edvOverviewTreeView
		{
			get
			{
				return (bool)this["edvOverviewTreeView"];
			}
			set
			{
				this["edvOverviewTreeView"] = value;
			}
		}

		[DebuggerNonUserCode]
		[DefaultSettingValue("True")]
		[UserScopedSetting]
		public bool suppressVersionConflictWarning
		{
			get
			{
				return (bool)this["suppressVersionConflictWarning"];
			}
			set
			{
				this["suppressVersionConflictWarning"] = value;
			}
		}

		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("965")]
		public int MainFormWidth
		{
			get
			{
				return (int)this["MainFormWidth"];
			}
			set
			{
				this["MainFormWidth"] = value;
			}
		}

		[DebuggerNonUserCode]
		[UserScopedSetting]
		[DefaultSettingValue("450")]
		public int MainFormHeight
		{
			get
			{
				return (int)this["MainFormHeight"];
			}
			set
			{
				this["MainFormHeight"] = value;
			}
		}

		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("597")]
		public int MainFormSplitterDistance
		{
			get
			{
				return (int)this["MainFormSplitterDistance"];
			}
			set
			{
				this["MainFormSplitterDistance"] = value;
			}
		}

		[UserScopedSetting]
		[DefaultSettingValue("False")]
		[DebuggerNonUserCode]
		public bool EnvGridHideDescription
		{
			get
			{
				return (bool)this["EnvGridHideDescription"];
			}
			set
			{
				this["EnvGridHideDescription"] = value;
			}
		}

		private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
		{
		}

		private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
		{
		}
	}
}
