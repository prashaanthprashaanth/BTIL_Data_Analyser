using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TDSView
{
	public class Options : Form
	{
		private IContainer components;

		private CheckedListBox clb_VisibleColumns;

		private Button btn_Exit;

		private Button btn_SelectAll;

		private Button btn_DeselectAll;

		private Button btn_Cancel;

		private TabControl tabControl_Options;

		private TabPage tabPage1;

		private TabPage tabPage2;

		private FolderBrowserDialog options_folderBrowserDialog;

		private ListBox lb_Language;

		private GroupBox groupBox2;

		private CheckBox cb_ErrorCodeAsHex;

		private TabPage tabPage3;

		private GroupBox groupBox3;

		private TextBox tb_OutputPath;

		private Button btn_OutputSavePath;

		private TextBox textBox4;

		private GroupBox groupBox1;

		private TextBox tb_ED_D;

		private Button btn_PathToEDD;

		private TextBox textBox2;

		private TextBox ed_d_path_textBox;

		private Button btn_browseDirectory;

		private TextBox tb_EDDSearchPath;

		private FontDialog fontDialog1;

		private Button btn_Font;

		private Label labelFont;

		private GroupBox groupBox4;

		private CheckBox cb_NegTimeWarn;

		private CheckBox cb_ShowMissingEDT;

		private TabPage tabPage4;

		private Label lbl_LongHi;

		private ComboBox cmb_LongitudeHigh;

		private Label lbl_LatLo;

		private Label lbl_LatHi;

		private Label lbl_LongLo;

		private ComboBox cmb_LatitudeLo;

		private ComboBox cmb_LatitudeHi;

		private ComboBox cmb_LongitudeLow;

		private CheckBox cb_AlternateRowColor;

		private GroupBox groupBox5;

		private TabPage tabPage5;

		private Label label5;

		private TextBox tb_SignalNameFilter;

		private GroupBox groupBox6;

		private RadioButton rb_ColSortAscending;

		private RadioButton rb_ColSortDescending;

		private GroupBox groupBox7;

		private ComboBox cmb_BoxSorting;

		private CheckBox cb_TimeWithMilliSec;

		private CheckBox cb_UseLocalTime;

		private GroupBox groupBox8;

		private Button btn_PathToSTSNT;

		private TextBox tb_STSNT;

		private OpenFileDialog openFileDialogSTSNT;

		private TabPage tabPage6;

		private CheckedListBox clb_ToolLang;

		private CheckBox cb_PredefinedLanguage;

		private TextBox tb_projectLanguage;

		private ComboBox cmb_TimeZone;

		private Label lbl_TimeZone;

		private CheckBox cb_NoEnvSigGap;

		private Label lbl_allSignals;

		private CheckBox cb_SuppressVersionConflictWarning;

		private RadioButton rb_CoordRDS;

		private RadioButton rb_CoordMRVC;

		private GroupBox gp_SrcOfPositionData;

		private RadioButton rb_TakeFromEventAttr;

		public bool refNr;

		public bool[] elements;

		public bool[] visibleElements;

		public bool cancel;

		public string ed_d_path;

		public string output_file_path;

		public string language;

		public bool errorCodeAsHex;

		public bool showNegativeTimeWarning;

		public bool showMissingEDTWarning;

		public bool alternateRowColor;

		public Font font;

		public bool takePositionFromEnv;

		public string longitudeHi;

		public string longitudeLo;

		public string latitudeHi;

		public string latitudeLo;

		public string csvFilterSignalName;

		public string sortColumn;

		public bool sortAscending;

		public bool showTimesWithMilliSec;

		public bool useLocalPcTime;

		public ED ed;

		public string pathToSTSNT;

		public string toolLanguage;

		public int selectedTimeZone;

		public double timeZoneShift;

		public bool keepGapBetweenEvents;

		public bool suppressVersionConflictWarning;

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TDSView.Options));
			this.clb_VisibleColumns = new System.Windows.Forms.CheckedListBox();
			this.btn_Exit = new System.Windows.Forms.Button();
			this.btn_SelectAll = new System.Windows.Forms.Button();
			this.btn_DeselectAll = new System.Windows.Forms.Button();
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.tabControl_Options = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.rb_ColSortDescending = new System.Windows.Forms.RadioButton();
			this.rb_ColSortAscending = new System.Windows.Forms.RadioButton();
			this.cmb_BoxSorting = new System.Windows.Forms.ComboBox();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.cb_SuppressVersionConflictWarning = new System.Windows.Forms.CheckBox();
			this.cb_NoEnvSigGap = new System.Windows.Forms.CheckBox();
			this.lbl_TimeZone = new System.Windows.Forms.Label();
			this.cmb_TimeZone = new System.Windows.Forms.ComboBox();
			this.cb_UseLocalTime = new System.Windows.Forms.CheckBox();
			this.cb_TimeWithMilliSec = new System.Windows.Forms.CheckBox();
			this.cb_AlternateRowColor = new System.Windows.Forms.CheckBox();
			this.cb_ShowMissingEDT = new System.Windows.Forms.CheckBox();
			this.cb_NegTimeWarn = new System.Windows.Forms.CheckBox();
			this.labelFont = new System.Windows.Forms.Label();
			this.cb_ErrorCodeAsHex = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cb_PredefinedLanguage = new System.Windows.Forms.CheckBox();
			this.tb_projectLanguage = new System.Windows.Forms.TextBox();
			this.lb_Language = new System.Windows.Forms.ListBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.btn_Font = new System.Windows.Forms.Button();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.btn_PathToSTSNT = new System.Windows.Forms.Button();
			this.tb_STSNT = new System.Windows.Forms.TextBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.tb_OutputPath = new System.Windows.Forms.TextBox();
			this.btn_OutputSavePath = new System.Windows.Forms.Button();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tb_ED_D = new System.Windows.Forms.TextBox();
			this.btn_PathToEDD = new System.Windows.Forms.Button();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.gp_SrcOfPositionData = new System.Windows.Forms.GroupBox();
			this.rb_TakeFromEventAttr = new System.Windows.Forms.RadioButton();
			this.rb_CoordRDS = new System.Windows.Forms.RadioButton();
			this.rb_CoordMRVC = new System.Windows.Forms.RadioButton();
			this.lbl_LatLo = new System.Windows.Forms.Label();
			this.lbl_LatHi = new System.Windows.Forms.Label();
			this.lbl_LongLo = new System.Windows.Forms.Label();
			this.cmb_LatitudeLo = new System.Windows.Forms.ComboBox();
			this.cmb_LatitudeHi = new System.Windows.Forms.ComboBox();
			this.cmb_LongitudeLow = new System.Windows.Forms.ComboBox();
			this.lbl_LongHi = new System.Windows.Forms.Label();
			this.cmb_LongitudeHigh = new System.Windows.Forms.ComboBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.label5 = new System.Windows.Forms.Label();
			this.tb_SignalNameFilter = new System.Windows.Forms.TextBox();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.lbl_allSignals = new System.Windows.Forms.Label();
			this.tabPage6 = new System.Windows.Forms.TabPage();
			this.clb_ToolLang = new System.Windows.Forms.CheckedListBox();
			this.options_folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.ed_d_path_textBox = new System.Windows.Forms.TextBox();
			this.btn_browseDirectory = new System.Windows.Forms.Button();
			this.tb_EDDSearchPath = new System.Windows.Forms.TextBox();
			this.fontDialog1 = new System.Windows.Forms.FontDialog();
			this.openFileDialogSTSNT = new System.Windows.Forms.OpenFileDialog();
			this.tabControl_Options.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.gp_SrcOfPositionData.SuspendLayout();
			this.tabPage5.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.tabPage6.SuspendLayout();
			base.SuspendLayout();
			this.clb_VisibleColumns.CheckOnClick = true;
			this.clb_VisibleColumns.FormattingEnabled = true;
			resources.ApplyResources(this.clb_VisibleColumns, "clb_VisibleColumns");
			this.clb_VisibleColumns.Name = "clb_VisibleColumns";
			resources.ApplyResources(this.btn_Exit, "btn_Exit");
			this.btn_Exit.Name = "btn_Exit";
			this.btn_Exit.UseVisualStyleBackColor = true;
			this.btn_Exit.Click += new System.EventHandler(Exit_Click);
			resources.ApplyResources(this.btn_SelectAll, "btn_SelectAll");
			this.btn_SelectAll.Name = "btn_SelectAll";
			this.btn_SelectAll.UseVisualStyleBackColor = true;
			this.btn_SelectAll.Click += new System.EventHandler(buttonSelectAll_Click);
			resources.ApplyResources(this.btn_DeselectAll, "btn_DeselectAll");
			this.btn_DeselectAll.Name = "btn_DeselectAll";
			this.btn_DeselectAll.UseVisualStyleBackColor = true;
			this.btn_DeselectAll.Click += new System.EventHandler(btn_DeselectAll_Click);
			resources.ApplyResources(this.btn_Cancel, "btn_Cancel");
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new System.EventHandler(btn_Cancel_Click);
			this.tabControl_Options.Controls.Add(this.tabPage1);
			this.tabControl_Options.Controls.Add(this.tabPage2);
			this.tabControl_Options.Controls.Add(this.tabPage3);
			this.tabControl_Options.Controls.Add(this.tabPage4);
			this.tabControl_Options.Controls.Add(this.tabPage5);
			this.tabControl_Options.Controls.Add(this.tabPage6);
			resources.ApplyResources(this.tabControl_Options, "tabControl_Options");
			this.tabControl_Options.Name = "tabControl_Options";
			this.tabControl_Options.SelectedIndex = 0;
			this.tabPage1.Controls.Add(this.rb_ColSortDescending);
			this.tabPage1.Controls.Add(this.rb_ColSortAscending);
			this.tabPage1.Controls.Add(this.cmb_BoxSorting);
			this.tabPage1.Controls.Add(this.clb_VisibleColumns);
			this.tabPage1.Controls.Add(this.btn_SelectAll);
			this.tabPage1.Controls.Add(this.btn_DeselectAll);
			this.tabPage1.Controls.Add(this.groupBox7);
			resources.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.rb_ColSortDescending, "rb_ColSortDescending");
			this.rb_ColSortDescending.Name = "rb_ColSortDescending";
			this.rb_ColSortDescending.TabStop = true;
			this.rb_ColSortDescending.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.rb_ColSortAscending, "rb_ColSortAscending");
			this.rb_ColSortAscending.Name = "rb_ColSortAscending";
			this.rb_ColSortAscending.TabStop = true;
			this.rb_ColSortAscending.UseVisualStyleBackColor = true;
			this.cmb_BoxSorting.FormattingEnabled = true;
			resources.ApplyResources(this.cmb_BoxSorting, "cmb_BoxSorting");
			this.cmb_BoxSorting.Name = "cmb_BoxSorting";
			this.cmb_BoxSorting.SelectedIndexChanged += new System.EventHandler(cb_Sorting_SelectedIndexChanged);
			resources.ApplyResources(this.groupBox7, "groupBox7");
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.TabStop = false;
			this.tabPage2.Controls.Add(this.cb_SuppressVersionConflictWarning);
			this.tabPage2.Controls.Add(this.cb_NoEnvSigGap);
			this.tabPage2.Controls.Add(this.lbl_TimeZone);
			this.tabPage2.Controls.Add(this.cmb_TimeZone);
			this.tabPage2.Controls.Add(this.cb_UseLocalTime);
			this.tabPage2.Controls.Add(this.cb_TimeWithMilliSec);
			this.tabPage2.Controls.Add(this.cb_AlternateRowColor);
			this.tabPage2.Controls.Add(this.cb_ShowMissingEDT);
			this.tabPage2.Controls.Add(this.cb_NegTimeWarn);
			this.tabPage2.Controls.Add(this.labelFont);
			this.tabPage2.Controls.Add(this.cb_ErrorCodeAsHex);
			this.tabPage2.Controls.Add(this.groupBox2);
			this.tabPage2.Controls.Add(this.groupBox4);
			resources.ApplyResources(this.tabPage2, "tabPage2");
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.cb_SuppressVersionConflictWarning, "cb_SuppressVersionConflictWarning");
			this.cb_SuppressVersionConflictWarning.Name = "cb_SuppressVersionConflictWarning";
			this.cb_SuppressVersionConflictWarning.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.cb_NoEnvSigGap, "cb_NoEnvSigGap");
			this.cb_NoEnvSigGap.Name = "cb_NoEnvSigGap";
			this.cb_NoEnvSigGap.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.lbl_TimeZone, "lbl_TimeZone");
			this.lbl_TimeZone.Name = "lbl_TimeZone";
			this.cmb_TimeZone.FormattingEnabled = true;
			resources.ApplyResources(this.cmb_TimeZone, "cmb_TimeZone");
			this.cmb_TimeZone.Name = "cmb_TimeZone";
			this.cmb_TimeZone.SelectedIndexChanged += new System.EventHandler(cmb_TimeZone_SelectedIndexChanged);
			resources.ApplyResources(this.cb_UseLocalTime, "cb_UseLocalTime");
			this.cb_UseLocalTime.Name = "cb_UseLocalTime";
			this.cb_UseLocalTime.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.cb_TimeWithMilliSec, "cb_TimeWithMilliSec");
			this.cb_TimeWithMilliSec.Name = "cb_TimeWithMilliSec";
			this.cb_TimeWithMilliSec.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.cb_AlternateRowColor, "cb_AlternateRowColor");
			this.cb_AlternateRowColor.Name = "cb_AlternateRowColor";
			this.cb_AlternateRowColor.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.cb_ShowMissingEDT, "cb_ShowMissingEDT");
			this.cb_ShowMissingEDT.Name = "cb_ShowMissingEDT";
			this.cb_ShowMissingEDT.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.cb_NegTimeWarn, "cb_NegTimeWarn");
			this.cb_NegTimeWarn.Name = "cb_NegTimeWarn";
			this.cb_NegTimeWarn.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.labelFont, "labelFont");
			this.labelFont.Name = "labelFont";
			resources.ApplyResources(this.cb_ErrorCodeAsHex, "cb_ErrorCodeAsHex");
			this.cb_ErrorCodeAsHex.Name = "cb_ErrorCodeAsHex";
			this.cb_ErrorCodeAsHex.UseVisualStyleBackColor = true;
			this.groupBox2.Controls.Add(this.cb_PredefinedLanguage);
			this.groupBox2.Controls.Add(this.tb_projectLanguage);
			this.groupBox2.Controls.Add(this.lb_Language);
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			resources.ApplyResources(this.cb_PredefinedLanguage, "cb_PredefinedLanguage");
			this.cb_PredefinedLanguage.Name = "cb_PredefinedLanguage";
			this.cb_PredefinedLanguage.UseVisualStyleBackColor = true;
			this.cb_PredefinedLanguage.CheckedChanged += new System.EventHandler(cb_PredefinedLanguage_CheckedChanged);
			resources.ApplyResources(this.tb_projectLanguage, "tb_projectLanguage");
			this.tb_projectLanguage.Name = "tb_projectLanguage";
			this.lb_Language.FormattingEnabled = true;
			this.lb_Language.Items.AddRange(new object[10]
			{
				resources.GetString("lb_Language.Items"),
				resources.GetString("lb_Language.Items1"),
				resources.GetString("lb_Language.Items2"),
				resources.GetString("lb_Language.Items3"),
				resources.GetString("lb_Language.Items4"),
				resources.GetString("lb_Language.Items5"),
				resources.GetString("lb_Language.Items6"),
				resources.GetString("lb_Language.Items7"),
				resources.GetString("lb_Language.Items8"),
				resources.GetString("lb_Language.Items9")
			});
			resources.ApplyResources(this.lb_Language, "lb_Language");
			this.lb_Language.Name = "lb_Language";
			this.lb_Language.SelectedIndexChanged += new System.EventHandler(lb_Language_SelectedIndexChanged);
			this.groupBox4.Controls.Add(this.btn_Font);
			resources.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			resources.ApplyResources(this.btn_Font, "btn_Font");
			this.btn_Font.Name = "btn_Font";
			this.btn_Font.UseVisualStyleBackColor = true;
			this.btn_Font.Click += new System.EventHandler(btn_Font_Click);
			this.tabPage3.Controls.Add(this.groupBox8);
			this.tabPage3.Controls.Add(this.groupBox3);
			this.tabPage3.Controls.Add(this.groupBox1);
			resources.ApplyResources(this.tabPage3, "tabPage3");
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.UseVisualStyleBackColor = true;
			this.groupBox8.Controls.Add(this.btn_PathToSTSNT);
			this.groupBox8.Controls.Add(this.tb_STSNT);
			resources.ApplyResources(this.groupBox8, "groupBox8");
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.TabStop = false;
			resources.ApplyResources(this.btn_PathToSTSNT, "btn_PathToSTSNT");
			this.btn_PathToSTSNT.Name = "btn_PathToSTSNT";
			this.btn_PathToSTSNT.UseVisualStyleBackColor = true;
			this.btn_PathToSTSNT.Click += new System.EventHandler(btn_PathToSTSNT_Click);
			resources.ApplyResources(this.tb_STSNT, "tb_STSNT");
			this.tb_STSNT.Name = "tb_STSNT";
			this.groupBox3.Controls.Add(this.tb_OutputPath);
			this.groupBox3.Controls.Add(this.btn_OutputSavePath);
			this.groupBox3.Controls.Add(this.textBox4);
			resources.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			this.tb_OutputPath.AcceptsTab = true;
			resources.ApplyResources(this.tb_OutputPath, "tb_OutputPath");
			this.tb_OutputPath.Name = "tb_OutputPath";
			resources.ApplyResources(this.btn_OutputSavePath, "btn_OutputSavePath");
			this.btn_OutputSavePath.Name = "btn_OutputSavePath";
			this.btn_OutputSavePath.UseVisualStyleBackColor = true;
			this.btn_OutputSavePath.Click += new System.EventHandler(btn_OutputSavePath_Click);
			this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.textBox4, "textBox4");
			this.textBox4.Name = "textBox4";
			this.textBox4.ReadOnly = true;
			this.groupBox1.Controls.Add(this.tb_ED_D);
			this.groupBox1.Controls.Add(this.btn_PathToEDD);
			this.groupBox1.Controls.Add(this.textBox2);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.tb_ED_D.AcceptsTab = true;
			resources.ApplyResources(this.tb_ED_D, "tb_ED_D");
			this.tb_ED_D.Name = "tb_ED_D";
			this.tb_ED_D.TextChanged += new System.EventHandler(tb_EDD_TextChanged);
			resources.ApplyResources(this.btn_PathToEDD, "btn_PathToEDD");
			this.btn_PathToEDD.Name = "btn_PathToEDD";
			this.btn_PathToEDD.UseVisualStyleBackColor = true;
			this.btn_PathToEDD.Click += new System.EventHandler(btn_BrowseToEDDDirectory_Click);
			this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.textBox2, "textBox2");
			this.textBox2.Name = "textBox2";
			this.textBox2.ReadOnly = true;
			this.tabPage4.Controls.Add(this.gp_SrcOfPositionData);
			this.tabPage4.Controls.Add(this.lbl_LatLo);
			this.tabPage4.Controls.Add(this.lbl_LatHi);
			this.tabPage4.Controls.Add(this.lbl_LongLo);
			this.tabPage4.Controls.Add(this.cmb_LatitudeLo);
			this.tabPage4.Controls.Add(this.cmb_LatitudeHi);
			this.tabPage4.Controls.Add(this.cmb_LongitudeLow);
			this.tabPage4.Controls.Add(this.lbl_LongHi);
			this.tabPage4.Controls.Add(this.cmb_LongitudeHigh);
			this.tabPage4.Controls.Add(this.groupBox5);
			resources.ApplyResources(this.tabPage4, "tabPage4");
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.UseVisualStyleBackColor = true;
			this.gp_SrcOfPositionData.Controls.Add(this.rb_TakeFromEventAttr);
			this.gp_SrcOfPositionData.Controls.Add(this.rb_CoordRDS);
			this.gp_SrcOfPositionData.Controls.Add(this.rb_CoordMRVC);
			resources.ApplyResources(this.gp_SrcOfPositionData, "gp_SrcOfPositionData");
			this.gp_SrcOfPositionData.Name = "gp_SrcOfPositionData";
			this.gp_SrcOfPositionData.TabStop = false;
			resources.ApplyResources(this.rb_TakeFromEventAttr, "rb_TakeFromEventAttr");
			this.rb_TakeFromEventAttr.Name = "rb_TakeFromEventAttr";
			this.rb_TakeFromEventAttr.TabStop = true;
			this.rb_TakeFromEventAttr.UseVisualStyleBackColor = true;
			this.rb_TakeFromEventAttr.CheckedChanged += new System.EventHandler(rb_TakeFromEventAttr_CheckedChanged);
			resources.ApplyResources(this.rb_CoordRDS, "rb_CoordRDS");
			this.rb_CoordRDS.Name = "rb_CoordRDS";
			this.rb_CoordRDS.TabStop = true;
			this.rb_CoordRDS.UseVisualStyleBackColor = true;
			this.rb_CoordRDS.CheckedChanged += new System.EventHandler(rb_CoordRDS_CheckedChanged);
			resources.ApplyResources(this.rb_CoordMRVC, "rb_CoordMRVC");
			this.rb_CoordMRVC.Name = "rb_CoordMRVC";
			this.rb_CoordMRVC.TabStop = true;
			this.rb_CoordMRVC.UseVisualStyleBackColor = true;
			this.rb_CoordMRVC.CheckedChanged += new System.EventHandler(rb_CoordMRVC_CheckedChanged);
			resources.ApplyResources(this.lbl_LatLo, "lbl_LatLo");
			this.lbl_LatLo.Name = "lbl_LatLo";
			resources.ApplyResources(this.lbl_LatHi, "lbl_LatHi");
			this.lbl_LatHi.Name = "lbl_LatHi";
			resources.ApplyResources(this.lbl_LongLo, "lbl_LongLo");
			this.lbl_LongLo.Name = "lbl_LongLo";
			this.cmb_LatitudeLo.FormattingEnabled = true;
			resources.ApplyResources(this.cmb_LatitudeLo, "cmb_LatitudeLo");
			this.cmb_LatitudeLo.Name = "cmb_LatitudeLo";
			this.cmb_LatitudeLo.SelectedIndexChanged += new System.EventHandler(comboBox3_SelectedIndexChanged);
			this.cmb_LatitudeHi.FormattingEnabled = true;
			resources.ApplyResources(this.cmb_LatitudeHi, "cmb_LatitudeHi");
			this.cmb_LatitudeHi.Name = "cmb_LatitudeHi";
			this.cmb_LatitudeHi.SelectedIndexChanged += new System.EventHandler(comboBox2_SelectedIndexChanged);
			this.cmb_LongitudeLow.FormattingEnabled = true;
			resources.ApplyResources(this.cmb_LongitudeLow, "cmb_LongitudeLow");
			this.cmb_LongitudeLow.Name = "cmb_LongitudeLow";
			resources.ApplyResources(this.lbl_LongHi, "lbl_LongHi");
			this.lbl_LongHi.Name = "lbl_LongHi";
			this.cmb_LongitudeHigh.FormattingEnabled = true;
			resources.ApplyResources(this.cmb_LongitudeHigh, "cmb_LongitudeHigh");
			this.cmb_LongitudeHigh.Name = "cmb_LongitudeHigh";
			resources.ApplyResources(this.groupBox5, "groupBox5");
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.TabStop = false;
			this.tabPage5.Controls.Add(this.label5);
			this.tabPage5.Controls.Add(this.tb_SignalNameFilter);
			this.tabPage5.Controls.Add(this.groupBox6);
			resources.ApplyResources(this.tabPage5, "tabPage5");
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			resources.ApplyResources(this.tb_SignalNameFilter, "tb_SignalNameFilter");
			this.tb_SignalNameFilter.Name = "tb_SignalNameFilter";
			this.groupBox6.Controls.Add(this.lbl_allSignals);
			resources.ApplyResources(this.groupBox6, "groupBox6");
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.TabStop = false;
			resources.ApplyResources(this.lbl_allSignals, "lbl_allSignals");
			this.lbl_allSignals.Name = "lbl_allSignals";
			this.tabPage6.Controls.Add(this.clb_ToolLang);
			resources.ApplyResources(this.tabPage6, "tabPage6");
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.UseVisualStyleBackColor = true;
			this.clb_ToolLang.CheckOnClick = true;
			this.clb_ToolLang.FormattingEnabled = true;
			this.clb_ToolLang.Items.AddRange(new object[3]
			{
				resources.GetString("clb_ToolLang.Items"),
				resources.GetString("clb_ToolLang.Items1"),
				resources.GetString("clb_ToolLang.Items2")
			});
			resources.ApplyResources(this.clb_ToolLang, "clb_ToolLang");
			this.clb_ToolLang.Name = "clb_ToolLang";
			this.clb_ToolLang.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(checkedListBoxToolLang_ItemCheck);
			this.ed_d_path_textBox.AcceptsTab = true;
			resources.ApplyResources(this.ed_d_path_textBox, "ed_d_path_textBox");
			this.ed_d_path_textBox.Name = "ed_d_path_textBox";
			resources.ApplyResources(this.btn_browseDirectory, "btn_browseDirectory");
			this.btn_browseDirectory.Name = "btn_browseDirectory";
			this.btn_browseDirectory.UseVisualStyleBackColor = true;
			this.tb_EDDSearchPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.tb_EDDSearchPath, "tb_EDDSearchPath");
			this.tb_EDDSearchPath.Name = "tb_EDDSearchPath";
			this.tb_EDDSearchPath.ReadOnly = true;
			this.openFileDialogSTSNT.FileName = "openFileDialogSTSNT";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.tabControl_Options);
			base.Controls.Add(this.btn_Cancel);
			base.Controls.Add(this.btn_Exit);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.Name = "Options";
			base.Load += new System.EventHandler(Options_Load);
			this.tabControl_Options.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.groupBox8.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabPage4.ResumeLayout(false);
			this.tabPage4.PerformLayout();
			this.gp_SrcOfPositionData.ResumeLayout(false);
			this.gp_SrcOfPositionData.PerformLayout();
			this.tabPage5.ResumeLayout(false);
			this.tabPage5.PerformLayout();
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.tabPage6.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		public Options(object[] checkElements, bool[] visElements)
		{
			InitializeComponent();
			clb_VisibleColumns.Items.AddRange(checkElements);
			cmb_BoxSorting.Items.Add("");
			elements = new bool[checkElements.Length];
			for (int i = 0; i < checkElements.Length; i++)
			{
				elements[i] = false;
				clb_VisibleColumns.SetItemChecked(i, visElements[i]);
				cmb_BoxSorting.Items.Add(checkElements[i]);
			}
			cancel = false;
			toolLanguage = "";
			SetElementText();
		}

		private void SetElementText()
		{
			Text = TVStr.Options;
			btn_Cancel.Text = TVStr.Cancel;
			btn_DeselectAll.Text = TVStr.DeselectAll;
			btn_Exit.Text = TVStr.Exit;
			btn_Font.Text = TVStr.SelectFont;
			btn_OutputSavePath.Text = TVStr.BrowseTo;
			btn_PathToEDD.Text = TVStr.BrowseTo;
			btn_PathToSTSNT.Text = TVStr.BrowseTo;
			btn_SelectAll.Text = TVStr.SelectAll;
			cb_AlternateRowColor.Text = TVStr.AlternateRowColor;
			cb_ErrorCodeAsHex.Text = TVStr.ErrorCodeAsHex;
			cb_NegTimeWarn.Text = TVStr.ShowNegativeTimeWarning;
			cb_NoEnvSigGap.Text = TVStr.NoEnvSigGap;
			cb_PredefinedLanguage.Text = TVStr.PredefinedLanguage;
			cb_ShowMissingEDT.Text = TVStr.ShowMissingEDT;
			cb_SuppressVersionConflictWarning.Text = TVStr.SuppressVersionConflict;
			cb_TimeWithMilliSec.Text = TVStr.TimeWithMilliSec;
			cb_UseLocalTime.Text = TVStr.UseLocalTime;
			gp_SrcOfPositionData.Text = TVStr.SrcOfPositionData;
			groupBox1.Text = TVStr.OptGroupBox1;
			groupBox2.Text = TVStr.OptGroupBox2;
			groupBox3.Text = TVStr.OptGroupBox3;
			groupBox4.Text = TVStr.OptGroupBox4;
			groupBox5.Text = TVStr.OptGroupBox5;
			groupBox6.Text = TVStr.OptGroupBox6;
			groupBox7.Text = TVStr.OptGroupBox7;
			groupBox8.Text = TVStr.OptGroupBox8;
			label5.Text = TVStr.OptLabel5Text;
			lbl_allSignals.Text = TVStr.AllSignals;
			lbl_LatHi.Text = TVStr.LatHi;
			lbl_LatLo.Text = TVStr.LatLo;
			lbl_LongHi.Text = TVStr.LongHi;
			lbl_LongLo.Text = TVStr.LongLo;
			lbl_TimeZone.Text = TVStr.TimeZone;
			rb_ColSortAscending.Text = TVStr.ColSortAscending;
			rb_ColSortDescending.Text = TVStr.ColSortDescending;
			rb_CoordMRVC.Text = TVStr.CoordMRVC;
			rb_CoordRDS.Text = TVStr.CoordRDS;
			rb_TakeFromEventAttr.Text = TVStr.TakeFromEventAttr;
			tabPage1.Text = TVStr.OptTabPage1;
			tabPage2.Text = TVStr.OptTabPage2;
			tabPage3.Text = TVStr.OptTabPage3;
			tabPage4.Text = TVStr.OptTabPage4;
			tabPage5.Text = TVStr.OptTabPage5;
			tabPage6.Text = TVStr.OptTabPage6;
			textBox2.Text = TVStr.OptTextBox2;
			textBox4.Text = TVStr.OptTextBox4;
		}

		private void Options_Load(object sender, EventArgs e)
		{
			tb_ED_D.Text = ed_d_path;
			tb_OutputPath.Text = output_file_path;
			int num = lb_Language.FindString(language);
			if (num == -1)
			{
				cb_PredefinedLanguage.Checked = false;
				tb_projectLanguage.Text = language;
			}
			else
			{
				cb_PredefinedLanguage.Checked = true;
				tb_projectLanguage.Text = "";
				lb_Language.SelectedIndex = num;
			}
			SetLanguageField();
			cb_ErrorCodeAsHex.Checked = errorCodeAsHex;
			cb_NegTimeWarn.Checked = showNegativeTimeWarning;
			cb_ShowMissingEDT.Checked = showMissingEDTWarning;
			setFontString();
			bool flag = false;
			bool flag2 = false;
			if (takePositionFromEnv)
			{
				rb_TakeFromEventAttr.Checked = false;
				if (longitudeHi.Equals("COORD_MRVC"))
				{
					rb_CoordRDS.Checked = false;
					rb_CoordMRVC.Checked = true;
					flag2 = true;
					flag = false;
				}
				else
				{
					rb_CoordRDS.Checked = true;
					rb_CoordMRVC.Checked = false;
					flag = (flag2 = true);
				}
			}
			else
			{
				rb_TakeFromEventAttr.Checked = true;
				rb_CoordRDS.Checked = false;
				rb_CoordMRVC.Checked = false;
				flag = (flag2 = false);
			}
			ShowPositionCombo(flag2, flag);
			cb_AlternateRowColor.Checked = alternateRowColor;
			cmb_LongitudeHigh.Text = longitudeHi;
			cmb_LongitudeLow.Text = longitudeLo;
			cmb_LatitudeHi.Text = latitudeHi;
			cmb_LatitudeLo.Text = latitudeLo;
			tb_SignalNameFilter.Text = csvFilterSignalName;
			cmb_BoxSorting.Text = sortColumn;
			rb_ColSortAscending.Checked = sortAscending;
			rb_ColSortDescending.Checked = !sortAscending;
			cb_TimeWithMilliSec.Checked = showTimesWithMilliSec;
			cb_UseLocalTime.Checked = useLocalPcTime;
			tb_STSNT.Text = pathToSTSNT;
			cb_NoEnvSigGap.Checked = keepGapBetweenEvents;
			cb_SuppressVersionConflictWarning.Checked = suppressVersionConflictWarning;
			LoadTimeZone(selectedTimeZone);
			if (sortColumn.Equals(""))
			{
				rb_ColSortAscending.Enabled = false;
				rb_ColSortDescending.Enabled = false;
			}
			foreach (ED_D item in ed.eddl)
			{
				foreach (ED_D_EnvSignal item2 in item.env_signal)
				{
					cmb_LongitudeHigh.Items.Add(item2.env_name);
					cmb_LongitudeLow.Items.Add(item2.env_name);
					cmb_LatitudeHi.Items.Add(item2.env_name);
					cmb_LatitudeLo.Items.Add(item2.env_name);
				}
			}
			SetLanguage(toolLanguage);
		}

		private void Exit_Click(object sender, EventArgs e)
		{
			foreach (object checkedItem in clb_VisibleColumns.CheckedItems)
			{
				elements[clb_VisibleColumns.Items.IndexOf(checkedItem)] = true;
			}
			if (cb_PredefinedLanguage.Checked)
			{
				language = lb_Language.SelectedItem.ToString();
			}
			else
			{
				language = tb_projectLanguage.Text;
			}
			errorCodeAsHex = cb_ErrorCodeAsHex.Checked;
			showNegativeTimeWarning = cb_NegTimeWarn.Checked;
			showMissingEDTWarning = cb_ShowMissingEDT.Checked;
			ed_d_path = tb_ED_D.Text;
			output_file_path = tb_OutputPath.Text;
			takePositionFromEnv = !rb_TakeFromEventAttr.Checked;
			if (rb_CoordMRVC.Checked)
			{
				longitudeHi = "COORD_MRVC";
			}
			else
			{
				longitudeHi = cmb_LongitudeHigh.Text;
			}
			longitudeLo = cmb_LongitudeLow.Text;
			latitudeHi = cmb_LatitudeHi.Text;
			latitudeLo = cmb_LatitudeLo.Text;
			alternateRowColor = cb_AlternateRowColor.Checked;
			csvFilterSignalName = tb_SignalNameFilter.Text;
			sortColumn = cmb_BoxSorting.Text;
			sortAscending = rb_ColSortAscending.Checked;
			showTimesWithMilliSec = cb_TimeWithMilliSec.Checked;
			useLocalPcTime = cb_UseLocalTime.Checked;
			pathToSTSNT = tb_STSNT.Text;
			toolLanguage = GetLanguage();
			timeZoneShift = GetTimeZoneShift();
			selectedTimeZone = cmb_TimeZone.SelectedIndex;
			keepGapBetweenEvents = cb_NoEnvSigGap.Checked;
			suppressVersionConflictWarning = cb_SuppressVersionConflictWarning.Checked;
			Close();
		}

		private void buttonSelectAll_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < clb_VisibleColumns.Items.Count; i++)
			{
				clb_VisibleColumns.SetItemChecked(i, value: true);
			}
		}

		private void btn_DeselectAll_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < clb_VisibleColumns.Items.Count; i++)
			{
				clb_VisibleColumns.SetItemChecked(i, value: false);
			}
		}

		private void btn_Cancel_Click(object sender, EventArgs e)
		{
			cancel = true;
			Close();
		}

		private void btn_BrowseToEDDDirectory_Click(object sender, EventArgs e)
		{
			options_folderBrowserDialog.SelectedPath = tb_ED_D.Text;
			if (options_folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				ed_d_path = options_folderBrowserDialog.SelectedPath;
				tb_ED_D.Text = ed_d_path;
			}
		}

		private void tb_EDD_TextChanged(object sender, EventArgs e)
		{
		}

		private void lb_Language_SelectedIndexChanged(object sender, EventArgs e)
		{
			lb_Language.SelectedItem.ToString();
		}

		private void btn_OutputSavePath_Click(object sender, EventArgs e)
		{
			options_folderBrowserDialog.SelectedPath = output_file_path;
			if (options_folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				output_file_path = options_folderBrowserDialog.SelectedPath;
				tb_OutputPath.Text = output_file_path;
			}
		}

		private void btn_Font_Click(object sender, EventArgs e)
		{
			fontDialog1.Font = font;
			fontDialog1.ShowDialog();
			font = fontDialog1.Font;
			setFontString();
		}

		private void setFontString()
		{
			string text = font.Name + " " + font.Size + " ";
			if (font.Italic)
			{
				text += "Italic ";
			}
			if (font.Bold)
			{
				text += "Bold ";
			}
			labelFont.Text = text;
		}

		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void ShowPositionCombo(bool showLo, bool showHi)
		{
			cmb_LatitudeLo.Enabled = showLo;
			cmb_LatitudeHi.Enabled = showHi;
			cmb_LongitudeHigh.Enabled = showHi;
			cmb_LongitudeLow.Enabled = showLo;
			lbl_LongHi.Enabled = showHi;
			lbl_LongLo.Enabled = showLo;
			lbl_LatHi.Enabled = showHi;
			lbl_LatLo.Enabled = showLo;
			lbl_LongHi.Text = "Longitude";
			lbl_LongLo.Text = "Longitude";
			lbl_LatHi.Text = "Latitude";
			lbl_LatLo.Text = "Latitude";
			if (showLo == showHi)
			{
				lbl_LongHi.Text += " high word";
				lbl_LongLo.Text += " low word";
				lbl_LatHi.Text += " high word";
				lbl_LatLo.Text += " low word";
			}
			cmb_LatitudeLo.Visible = showLo;
			cmb_LatitudeHi.Visible = showHi;
			cmb_LongitudeHigh.Visible = showHi;
			cmb_LongitudeLow.Visible = showLo;
			lbl_LongHi.Visible = showHi;
			lbl_LongLo.Visible = showLo;
			lbl_LatHi.Visible = showHi;
			lbl_LatLo.Visible = showLo;
		}

		private void rb_TakeFromEventAttr_CheckedChanged(object sender, EventArgs e)
		{
			if (rb_TakeFromEventAttr.Checked)
			{
				ShowPositionCombo(showLo: false, showHi: false);
			}
		}

		private void rb_CoordRDS_CheckedChanged(object sender, EventArgs e)
		{
			if (rb_CoordRDS.Checked)
			{
				ShowPositionCombo(showLo: true, showHi: true);
			}
		}

		private void rb_CoordMRVC_CheckedChanged(object sender, EventArgs e)
		{
			if (rb_CoordMRVC.Checked)
			{
				ShowPositionCombo(showLo: true, showHi: false);
			}
		}

		private void cb_Sorting_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmb_BoxSorting.Text.Equals(""))
			{
				rb_ColSortAscending.Enabled = false;
				rb_ColSortDescending.Enabled = false;
			}
			else
			{
				rb_ColSortAscending.Enabled = true;
				rb_ColSortDescending.Enabled = true;
			}
		}

		private void btn_PathToSTSNT_Click(object sender, EventArgs e)
		{
			if (File.Exists(tb_STSNT.Text))
			{
				openFileDialogSTSNT.FileName = tb_STSNT.Text;
			}
			else
			{
				openFileDialogSTSNT.FileName = "";
			}
			if (openFileDialogSTSNT.ShowDialog() == DialogResult.OK)
			{
				tb_STSNT.Text = openFileDialogSTSNT.FileName;
			}
		}

		private void checkedListBoxToolLang_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			if (clb_ToolLang.CheckedItems.Count == 1)
			{
				if (e.CurrentValue == CheckState.Checked)
				{
					e.NewValue = CheckState.Checked;
					return;
				}
				int index = clb_ToolLang.CheckedIndices[0];
				clb_ToolLang.ItemCheck -= checkedListBoxToolLang_ItemCheck;
				clb_ToolLang.SetItemChecked(index, value: false);
				clb_ToolLang.ItemCheck += checkedListBoxToolLang_ItemCheck;
			}
		}

		private void SetLanguage(string languageCode)
		{
			string value = "English";
			if (languageCode.Equals("en"))
			{
				value = "English";
			}
			else if (languageCode.Equals("de"))
			{
				value = "Deutsch";
			}
			else if (languageCode.Equals("zh-CHS"))
			{
				value = "中文";
			}
			clb_ToolLang.SetItemChecked(clb_ToolLang.Items.IndexOf(value), value: true);
		}

		private string GetLanguage()
		{
			string text = "";
			string result = "";
			IEnumerator enumerator = clb_ToolLang.CheckedItems.GetEnumerator();
			try
			{
				if (enumerator.MoveNext())
				{
					string text2 = (string)enumerator.Current;
					text = text2;
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			if (text.Equals("Deutsch"))
			{
				result = "de";
			}
			else if (text.Equals("中文"))
			{
				result = "zh-CHS";
			}
			return result;
		}

		private void cb_PredefinedLanguage_CheckedChanged(object sender, EventArgs e)
		{
			SetLanguageField();
		}

		private void SetLanguageField()
		{
			tb_projectLanguage.Enabled = !cb_PredefinedLanguage.Checked;
			lb_Language.Enabled = cb_PredefinedLanguage.Checked;
			if (lb_Language.SelectedItem == null)
			{
				lb_Language.SetSelected(0, value: true);
			}
		}

		private void cmb_TimeZone_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void LoadTimeZone(int selIndex)
		{
			cmb_TimeZone.Items.Add(TVStr.DefaultTimeZone);
			cmb_TimeZone.Items.Add("(UTC-12:00) International Date Line West");
			cmb_TimeZone.Items.Add("(UTC-11:00) Coordinated Universal Time-11");
			cmb_TimeZone.Items.Add("(UTC-11:00) Samoa");
			cmb_TimeZone.Items.Add("(UTC-10:00) Hawaii");
			cmb_TimeZone.Items.Add("(UTC-09:00) Alaska");
			cmb_TimeZone.Items.Add("(UTC-08:00) Baja California");
			cmb_TimeZone.Items.Add("(UTC-08:00) Pacific Time (US & Canada)");
			cmb_TimeZone.Items.Add("(UTC-07:00) Arizona");
			cmb_TimeZone.Items.Add("(UTC-07:00) Chihuahua, La Paz, Mazatlan");
			cmb_TimeZone.Items.Add("(UTC-07:00) Mountain Time (US & Canada)");
			cmb_TimeZone.Items.Add("(UTC-06:00) Central America");
			cmb_TimeZone.Items.Add("(UTC-06:00) Central Time (US & Canada)");
			cmb_TimeZone.Items.Add("(UTC-06:00) Guadalajara, Mexico City, Monterrey");
			cmb_TimeZone.Items.Add("(UTC-06:00) Saskatchewan");
			cmb_TimeZone.Items.Add("(UTC-05:00) Bogota, Lima, Quito");
			cmb_TimeZone.Items.Add("(UTC-05:00) Eastern Time (US & Canada)");
			cmb_TimeZone.Items.Add("(UTC-05:00) Indiana (East)");
			cmb_TimeZone.Items.Add("(UTC-04:30) Caracas");
			cmb_TimeZone.Items.Add("(UTC-04:00) Asuncion");
			cmb_TimeZone.Items.Add("(UTC-04:00) Atlantic Time (Canada)");
			cmb_TimeZone.Items.Add("(UTC-04:00) Cuiaba");
			cmb_TimeZone.Items.Add("(UTC-04:00) Georgetown, La Paz, Manaus, San Juan");
			cmb_TimeZone.Items.Add("(UTC-04:00) Santiago");
			cmb_TimeZone.Items.Add("(UTC-03:30) Newfoundland");
			cmb_TimeZone.Items.Add("(UTC-03:00) Brasilia");
			cmb_TimeZone.Items.Add("(UTC-03:00) Buenos Aires");
			cmb_TimeZone.Items.Add("(UTC-03:00) Cayenne, Fortaleza");
			cmb_TimeZone.Items.Add("(UTC-03:00) Greenland");
			cmb_TimeZone.Items.Add("(UTC-03:00) Montevideo");
			cmb_TimeZone.Items.Add("(UTC-02:00) Coordinated Universal Time-02");
			cmb_TimeZone.Items.Add("(UTC-02:00) Mid-Atlantic");
			cmb_TimeZone.Items.Add("(UTC-01:00) Azores");
			cmb_TimeZone.Items.Add("(UTC-01:00) Cape Verde Island");
			cmb_TimeZone.Items.Add("(UTC) Casablanca");
			cmb_TimeZone.Items.Add("(UTC) Coordinated Universal Time");
			cmb_TimeZone.Items.Add("(UTC) Dublin, Edinburgh, Lisbon, London");
			cmb_TimeZone.Items.Add("(UTC) Monrovia, Reykjavik");
			cmb_TimeZone.Items.Add("(UTC+01:00) Amsterdam, Berlin, Bern, Strengelbach");
			cmb_TimeZone.Items.Add("(UTC+01:00) Rome, Stockholm, Vienna, Zaniglas");
			cmb_TimeZone.Items.Add("(UTC+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague");
			cmb_TimeZone.Items.Add("(UTC+01:00) Brussels, Copenhagen, Madrid, Paris, Harpolingen");
			cmb_TimeZone.Items.Add("(UTC+01:00) Sarajevo, Skopje, Warsaw, Zagreb, Uerikon");
			cmb_TimeZone.Items.Add("(UTC+01:00) West Central Africa");
			cmb_TimeZone.Items.Add("(UTC+01:00) Windhoek, ");
			cmb_TimeZone.Items.Add("(UTC+02:00) Amman");
			cmb_TimeZone.Items.Add("(UTC+02:00) Athens, Bucharest, Istanbul");
			cmb_TimeZone.Items.Add("(UTC+02:00) Beirut");
			cmb_TimeZone.Items.Add("(UTC+02:00) Cairo");
			cmb_TimeZone.Items.Add("(UTC+02:00) Damascus");
			cmb_TimeZone.Items.Add("(UTC+02:00) Harare, Pretoria");
			cmb_TimeZone.Items.Add("(UTC+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius");
			cmb_TimeZone.Items.Add("(UTC+02:00) Jerusalem");
			cmb_TimeZone.Items.Add("(UTC+02:00) Minsk");
			cmb_TimeZone.Items.Add("(UTC+03:00) Baghdad");
			cmb_TimeZone.Items.Add("(UTC+03:00) Kuwait, Riyadh");
			cmb_TimeZone.Items.Add("(UTC+03:00) Moscow, St. Petersburg, Volgograd");
			cmb_TimeZone.Items.Add("(UTC+03:00) Nairobi");
			cmb_TimeZone.Items.Add("(UTC+03:30) Tehran");
			cmb_TimeZone.Items.Add("(UTC+04:00) Abu Dhabi, Muscat");
			cmb_TimeZone.Items.Add("(UTC+04:00) Baku");
			cmb_TimeZone.Items.Add("(UTC+04:00) Port Louis");
			cmb_TimeZone.Items.Add("(UTC+04:00) Tbilisi");
			cmb_TimeZone.Items.Add("(UTC+04:00) Yerevan");
			cmb_TimeZone.Items.Add("(UTC+04:30) Kabul");
			cmb_TimeZone.Items.Add("(UTC+05:00) Ekaterinburg");
			cmb_TimeZone.Items.Add("(UTC+05:00) Islamabad, Karachi");
			cmb_TimeZone.Items.Add("(UTC+05:00) Tashkente");
			cmb_TimeZone.Items.Add("(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi");
			cmb_TimeZone.Items.Add("(UTC+05:30) Sri Jayawardenepura");
			cmb_TimeZone.Items.Add("(UTC+05:45) Kathmandu");
			cmb_TimeZone.Items.Add("(UTC+06:00) Astana");
			cmb_TimeZone.Items.Add("(UTC+06:00) Dhaka");
			cmb_TimeZone.Items.Add("(UTC+06:00) Novosibirsk");
			cmb_TimeZone.Items.Add("(UTC+06:30) Yangon (Rangoon)");
			cmb_TimeZone.Items.Add("(UTC+07:00) Bangkok, Hanoi, Jakarta");
			cmb_TimeZone.Items.Add("(UTC+07:00) Krasnoyarsk");
			cmb_TimeZone.Items.Add("(UTC+08:00) Beijing, Chongqing, Hong Kong, Urumqi");
			cmb_TimeZone.Items.Add("(UTC+08:00) Irkutsk");
			cmb_TimeZone.Items.Add("(UTC+08:00) Kuala Lumpur, Singapore");
			cmb_TimeZone.Items.Add("(UTC+08:00) Perth");
			cmb_TimeZone.Items.Add("(UTC+08:00) Taipei");
			cmb_TimeZone.Items.Add("(UTC+08:00) Ulaanbaatar");
			cmb_TimeZone.Items.Add("(UTC+09:00) Osaka, Sapporo, Tokyo");
			cmb_TimeZone.Items.Add("(UTC+09:00) Seoul");
			cmb_TimeZone.Items.Add("(UTC+09:00) Yakutsk");
			cmb_TimeZone.Items.Add("(UTC+09:30) Adelaide");
			cmb_TimeZone.Items.Add("(UTC+09:30) Darwin");
			cmb_TimeZone.Items.Add("(UTC+10:00) Brisbane");
			cmb_TimeZone.Items.Add("(UTC+10:00) Canberra, Melbourne, Sydney");
			cmb_TimeZone.Items.Add("(UTC+10:00) Guam, Port Moresbye");
			cmb_TimeZone.Items.Add("(UTC+10:00) Hobarte");
			cmb_TimeZone.Items.Add("(UTC+10:00) Vladivostok");
			cmb_TimeZone.Items.Add("(UTC+11:00) Magadan");
			cmb_TimeZone.Items.Add("(UTC+11:00) Solomon Is., New Caledonia");
			cmb_TimeZone.Items.Add("(UTC+12:00) Auckland, Wellington");
			cmb_TimeZone.Items.Add("(UTC+12:00) Coordinated Universal Time+12");
			cmb_TimeZone.Items.Add("(UTC+12:00) Fiji");
			cmb_TimeZone.Items.Add("(UTC+12:00) Petropavlovsk-Kamchatsky - Old");
			cmb_TimeZone.Items.Add("(UTC+13:00) Nuku'alofa");
			cmb_TimeZone.SelectedIndex = selIndex;
		}

		private double GetTimeZoneShift()
		{
			string input = cmb_TimeZone.Text;
			double num = 0.0;
			Regex regex = new Regex("-?\\d+");
			Match match = regex.Match(input);
			if (match.Success)
			{
				num = double.Parse(match.Value);
				match = match.NextMatch();
				if (match.Success)
				{
					num = ((!(num < 0.0)) ? (num + double.Parse(match.Value) / 60.0) : (num - double.Parse(match.Value) / 60.0));
				}
			}
			return num;
		}
	}
}
