using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using CLI;
using TDSView.Properties;

namespace TDSView
{
	public class MainForm : Form
	{
		private IContainer components;

		private MenuStrip menuStrip;

		private ToolStrip toolStrip;

		private SplitContainer splitContainer;

		private DataGridView dgv_Events;

		private DataGridView dgv_Env;

		private ToolStripMenuItem tsmnui_File;

		private ToolStripMenuItem tsmnui_OpenEDVFile;

		private OpenFileDialog openFileDialog_ED_V;

		private ToolStripMenuItem tsmnui_View;

		private ToolStripMenuItem tsmnui_ExportToSTSNT;

		private ToolStripMenuItem tsmnui_Exit;

		private ToolStripMenuItem tsmnui_Help;

		private ToolStripMenuItem tsmnui_AboutTDSView;

		private ToolStripButton tsbtn_Open_ED_V;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripMenuItem tsmnui_AllPrePostEnvironmentVariables;

		private ToolStripMenuItem tsmnui_Close;

		private ToolStripMenuItem tsmnui_Print;

		private PrintDialog printDialog;

		private ToolStripButton tsbtn_STSNT;

		private ToolStripButton tsbtn_Print;

		private OpenFileDialog open_ED_D_FileDialog;

		private ToolStripMenuItem tsmnui_Tools;

		private ToolStripMenuItem tsmnui_Options;

		private ToolStripMenuItem tsmnui_FindInEventDescription;

		private ToolStripMenuItem tsmnui_FindNext;

		private ToolStripButton tsbtn_Find;

		private ToolStripButton tsbtn_FindNext;

		private ToolStripSeparator toolStripSeparator2;

		private ToolStripMenuItem tsmnui_GeneralFileInformation;

		private StatusStrip statusStrip;

		private ToolStripStatusLabel toolStripStatusLabel1;

		private ToolStripStatusLabel toolStripStatusLabel2;

		private ToolStripStatusLabel toolStripStatusLabel3;

		private ToolStripStatusLabel toolStripStatusLabel4;

		private ToolStripButton tsbtn_GeneralInfo;

		private ContextMenuStrip contextMenuStrip_Events;

		private ToolStripMenuItem tsmnui_ShowRepairAndOtherText;

		private ToolStripMenuItem tsmnui_AddEDVFile;

		private ToolStripMenuItem tsmnui_ExportToEXCEL;

		private SaveFileDialog saveFileDialog;

		private ToolStripButton EXCEL_toolStripButton;

		private ToolStripSeparator toolStripSeparator3;

		private ToolStripSeparator toolStripSeparator5;

		private ToolStripSeparator toolStripSeparator4;

		private ToolStripMenuItem tsmnui_SavePackage;

		private SaveFileDialog saveFileDialogPackage;

		private ToolStripMenuItem tsmnui_OpenPackage;

		private OpenFileDialog openFileDialogPackage;

		private ToolStripSeparator toolStripSeparator6;

		private ContextMenuStrip contextMenuStrip_EnvData;

		private ToolStripMenuItem tsmnui_AddToEventColumn;

		private ToolStripMenuItem tsmnui_User1;

		private ToolStripMenuItem tsmnui_User2;

		private ToolStripMenuItem tsmnui_User3;

		private ToolStripMenuItem tsmnui_User4;

		private ToolStripMenuItem tsmnui_User5;

		private ToolStripMenuItem tsmnui_RemoveUserColumn;

		private ToolStripMenuItem tsmnui_ExportToText;

		private ToolStripMenuItem tsmnui_ShowMap;

		private ToolStripMenuItem tsmnui_ShowMap1;

		private ToolStripMenuItem tsmnui_ShowPathInGoogleEarth;

		private ToolStripMenuItem tsmnui_HelpContent;

		private ToolStripMenuItem tsmnui_HelpIndex;

		private ToolStripMenuItem tsmnui_ExportToTSV;

		private ToolStripMenuItem tsmnui_ShowEnvironmentInSTSNT;

		private ToolStripMenuItem tsmnui_Recent;

		private ToolStripSeparator toolStripSeparator7;

		private DataGridViewTextBoxColumn EnvVariableName;

		private DataGridViewTextBoxColumn EnvDescr;

		private DataGridViewTextBoxColumn EnvUnit;

		private ToolStripStatusLabel toolStripStatusLabel_Language;

		private ToolStripMenuItem tsmnui_Ranking;

		private ToolStripStatusLabel tsslbl_TimeShift;

		private ToolStripMenuItem configurationsToolStripMenuItem;

		private ToolStripMenuItem tsmnui_saveSettings;

		private ToolStripMenuItem tsmnui_getSettings;

		private ToolStripMenuItem tsmnui_resetSettingsToDefault;

		private SaveFileDialog saveConfigurationDialog;

		private OpenFileDialog loadConfigurationDialog;

		private ToolStripMenuItem tsmnui_exportToXML;

		private ToolStripMenuItem openInTreeToolStripMenuItem;

		private ToolStripStatusLabel toolStripStatusLabel5;

		private ToolStripMenuItem tsmnui_Filter;

		private ToolStripMenuItem hideDescriptionColumnToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator8;

		private ToolStripMenuItem openEDVForRawDataViewToolStripMenuItem;

		private DataGridView dgv_RawView;

		private OpenFileDialog openFileDialogRawView;

		private ToolStripMenuItem tsmnui_Update;

		private ED_V evl;

		private bool edvRawView;

		private string[] edv_elements_general = new string[31]
		{
			"REFERENCE_NR", "EVENT_ID", "PROCESS_ID", "LIMIT", "VEHICLE_POS", "SUBSYSTEM_NR", "LOCATION", "PRIO", "ERRORCODE_0", "ERRORCODE_1",
			"ERRORCODE_2", "ERRORCODE_3", "ACKNOW_0", "ACKNOW_1", "ACKNOW_2", "ACKNOW_3", "ERR_CODE_MISM", "ACTIVE", "DELETED", "UPLOADED",
			"EVENT_CNT", "START_TIME", "END_TIME", "ENV_DATA_CNT_START", "BLOCK_CNT_START", "OLD_BLOCK_IDX_START", "TRX_BLOCK_IDX_START", "ENV_DATA_CNT_END", "BLOCK_CNT_END", "OLD_BLOCK_IDX_END",
			"TRX_BLOCK_IDX_END"
		};

		private string[] edv_elements_2200 = new string[16]
		{
			"LATITUDE", "LONGITUDE", "ALTITUDE", "SPEED", "HEADING", "UTC_TIME", "ODOMETER", "TRIP", "DBS_ENV_DATA_CNT_START", "DBS_BLOCK_CNT_START",
			"DBS_OLD_BLOCK_IDX_START", "DBS_TRX_BLOCK_IDX_START", "DBS_ENV_DATA_CNT_END", "DBS_BLOCK_CNT_END", "DBS_OLD_BLOCK_IDX_END", "DBS_TRX_BLOCK_IDX_END"
		};

		public ED ed;

		public int returnValue;

		private string language;

		private string ed_d_path_options;

		private string output_file_path_options;

		private int actualSearchInx;

		private string actualSearchString;

		private bool searchInEventDescr;

		private bool searchInSignalName;

		private bool searchAll;

		private bool markAllFindings;

		private bool clickedContextCellIsOutOfEventGrid;

		private bool clickedContextCellIsOutOfEnvGrid;

		private DateTime earliestSelectedTime;

		private DateTime latestSelectedTime;

		private DataGridViewCell clickedContextCell;

		private DataGridViewCell clickedContextEnvCell;

		private CreateText ct;

		private CreateCsv cv;

		private bool batch_mode;

		private string batch_ED_V;

		private string batch_ED_D;

		private string batch_ED_D_Path;

		private string batchOutFileName;

		private string batch_TSV_signal_filter;

		private bool batch_wrongArgumentInBatchMode;

		private Process googleEarth;

		private IBrowser browser;

		private bool doDefaultSort;

		private bool startWithCommandLineParams;

		private bool wrongCommandLineArgumentsInOPMode;

		private int selectedUserColInx;

		private bool selectedColIsUserCol;

		private bool selectedColIsFilterable;

		private string selectedColName;

		private Stack<string> fltStack;

		private Color stdBackGround;

		private RankingList rList;

		private MRUClass mru;

		private DataTable eventTable = new DataTable();

		private bool eventTableDefined;

		private bool envVarShown;

		private bool batch_exportAsRawFile;

		private int batch_createRawFileRetVal;

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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TDSView.MainForm));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.dgv_Events = new System.Windows.Forms.DataGridView();
			this.contextMenuStrip_Events = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmnui_ShowRepairAndOtherText = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_RemoveUserColumn = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_ShowMap1 = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_ShowEnvironmentInSTSNT = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_Filter = new System.Windows.Forms.ToolStripMenuItem();
			this.dgv_Env = new System.Windows.Forms.DataGridView();
			this.EnvVariableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EnvDescr = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EnvUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.contextMenuStrip_EnvData = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmnui_AddToEventColumn = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_User1 = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_User2 = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_User3 = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_User4 = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_User5 = new System.Windows.Forms.ToolStripMenuItem();
			this.hideDescriptionColumnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.tsmnui_File = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_OpenEDVFile = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_AddEDVFile = new System.Windows.Forms.ToolStripMenuItem();
			this.openInTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_Close = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmnui_Recent = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmnui_OpenPackage = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_SavePackage = new System.Windows.Forms.ToolStripMenuItem();
			this.configurationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_saveSettings = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_getSettings = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_resetSettingsToDefault = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmnui_Print = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmnui_ExportToSTSNT = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_ExportToEXCEL = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_ExportToText = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_ExportToTSV = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_exportToXML = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this.openEDVForRawDataViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmnui_Exit = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_View = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_AllPrePostEnvironmentVariables = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_GeneralFileInformation = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_ShowMap = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_ShowPathInGoogleEarth = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_Ranking = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_Tools = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_Options = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_FindInEventDescription = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_FindNext = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_Help = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_AboutTDSView = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_HelpContent = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_HelpIndex = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmnui_Update = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.tsbtn_Open_ED_V = new System.Windows.Forms.ToolStripButton();
			this.tsbtn_Print = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbtn_Find = new System.Windows.Forms.ToolStripButton();
			this.tsbtn_FindNext = new System.Windows.Forms.ToolStripButton();
			this.tsbtn_GeneralInfo = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbtn_STSNT = new System.Windows.Forms.ToolStripButton();
			this.EXCEL_toolStripButton = new System.Windows.Forms.ToolStripButton();
			this.openFileDialog_ED_V = new System.Windows.Forms.OpenFileDialog();
			this.printDialog = new System.Windows.Forms.PrintDialog();
			this.open_ED_D_FileDialog = new System.Windows.Forms.OpenFileDialog();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel_Language = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsslbl_TimeShift = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.saveFileDialogPackage = new System.Windows.Forms.SaveFileDialog();
			this.openFileDialogPackage = new System.Windows.Forms.OpenFileDialog();
			this.saveConfigurationDialog = new System.Windows.Forms.SaveFileDialog();
			this.loadConfigurationDialog = new System.Windows.Forms.OpenFileDialog();
			this.dgv_RawView = new System.Windows.Forms.DataGridView();
			this.openFileDialogRawView = new System.Windows.Forms.OpenFileDialog();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgv_Events).BeginInit();
			this.contextMenuStrip_Events.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgv_Env).BeginInit();
			this.contextMenuStrip_EnvData.SuspendLayout();
			this.menuStrip.SuspendLayout();
			this.toolStrip.SuspendLayout();
			this.statusStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dgv_RawView).BeginInit();
			base.SuspendLayout();
			resources.ApplyResources(this.splitContainer, "splitContainer");
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Panel1.Controls.Add(this.dgv_Events);
			this.splitContainer.Panel2.Controls.Add(this.dgv_Env);
			this.dgv_Events.AllowUserToAddRows = false;
			this.dgv_Events.AllowUserToDeleteRows = false;
			this.dgv_Events.AllowUserToOrderColumns = true;
			this.dgv_Events.ContextMenuStrip = this.contextMenuStrip_Events;
			dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			dataGridViewCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgv_Events.DefaultCellStyle = dataGridViewCellStyle;
			resources.ApplyResources(this.dgv_Events, "dgv_Events");
			this.dgv_Events.Name = "dgv_Events";
			this.dgv_Events.RowHeadersVisible = false;
			this.dgv_Events.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dgv_Events.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgv_Events.MouseDown += new System.Windows.Forms.MouseEventHandler(dataGridViewEvents_MouseDown);
			this.dgv_Events.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(dgv_Events_SortCompare);
			this.dgv_Events.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(dgv_Events_CellClick);
			this.contextMenuStrip_Events.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.tsmnui_ShowRepairAndOtherText, this.tsmnui_RemoveUserColumn, this.tsmnui_ShowMap1, this.tsmnui_ShowEnvironmentInSTSNT, this.tsmnui_Filter });
			this.contextMenuStrip_Events.Name = "contextMenuStrip1";
			resources.ApplyResources(this.contextMenuStrip_Events, "contextMenuStrip_Events");
			this.contextMenuStrip_Events.Opening += new System.ComponentModel.CancelEventHandler(contextMenuStrip_Events_Opening);
			this.tsmnui_ShowRepairAndOtherText.Image = TDSView.Properties.Resources.Tool;
			this.tsmnui_ShowRepairAndOtherText.Name = "tsmnui_ShowRepairAndOtherText";
			resources.ApplyResources(this.tsmnui_ShowRepairAndOtherText, "tsmnui_ShowRepairAndOtherText");
			this.tsmnui_ShowRepairAndOtherText.Click += new System.EventHandler(repairTextToolStripMenuItem_Click);
			this.tsmnui_RemoveUserColumn.AutoToolTip = true;
			this.tsmnui_RemoveUserColumn.Image = TDSView.Properties.Resources.delete;
			resources.ApplyResources(this.tsmnui_RemoveUserColumn, "tsmnui_RemoveUserColumn");
			this.tsmnui_RemoveUserColumn.Name = "tsmnui_RemoveUserColumn";
			this.tsmnui_RemoveUserColumn.Click += new System.EventHandler(removeUserColumnToolStripMenuItem_Click);
			this.tsmnui_ShowMap1.AutoToolTip = true;
			this.tsmnui_ShowMap1.Image = TDSView.Properties.Resources.google_maps_icon;
			this.tsmnui_ShowMap1.Name = "tsmnui_ShowMap1";
			resources.ApplyResources(this.tsmnui_ShowMap1, "tsmnui_ShowMap1");
			this.tsmnui_ShowMap1.Click += new System.EventHandler(showMapToolStripMenuItem_Click);
			this.tsmnui_ShowEnvironmentInSTSNT.AutoToolTip = true;
			this.tsmnui_ShowEnvironmentInSTSNT.Image = TDSView.Properties.Resources.STSNT_BitMap2;
			this.tsmnui_ShowEnvironmentInSTSNT.Name = "tsmnui_ShowEnvironmentInSTSNT";
			resources.ApplyResources(this.tsmnui_ShowEnvironmentInSTSNT, "tsmnui_ShowEnvironmentInSTSNT");
			this.tsmnui_ShowEnvironmentInSTSNT.Click += new System.EventHandler(showEnvironmentInSTSNTToolStripMenuItem_Click);
			this.tsmnui_Filter.Image = TDSView.Properties.Resources.Filter2HS;
			this.tsmnui_Filter.Name = "tsmnui_Filter";
			resources.ApplyResources(this.tsmnui_Filter, "tsmnui_Filter");
			this.tsmnui_Filter.Click += new System.EventHandler(tsmnui_Filter_Click);
			this.dgv_Env.AllowUserToAddRows = false;
			this.dgv_Env.AllowUserToDeleteRows = false;
			this.dgv_Env.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgv_Env.Columns.AddRange(this.EnvVariableName, this.EnvDescr, this.EnvUnit);
			this.dgv_Env.ContextMenuStrip = this.contextMenuStrip_EnvData;
			resources.ApplyResources(this.dgv_Env, "dgv_Env");
			this.dgv_Env.Name = "dgv_Env";
			this.dgv_Env.ReadOnly = true;
			this.dgv_Env.RowHeadersVisible = false;
			this.dgv_Env.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgv_Env.MouseDown += new System.Windows.Forms.MouseEventHandler(dataGridViewEnv_MouseDown);
			this.EnvVariableName.Frozen = true;
			resources.ApplyResources(this.EnvVariableName, "EnvVariableName");
			this.EnvVariableName.Name = "EnvVariableName";
			this.EnvVariableName.ReadOnly = true;
			this.EnvDescr.Frozen = true;
			resources.ApplyResources(this.EnvDescr, "EnvDescr");
			this.EnvDescr.Name = "EnvDescr";
			this.EnvDescr.ReadOnly = true;
			this.EnvUnit.Frozen = true;
			resources.ApplyResources(this.EnvUnit, "EnvUnit");
			this.EnvUnit.Name = "EnvUnit";
			this.EnvUnit.ReadOnly = true;
			this.contextMenuStrip_EnvData.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsmnui_AddToEventColumn, this.hideDescriptionColumnToolStripMenuItem });
			this.contextMenuStrip_EnvData.Name = "contextMenuStrip2";
			this.contextMenuStrip_EnvData.ShowImageMargin = false;
			resources.ApplyResources(this.contextMenuStrip_EnvData, "contextMenuStrip_EnvData");
			this.tsmnui_AddToEventColumn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsmnui_AddToEventColumn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.tsmnui_User1, this.tsmnui_User2, this.tsmnui_User3, this.tsmnui_User4, this.tsmnui_User5 });
			this.tsmnui_AddToEventColumn.Name = "tsmnui_AddToEventColumn";
			resources.ApplyResources(this.tsmnui_AddToEventColumn, "tsmnui_AddToEventColumn");
			this.tsmnui_AddToEventColumn.Click += new System.EventHandler(addToEventColumnToolStripMenuItem_Click);
			this.tsmnui_User1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsmnui_User1.Name = "tsmnui_User1";
			resources.ApplyResources(this.tsmnui_User1, "tsmnui_User1");
			this.tsmnui_User1.Click += new System.EventHandler(user1ToolStripMenuItem_Click);
			this.tsmnui_User2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsmnui_User2.Name = "tsmnui_User2";
			resources.ApplyResources(this.tsmnui_User2, "tsmnui_User2");
			this.tsmnui_User2.Click += new System.EventHandler(user2ToolStripMenuItem_Click);
			this.tsmnui_User3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsmnui_User3.Name = "tsmnui_User3";
			resources.ApplyResources(this.tsmnui_User3, "tsmnui_User3");
			this.tsmnui_User3.Click += new System.EventHandler(user3ToolStripMenuItem_Click);
			this.tsmnui_User4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsmnui_User4.Name = "tsmnui_User4";
			resources.ApplyResources(this.tsmnui_User4, "tsmnui_User4");
			this.tsmnui_User4.Click += new System.EventHandler(user4ToolStripMenuItem_Click);
			this.tsmnui_User5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsmnui_User5.Name = "tsmnui_User5";
			resources.ApplyResources(this.tsmnui_User5, "tsmnui_User5");
			this.tsmnui_User5.Click += new System.EventHandler(user5ToolStripMenuItem_Click);
			this.hideDescriptionColumnToolStripMenuItem.Name = "hideDescriptionColumnToolStripMenuItem";
			resources.ApplyResources(this.hideDescriptionColumnToolStripMenuItem, "hideDescriptionColumnToolStripMenuItem");
			this.hideDescriptionColumnToolStripMenuItem.Click += new System.EventHandler(hideDescriptionColumnToolStripMenuItem_Click);
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.tsmnui_File, this.tsmnui_View, this.tsmnui_Tools, this.tsmnui_Help });
			resources.ApplyResources(this.menuStrip, "menuStrip");
			this.menuStrip.Name = "menuStrip";
			this.tsmnui_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[22]
			{
				this.tsmnui_OpenEDVFile, this.tsmnui_AddEDVFile, this.openInTreeToolStripMenuItem, this.tsmnui_Close, this.toolStripSeparator7, this.tsmnui_Recent, this.toolStripSeparator6, this.tsmnui_OpenPackage, this.tsmnui_SavePackage, this.configurationsToolStripMenuItem,
				this.toolStripSeparator5, this.tsmnui_Print, this.toolStripSeparator4, this.tsmnui_ExportToSTSNT, this.tsmnui_ExportToEXCEL, this.tsmnui_ExportToText, this.tsmnui_ExportToTSV, this.tsmnui_exportToXML, this.toolStripSeparator8, this.openEDVForRawDataViewToolStripMenuItem,
				this.toolStripSeparator1, this.tsmnui_Exit
			});
			this.tsmnui_File.Name = "tsmnui_File";
			resources.ApplyResources(this.tsmnui_File, "tsmnui_File");
			this.tsmnui_OpenEDVFile.Image = TDSView.Properties.Resources.OpenPH;
			this.tsmnui_OpenEDVFile.Name = "tsmnui_OpenEDVFile";
			resources.ApplyResources(this.tsmnui_OpenEDVFile, "tsmnui_OpenEDVFile");
			this.tsmnui_OpenEDVFile.Click += new System.EventHandler(Open_ED_V_File);
			this.tsmnui_AddEDVFile.Image = TDSView.Properties.Resources.OpenAdd;
			this.tsmnui_AddEDVFile.Name = "tsmnui_AddEDVFile";
			resources.ApplyResources(this.tsmnui_AddEDVFile, "tsmnui_AddEDVFile");
			this.tsmnui_AddEDVFile.Click += new System.EventHandler(addEDVFileToolStripMenuItem_Click);
			this.openInTreeToolStripMenuItem.Image = TDSView.Properties.Resources.Stuffed_Folder;
			this.openInTreeToolStripMenuItem.Name = "openInTreeToolStripMenuItem";
			resources.ApplyResources(this.openInTreeToolStripMenuItem, "openInTreeToolStripMenuItem");
			this.openInTreeToolStripMenuItem.Click += new System.EventHandler(openInTreeToolStripMenuItem_Click);
			this.tsmnui_Close.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this.tsmnui_Close.Name = "tsmnui_Close";
			resources.ApplyResources(this.tsmnui_Close, "tsmnui_Close");
			this.tsmnui_Close.Click += new System.EventHandler(closeOpenedGridsToolStripMenuItem_Click);
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
			this.tsmnui_Recent.Name = "tsmnui_Recent";
			resources.ApplyResources(this.tsmnui_Recent, "tsmnui_Recent");
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
			this.tsmnui_OpenPackage.Name = "tsmnui_OpenPackage";
			resources.ApplyResources(this.tsmnui_OpenPackage, "tsmnui_OpenPackage");
			this.tsmnui_OpenPackage.Click += new System.EventHandler(openPackageToolStripMenuItem_Click);
			this.tsmnui_SavePackage.Name = "tsmnui_SavePackage";
			resources.ApplyResources(this.tsmnui_SavePackage, "tsmnui_SavePackage");
			this.tsmnui_SavePackage.Click += new System.EventHandler(savePackageToolStripMenuItem_Click);
			this.configurationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.tsmnui_saveSettings, this.tsmnui_getSettings, this.tsmnui_resetSettingsToDefault });
			this.configurationsToolStripMenuItem.Name = "configurationsToolStripMenuItem";
			resources.ApplyResources(this.configurationsToolStripMenuItem, "configurationsToolStripMenuItem");
			this.tsmnui_saveSettings.Name = "tsmnui_saveSettings";
			resources.ApplyResources(this.tsmnui_saveSettings, "tsmnui_saveSettings");
			this.tsmnui_saveSettings.Click += new System.EventHandler(tsmnui_save_Click);
			this.tsmnui_getSettings.Name = "tsmnui_getSettings";
			resources.ApplyResources(this.tsmnui_getSettings, "tsmnui_getSettings");
			this.tsmnui_getSettings.Click += new System.EventHandler(tsmnui_load_Click);
			this.tsmnui_resetSettingsToDefault.Name = "tsmnui_resetSettingsToDefault";
			resources.ApplyResources(this.tsmnui_resetSettingsToDefault, "tsmnui_resetSettingsToDefault");
			this.tsmnui_resetSettingsToDefault.Click += new System.EventHandler(tsmnui_resetToDefault_Click);
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
			this.tsmnui_Print.Image = TDSView.Properties.Resources.PrintHH;
			resources.ApplyResources(this.tsmnui_Print, "tsmnui_Print");
			this.tsmnui_Print.Name = "tsmnui_Print";
			this.tsmnui_Print.Click += new System.EventHandler(printToolStripMenuItem_Click);
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
			this.tsmnui_ExportToSTSNT.Image = TDSView.Properties.Resources.STSNT_BitMap2;
			this.tsmnui_ExportToSTSNT.Name = "tsmnui_ExportToSTSNT";
			resources.ApplyResources(this.tsmnui_ExportToSTSNT, "tsmnui_ExportToSTSNT");
			this.tsmnui_ExportToSTSNT.Click += new System.EventHandler(exportToSTSNTToolStripMenuItem_Click);
			this.tsmnui_ExportToEXCEL.Image = TDSView.Properties.Resources._01EXCEL1;
			this.tsmnui_ExportToEXCEL.Name = "tsmnui_ExportToEXCEL";
			resources.ApplyResources(this.tsmnui_ExportToEXCEL, "tsmnui_ExportToEXCEL");
			this.tsmnui_ExportToEXCEL.Click += new System.EventHandler(exportToEXCELToolStripMenuItem_Click);
			this.tsmnui_ExportToText.Image = TDSView.Properties.Resources.Textbox;
			resources.ApplyResources(this.tsmnui_ExportToText, "tsmnui_ExportToText");
			this.tsmnui_ExportToText.Name = "tsmnui_ExportToText";
			this.tsmnui_ExportToText.Click += new System.EventHandler(exportToTextToolStripMenuItem_Click);
			resources.ApplyResources(this.tsmnui_ExportToTSV, "tsmnui_ExportToTSV");
			this.tsmnui_ExportToTSV.Name = "tsmnui_ExportToTSV";
			this.tsmnui_ExportToTSV.Click += new System.EventHandler(exportToCSVToolStripMenuItem_Click);
			resources.ApplyResources(this.tsmnui_exportToXML, "tsmnui_exportToXML");
			this.tsmnui_exportToXML.Name = "tsmnui_exportToXML";
			this.tsmnui_exportToXML.Click += new System.EventHandler(exportToXMLToolStripMenuItem_Click);
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
			resources.ApplyResources(this.openEDVForRawDataViewToolStripMenuItem, "openEDVForRawDataViewToolStripMenuItem");
			this.openEDVForRawDataViewToolStripMenuItem.Image = TDSView.Properties.Resources.ShowRulelines;
			this.openEDVForRawDataViewToolStripMenuItem.Name = "openEDVForRawDataViewToolStripMenuItem";
			this.openEDVForRawDataViewToolStripMenuItem.Click += new System.EventHandler(openEDVForRawDataViewToolStripMenuItem_Click);
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			this.tsmnui_Exit.Name = "tsmnui_Exit";
			resources.ApplyResources(this.tsmnui_Exit, "tsmnui_Exit");
			this.tsmnui_Exit.Click += new System.EventHandler(exitToolStripMenuItem_Click);
			this.tsmnui_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.tsmnui_AllPrePostEnvironmentVariables, this.tsmnui_GeneralFileInformation, this.tsmnui_ShowMap, this.tsmnui_ShowPathInGoogleEarth, this.tsmnui_Ranking });
			this.tsmnui_View.Name = "tsmnui_View";
			resources.ApplyResources(this.tsmnui_View, "tsmnui_View");
			this.tsmnui_AllPrePostEnvironmentVariables.CheckOnClick = true;
			this.tsmnui_AllPrePostEnvironmentVariables.Name = "tsmnui_AllPrePostEnvironmentVariables";
			resources.ApplyResources(this.tsmnui_AllPrePostEnvironmentVariables, "tsmnui_AllPrePostEnvironmentVariables");
			this.tsmnui_AllPrePostEnvironmentVariables.Click += new System.EventHandler(allPrePostEnvironmentVariablesToolStripMenuItem_Click);
			this.tsmnui_GeneralFileInformation.Image = TDSView.Properties.Resources.PropertiesHH;
			this.tsmnui_GeneralFileInformation.Name = "tsmnui_GeneralFileInformation";
			resources.ApplyResources(this.tsmnui_GeneralFileInformation, "tsmnui_GeneralFileInformation");
			this.tsmnui_GeneralFileInformation.Click += new System.EventHandler(generalFileInformationToolStripMenuItem_Click);
			this.tsmnui_ShowMap.Image = TDSView.Properties.Resources.google_maps_icon;
			this.tsmnui_ShowMap.Name = "tsmnui_ShowMap";
			resources.ApplyResources(this.tsmnui_ShowMap, "tsmnui_ShowMap");
			this.tsmnui_ShowMap.Click += new System.EventHandler(showMapToolStripMenuItem_Click);
			this.tsmnui_ShowPathInGoogleEarth.Image = TDSView.Properties.Resources.Google_earth;
			resources.ApplyResources(this.tsmnui_ShowPathInGoogleEarth, "tsmnui_ShowPathInGoogleEarth");
			this.tsmnui_ShowPathInGoogleEarth.Name = "tsmnui_ShowPathInGoogleEarth";
			this.tsmnui_ShowPathInGoogleEarth.Click += new System.EventHandler(showPathInGoogleEarthToolStripMenuItem_Click);
			this.tsmnui_Ranking.AutoToolTip = true;
			resources.ApplyResources(this.tsmnui_Ranking, "tsmnui_Ranking");
			this.tsmnui_Ranking.Name = "tsmnui_Ranking";
			this.tsmnui_Ranking.Click += new System.EventHandler(rankingListToolStripMenuItem_Click);
			this.tsmnui_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.tsmnui_Options, this.tsmnui_FindInEventDescription, this.tsmnui_FindNext });
			this.tsmnui_Tools.Name = "tsmnui_Tools";
			resources.ApplyResources(this.tsmnui_Tools, "tsmnui_Tools");
			this.tsmnui_Options.Image = TDSView.Properties.Resources.Options;
			resources.ApplyResources(this.tsmnui_Options, "tsmnui_Options");
			this.tsmnui_Options.Name = "tsmnui_Options";
			this.tsmnui_Options.Click += new System.EventHandler(optionsToolStripMenuItem_Click);
			this.tsmnui_FindInEventDescription.Image = TDSView.Properties.Resources.FindHH;
			resources.ApplyResources(this.tsmnui_FindInEventDescription, "tsmnui_FindInEventDescription");
			this.tsmnui_FindInEventDescription.Name = "tsmnui_FindInEventDescription";
			this.tsmnui_FindInEventDescription.Click += new System.EventHandler(findInEventDescriptionToolStripMenuItem_Click);
			this.tsmnui_FindNext.Image = TDSView.Properties.Resources.FindNextHS;
			resources.ApplyResources(this.tsmnui_FindNext, "tsmnui_FindNext");
			this.tsmnui_FindNext.Name = "tsmnui_FindNext";
			this.tsmnui_FindNext.Click += new System.EventHandler(findNextToolStripMenuItem_Click);
			this.tsmnui_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.tsmnui_AboutTDSView, this.tsmnui_HelpContent, this.tsmnui_HelpIndex, this.tsmnui_Update });
			this.tsmnui_Help.Name = "tsmnui_Help";
			resources.ApplyResources(this.tsmnui_Help, "tsmnui_Help");
			this.tsmnui_AboutTDSView.Image = TDSView.Properties.Resources.Information;
			resources.ApplyResources(this.tsmnui_AboutTDSView, "tsmnui_AboutTDSView");
			this.tsmnui_AboutTDSView.Name = "tsmnui_AboutTDSView";
			this.tsmnui_AboutTDSView.Click += new System.EventHandler(aboutTDSViewToolStripMenuItem_Click);
			this.tsmnui_HelpContent.Name = "tsmnui_HelpContent";
			resources.ApplyResources(this.tsmnui_HelpContent, "tsmnui_HelpContent");
			this.tsmnui_HelpContent.Click += new System.EventHandler(contentToolStripMenuItem_Click);
			this.tsmnui_HelpIndex.Name = "tsmnui_HelpIndex";
			resources.ApplyResources(this.tsmnui_HelpIndex, "tsmnui_HelpIndex");
			this.tsmnui_HelpIndex.Click += new System.EventHandler(indexToolStripMenuItem_Click);
			this.tsmnui_Update.Image = TDSView.Properties.Resources.FillDownHS;
			this.tsmnui_Update.Name = "tsmnui_Update";
			resources.ApplyResources(this.tsmnui_Update, "tsmnui_Update");
			this.tsmnui_Update.Click += new System.EventHandler(updateToolStripMenuItem_Click);
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.tsbtn_Open_ED_V, this.tsbtn_Print, this.toolStripSeparator2, this.tsbtn_Find, this.tsbtn_FindNext, this.tsbtn_GeneralInfo, this.toolStripSeparator3, this.tsbtn_STSNT, this.EXCEL_toolStripButton });
			resources.ApplyResources(this.toolStrip, "toolStrip");
			this.toolStrip.Name = "toolStrip";
			this.tsbtn_Open_ED_V.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbtn_Open_ED_V.Image = TDSView.Properties.Resources.OpenPH;
			resources.ApplyResources(this.tsbtn_Open_ED_V, "tsbtn_Open_ED_V");
			this.tsbtn_Open_ED_V.Name = "tsbtn_Open_ED_V";
			this.tsbtn_Open_ED_V.Click += new System.EventHandler(Open_ED_V_File);
			this.tsbtn_Print.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbtn_Print.Image = TDSView.Properties.Resources.PrintHH;
			resources.ApplyResources(this.tsbtn_Print, "tsbtn_Print");
			this.tsbtn_Print.Name = "tsbtn_Print";
			this.tsbtn_Print.Click += new System.EventHandler(printToolStripMenuItem_Click);
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			this.tsbtn_Find.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbtn_Find.Image = TDSView.Properties.Resources.FindHH;
			resources.ApplyResources(this.tsbtn_Find, "tsbtn_Find");
			this.tsbtn_Find.Name = "tsbtn_Find";
			this.tsbtn_Find.Click += new System.EventHandler(findInEventDescriptionToolStripMenuItem_Click);
			this.tsbtn_FindNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbtn_FindNext.Image = TDSView.Properties.Resources.FindNextHS;
			resources.ApplyResources(this.tsbtn_FindNext, "tsbtn_FindNext");
			this.tsbtn_FindNext.Name = "tsbtn_FindNext";
			this.tsbtn_FindNext.Click += new System.EventHandler(findNextToolStripMenuItem_Click);
			this.tsbtn_GeneralInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbtn_GeneralInfo.Image = TDSView.Properties.Resources.PropertiesHH;
			resources.ApplyResources(this.tsbtn_GeneralInfo, "tsbtn_GeneralInfo");
			this.tsbtn_GeneralInfo.Name = "tsbtn_GeneralInfo";
			this.tsbtn_GeneralInfo.Click += new System.EventHandler(generalFileInformationToolStripMenuItem_Click);
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
			this.tsbtn_STSNT.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbtn_STSNT.Image = TDSView.Properties.Resources.STSNT_BitMap2;
			resources.ApplyResources(this.tsbtn_STSNT, "tsbtn_STSNT");
			this.tsbtn_STSNT.Name = "tsbtn_STSNT";
			this.tsbtn_STSNT.Click += new System.EventHandler(exportToSTSNTToolStripMenuItem_Click);
			this.EXCEL_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.EXCEL_toolStripButton.Image = TDSView.Properties.Resources._01EXCEL1;
			resources.ApplyResources(this.EXCEL_toolStripButton, "EXCEL_toolStripButton");
			this.EXCEL_toolStripButton.Name = "EXCEL_toolStripButton";
			this.EXCEL_toolStripButton.Click += new System.EventHandler(exportToEXCELToolStripMenuItem_Click);
			resources.ApplyResources(this.openFileDialog_ED_V, "openFileDialog_ED_V");
			this.printDialog.UseEXDialog = true;
			resources.ApplyResources(this.open_ED_D_FileDialog, "open_ED_D_FileDialog");
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripStatusLabel1, this.toolStripStatusLabel2, this.toolStripStatusLabel3, this.toolStripStatusLabel4, this.toolStripStatusLabel_Language, this.tsslbl_TimeShift, this.toolStripStatusLabel5 });
			resources.ApplyResources(this.statusStrip, "statusStrip");
			this.statusStrip.Name = "statusStrip";
			this.toolStripStatusLabel1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.All;
			this.toolStripStatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
			this.toolStripStatusLabel2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.All;
			this.toolStripStatusLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
			this.toolStripStatusLabel3.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.All;
			this.toolStripStatusLabel3.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
			this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
			resources.ApplyResources(this.toolStripStatusLabel3, "toolStripStatusLabel3");
			this.toolStripStatusLabel4.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.All;
			this.toolStripStatusLabel4.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
			this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
			resources.ApplyResources(this.toolStripStatusLabel4, "toolStripStatusLabel4");
			this.toolStripStatusLabel_Language.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.All;
			this.toolStripStatusLabel_Language.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
			this.toolStripStatusLabel_Language.Name = "toolStripStatusLabel_Language";
			resources.ApplyResources(this.toolStripStatusLabel_Language, "toolStripStatusLabel_Language");
			this.tsslbl_TimeShift.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.All;
			this.tsslbl_TimeShift.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
			this.tsslbl_TimeShift.Name = "tsslbl_TimeShift";
			resources.ApplyResources(this.tsslbl_TimeShift, "tsslbl_TimeShift");
			resources.ApplyResources(this.toolStripStatusLabel5, "toolStripStatusLabel5");
			this.toolStripStatusLabel5.ForeColor = System.Drawing.Color.Red;
			this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
			this.toolStripStatusLabel5.Spring = true;
			resources.ApplyResources(this.saveConfigurationDialog, "saveConfigurationDialog");
			resources.ApplyResources(this.loadConfigurationDialog, "loadConfigurationDialog");
			this.dgv_RawView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			resources.ApplyResources(this.dgv_RawView, "dgv_RawView");
			this.dgv_RawView.Name = "dgv_RawView";
			resources.ApplyResources(this.openFileDialogRawView, "openFileDialogRawView");
			this.AllowDrop = true;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.splitContainer);
			base.Controls.Add(this.toolStrip);
			base.Controls.Add(this.menuStrip);
			base.Controls.Add(this.statusStrip);
			base.Controls.Add(this.dgv_RawView);
			base.MainMenuStrip = this.menuStrip;
			base.Name = "MainForm";
			base.Load += new System.EventHandler(MainForm_Load);
			base.DragDrop += new System.Windows.Forms.DragEventHandler(MainForm_DragDrop);
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(MainForm_FormClosed);
			base.DragEnter += new System.Windows.Forms.DragEventHandler(MainForm_DragEnter);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			this.splitContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.dgv_Events).EndInit();
			this.contextMenuStrip_Events.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.dgv_Env).EndInit();
			this.contextMenuStrip_EnvData.ResumeLayout(false);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.dgv_RawView).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void openEDVForRawDataViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CloseActualView();
			dgv_RawView.Columns.Clear();
			tsmnui_Close.Enabled = true;
			dgv_RawView.Visible = true;
			dgv_RawView.RowHeadersVisible = false;
			dgv_RawView.AllowUserToOrderColumns = true;
			dgv_RawView.AllowUserToDeleteRows = false;
			dgv_RawView.AllowUserToAddRows = false;
			dgv_RawView.AllowUserToResizeRows = false;
			dgv_RawView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dgv_RawView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			openFileDialogRawView.Title = TVStr.Open_ED_VFile + " - Raw View";
			TDSViewUtil.ofdTitle = openFileDialog_ED_V.Title;
			new Thread(TDSViewUtil.OfnModify).Start();
			openFileDialogRawView.ShowDialog();
			if (File.Exists(openFileDialogRawView.FileName))
			{
				Text = TVStr.TDSView + " - Raw View - " + Path.GetFileName(openFileDialogRawView.FileName);
				evl = new ED_V(useLocalTime: false, 0.0);
				if (evl.Read_ED_V_File(openFileDialogRawView.FileName) != 0)
				{
					MessageBox.Show(TVStr.InvalidEdvFile, TVStr.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				for (int i = 0; i < edv_elements_general.Length; i++)
				{
					dgv_RawView.Columns.Add(edv_elements_general[i], edv_elements_general[i]);
				}
				if (evl.header.ed_version_int >= 2200)
				{
					for (int j = 0; j < edv_elements_2200.Length; j++)
					{
						dgv_RawView.Columns.Add(edv_elements_2200[j], edv_elements_2200[j]);
					}
				}
				foreach (ED_V_Data datum in evl.data)
				{
					string text = TDSViewUtil.TimeDate2Str(datum.start_time, datum.start_time.Millisecond) + "\t";
					string text2 = "";
					if (datum.end_time_sec != uint.MaxValue)
					{
						text2 = TDSViewUtil.TimeDate2Str(datum.end_time, datum.end_time.Millisecond) + "\t";
					}
					DateTime dateTime = ConvertFromUnixTimestamp(datum.UTC_time);
					TDSViewUtil.TimeDate2Str(dateTime);
					dgv_RawView.Rows.Add(datum.reference_nr.ToString(), datum.event_id.ToString(), datum.process_id.ToString(), datum.limit.ToString(), datum.vehicle_pos.ToString(), datum.subsystem_nr.ToString(), datum.location.ToString(), datum.prio.ToString(), datum.errorcode_0.ToString("X4"), datum.errorcode_1.ToString("X4"), datum.errorcode_2.ToString("X4"), datum.errorcode_3.ToString("X4"), datum.acknow_0.ToString(), datum.acknow_1.ToString(), datum.acknow_2.ToString(), datum.acknow_3.ToString(), datum.err_code_mism.ToString(), datum.active.ToString(), datum.deleted.ToString(), datum.uploaded.ToString(), datum.event_cnt.ToString(), text, text2, datum.env_data_cnt_start.ToString(), datum.block_cnt_start.ToString(), datum.old_block_idx_start.ToString(), datum.trx_block_idx_start.ToString(), datum.env_data_cnt_end.ToString(), datum.block_cnt_end.ToString(), datum.old_block_idx_end.ToString(), datum.trx_block_idx_end.ToString(), datum.latitude.ToString(), datum.longitude.ToString(), datum.altitude.ToString(), datum.speed.ToString(), datum.heading.ToString(), dateTime, datum.odometer.ToString(), datum.trip.ToString(), datum.dbs_env_data_cnt_start.ToString(), datum.dbs_block_cnt_start.ToString(), datum.dbs_old_block_idx_start.ToString(), datum.dbs_trx_block_idx_start.ToString(), datum.dbs_env_data_cnt_end.ToString(), datum.dbs_block_cnt_end.ToString(), datum.dbs_old_block_idx_end.ToString(), datum.dbs_trx_block_idx_end.ToString());
				}
				dgv_RawView.BringToFront();
			}
			dgv_RawView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
			tsmnui_GeneralFileInformation.Enabled = true;
			tsbtn_GeneralInfo.Enabled = true;
			edvRawView = true;
		}

		private void closeRawViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CloseRawView();
		}

		private void CloseRawView()
		{
			dgv_RawView.Columns.Clear();
			dgv_RawView.Visible = false;
			edvRawView = false;
		}

		private DateTime ConvertFromUnixTimestamp(uint timestamp)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp);
		}

		public MainForm(string[] args)
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.ToolLanguage);
			InitializeComponent();
			SetElementText();
			ed = new ED();
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			Version version = executingAssembly.GetName().Version;
			string applicationVersion = version.ToString();
			if (Settings.Default.ApplicationVersion != version.ToString())
			{
				Settings.Default.Upgrade();
				Settings.Default.ApplicationVersion = applicationVersion;
			}
			dgv_Events.ReadOnly = true;
			actualSearchInx = 0;
			actualSearchString = "";
			searchInEventDescr = true;
			searchInSignalName = true;
			searchAll = false;
			markAllFindings = false;
			doDefaultSort = true;
			ED_V edv = null;
			ShowStatusStrip(show: false, edv);
			ShowGridViews(showIt: false);
			dgv_Env.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			dgv_Env.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
			dgv_Events.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			dgv_Events.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			printDialog.PrinterSettings.DefaultPageSettings.Landscape = true;
			DefineEventTable();
			GetSettings();
			earliestSelectedTime = new DateTime(DateTime.MaxValue.Ticks);
			latestSelectedTime = new DateTime(DateTime.MinValue.Ticks);
			mru = new MRUClass();
			mru.TakeSettings(Settings.Default.MRUList);
			batch_mode = false;
			startWithCommandLineParams = false;
			batch_wrongArgumentInBatchMode = false;
			wrongCommandLineArgumentsInOPMode = false;
			batch_TSV_signal_filter = "";
			fltStack = new Stack<string>();
			eventTableDefined = false;
			if (args.Length <= 0)
			{
				return;
			}
			batch_mode = true;
			batch_wrongArgumentInBatchMode = true;
			bool flag = false;
			batch_exportAsRawFile = false;
			global::CLI.CLI cLI = new global::CLI.CLI(args, new string[9] { "TXT", "TSV", "RAW", "OP", "XML", "L", "OUT", "FILT", "ENV" }, 0, 2);
			string[] array = cLI.CheckCommmandLine();
			if (array != null)
			{
				Console.WriteLine("Command line error");
				string[] array2 = array;
				foreach (string text in array2)
				{
					if (text != null)
					{
						Console.WriteLine(text);
					}
				}
				if (cLI.IsOption("OP"))
				{
					startWithCommandLineParams = true;
					wrongCommandLineArgumentsInOPMode = true;
				}
				return;
			}
			if (cLI.IsOption("RAW"))
			{
				cLI.GetArgument(1, out var attr);
				CreateRawDataCsv createRawDataCsv = new CreateRawDataCsv();
				batch_createRawFileRetVal = createRawDataCsv.CreateTextFile(attr);
				batch_wrongArgumentInBatchMode = false;
				batch_exportAsRawFile = true;
				return;
			}
			cLI.GetArgument(1, out batch_ED_V);
			if (!cLI.GetOptionArgument("L", out language))
			{
				language = "EN";
			}
			if (cLI.IsOption("OP"))
			{
				startWithCommandLineParams = true;
				if (cLI.numberOfArgs == 2)
				{
					cLI.GetArgument(2, out batch_ED_D);
					batch_wrongArgumentInBatchMode = false;
					language = "";
					batchOutFileName = "";
					batch_mode = true;
				}
				else
				{
					wrongCommandLineArgumentsInOPMode = true;
				}
			}
			else if (cLI.IsOption("TXT") || cLI.IsOption("XML"))
			{
				flag = cLI.IsOption("XML");
				cLI.GetArgument(2, out batch_ED_D_Path);
				bool givenName = false;
				if (!cLI.GetOptionArgument("OUT", out batchOutFileName))
				{
					batchOutFileName = "";
				}
				else
				{
					givenName = true;
				}
				ct = new CreateText(ed, flag, givenName, language);
				batch_wrongArgumentInBatchMode = false;
			}
			else
			{
				if (!cLI.IsOption("TSV"))
				{
					return;
				}
				cLI.GetArgument(2, out batch_ED_D_Path);
				if (!cLI.GetOptionArgument("OUT", out batchOutFileName))
				{
					batchOutFileName = "$DEFAULT_OUT$";
				}
				if (!cLI.GetOptionArgument("FILT", out batch_TSV_signal_filter))
				{
					if (cLI.IsOption("ENV"))
					{
						batch_TSV_signal_filter = "$ALL_ENV$";
					}
					else
					{
						batch_TSV_signal_filter = "$ALL$";
					}
				}
				cv = new CreateCsv(ed, language);
				batch_wrongArgumentInBatchMode = false;
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			if (batch_mode)
			{
				int num = 2;
				if (batch_wrongArgumentInBatchMode)
				{
					num = 7;
				}
				else if (batch_exportAsRawFile)
				{
					num = batch_createRawFileRetVal;
				}
				else if (File.Exists(batch_ED_V))
				{
					ed_d_path_options = batch_ED_D_Path;
					num = Read_OTI_Files(batch_ED_V, "", addIt: false, batchMode: true, updateTableAfterReading: true);
					if (num == 0 && !startWithCommandLineParams)
					{
						if (batch_TSV_signal_filter.Equals(""))
						{
							if (batchOutFileName.Equals(""))
							{
								batchOutFileName = ct.GetOutfileName(batch_ED_V);
							}
							num = ct.CreateTextFile(batchOutFileName);
						}
						else
						{
							if (batchOutFileName.Equals("$DEFAULT_OUT$"))
							{
								batchOutFileName = cv.GetOutfileName(batch_ED_V);
							}
							num = cv.CreateTextFile(batchOutFileName, batch_TSV_signal_filter);
						}
					}
				}
				else
				{
					num = 2;
				}
				returnValue = num;
				if (!startWithCommandLineParams)
				{
					Close();
				}
			}
			if (wrongCommandLineArgumentsInOPMode)
			{
				MessageBox.Show(TVStr.InvalidCommandLineArguments, TVStr.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			if (!Settings.Default.settingsSaved)
			{
				TDSViewUtil.ShowInstallationText();
			}
			if (startWithCommandLineParams && !wrongCommandLineArgumentsInOPMode)
			{
				Read_OTI_Files(batch_ED_V, batch_ED_D, addIt: false, batchMode: false, updateTableAfterReading: true);
			}
			batch_mode = false;
			batch_wrongArgumentInBatchMode = false;
			startWithCommandLineParams = false;
			FillRecentList();
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			SaveActualSettings();
			Settings.Default.MRUList = mru;
			Settings.Default.Save();
		}

		private void GetSettings()
		{
			StringCollection dataGridViewFormColumns = Settings.Default.DataGridViewFormColumns;
			if (dataGridViewFormColumns != null)
			{
				string[] array = new string[dataGridViewFormColumns.Count];
				dataGridViewFormColumns.CopyTo(array, 0);
				Array.Sort(array);
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split(',');
					int index = int.Parse(array2[3]);
					dgv_Events.Columns[index].DisplayIndex = short.Parse(array2[0]);
					dgv_Events.Columns[index].Width = short.Parse(array2[1]);
					dgv_Events.Columns[index].Visible = bool.Parse(array2[2]);
				}
			}
			SetAlternatingRowColor(Settings.Default.alternateRowColor);
			language = Settings.Default.language;
			toolStripStatusLabel_Language.Text = language;
			WriteTimeZoneShiftLbl();
			ed_d_path_options = Settings.Default.ed_d_path;
			output_file_path_options = Settings.Default.stsnt_save_path;
			if (Settings.Default.colVisibleColumns == 0)
			{
				Settings.Default.colVisibleColumns = 959uL;
			}
			ulong num = 1uL;
			foreach (DataGridViewColumn column in dgv_Events.Columns)
			{
				column.Visible = (Settings.Default.colVisibleColumns & num) != 0;
				num <<= 1;
			}
			tsmnui_AllPrePostEnvironmentVariables.Checked = Settings.Default.prePostEnvVarVisible;
			dgv_Events.DefaultCellStyle.Font = Settings.Default.fontGrid;
			dgv_Env.DefaultCellStyle.Font = Settings.Default.fontGrid;
			dgv_Events.Columns["User1"].HeaderText = Settings.Default.ColUser1;
			dgv_Events.Columns["User2"].HeaderText = Settings.Default.ColUser2;
			dgv_Events.Columns["User3"].HeaderText = Settings.Default.ColUser3;
			dgv_Events.Columns["User4"].HeaderText = Settings.Default.ColUser4;
			dgv_Events.Columns["User5"].HeaderText = Settings.Default.ColUser5;
			UpdateUserColMenu();
			base.Height = Settings.Default.MainFormHeight;
			base.Width = Settings.Default.MainFormWidth;
			splitContainer.SplitterDistance = Settings.Default.MainFormSplitterDistance;
			if (Settings.Default.EnvGridHideDescription)
			{
				dgv_Env.Columns["EnvDescr"].Visible = false;
				hideDescriptionColumnToolStripMenuItem.Text = TVStr.EnvGridShowDescrCol;
			}
			else
			{
				dgv_Env.Columns["EnvDescr"].Visible = true;
				hideDescriptionColumnToolStripMenuItem.Text = TVStr.EnvGridHideDescrCol;
			}
		}

		private void tsmnui_save_Click(object sender, EventArgs e)
		{
			saveConfigurationDialog.Title = TVStr.SaveConfiguration;
			saveConfigurationDialog.Filter = TVStr.SettingeFile + " (*.TVS)|*.TVS|" + TVStr.AllFiles + "(*.*)|*.*";
			if (saveConfigurationDialog.ShowDialog() == DialogResult.OK)
			{
				SaveActualSettings();
				UserSettings userSettings = new UserSettings();
				userSettings.SaveSettings(saveConfigurationDialog.FileName);
			}
		}

		private void tsmnui_load_Click(object sender, EventArgs e)
		{
			loadConfigurationDialog.Title = TVStr.GetConfiguration;
			loadConfigurationDialog.Filter = TVStr.SettingeFile + " (*.TVS)|*.TVS|" + TVStr.AllFiles + "(*.*)|*.*";
			if (loadConfigurationDialog.ShowDialog() == DialogResult.OK)
			{
				SaveActualSettings();
				UserSettings userSettings = new UserSettings();
				if (userSettings.GetSettings(loadConfigurationDialog.FileName) != 0)
				{
					Settings.Default.Reset();
				}
				GetSettings();
			}
		}

		private void tsmnui_resetToDefault_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(TVStr.ReallyWantToReset, TVStr.SettingsBackToDefault, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
			{
				SaveActualSettings();
				Settings.Default.Reset();
				GetSettings();
			}
		}

		private void SaveActualSettings()
		{
			int num = 0;
			ulong num2 = 0uL;
			Settings.Default.colVisibleColumns = 0uL;
			Settings.Default.ed_d_path = ed_d_path_options;
			Settings.Default.stsnt_save_path = output_file_path_options;
			foreach (DataGridViewColumn column in dgv_Events.Columns)
			{
				if (column.Visible)
				{
					num2 = 1uL;
					num2 <<= num;
				}
				Settings.Default.colVisibleColumns = Settings.Default.colVisibleColumns | num2;
				num++;
			}
			if (base.WindowState == FormWindowState.Maximized || base.WindowState == FormWindowState.Minimized)
			{
				Settings.Default.MainFormHeight = base.RestoreBounds.Height;
				Settings.Default.MainFormWidth = base.RestoreBounds.Width;
				Settings.Default.MainFormSplitterDistance = splitContainer.SplitterDistance;
			}
			else
			{
				Settings.Default.MainFormHeight = base.Size.Height;
				Settings.Default.MainFormWidth = base.Size.Width;
				Settings.Default.MainFormSplitterDistance = splitContainer.SplitterDistance;
			}
			Settings.Default.EnvGridHideDescription = !dgv_Env.Columns["EnvDescr"].Visible;
			Settings.Default.settingsSaved = true;
			StringCollection stringCollection = new StringCollection();
			int num3 = 0;
			foreach (DataGridViewColumn column2 in dgv_Events.Columns)
			{
				stringCollection.Add(string.Format("{0},{1},{2},{3}", column2.DisplayIndex.ToString("D2"), column2.Width, column2.Visible, num3++));
			}
			Settings.Default.DataGridViewFormColumns = stringCollection;
		}

		private void Open_ED_V_File(object sender, EventArgs e)
		{
			Read_ED_V_File(addIt: false);
		}

		private void closeOpenedGridsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CloseActualView();
		}

		private void CloseActualView()
		{
			CloseRankingList();
			CloseRawView();
			ed.Clear();
			ED_V edv = null;
			ShowGridViews(showIt: false);
			ShowStatusStrip(show: false, edv);
			Text = TVStr.TDSView;
		}

		private void addEDVFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Read_ED_V_File(addIt: true);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void Read_ED_V_File(bool addIt)
		{
			int num = 0;
			openFileDialog_ED_V.Title = TVStr.Open_ED_VFile;
			TDSViewUtil.ofdTitle = openFileDialog_ED_V.Title;
			new Thread(TDSViewUtil.OfnModify).Start();
			openFileDialog_ED_V.ShowDialog();
			if (File.Exists(openFileDialog_ED_V.FileName))
			{
				num = Read_OTI_Files(openFileDialog_ED_V.FileName, "", addIt, batchMode: false, updateTableAfterReading: true);
			}
			if (num != 3 && ed.edvl.Count > 0)
			{
				ED_V eD_V = (ED_V)ed.edvl[ed.edvl.Count - 1];
				if (eD_V.data.Count == 0)
				{
					MessageBox.Show(TVStr.NoEvents, TVStr.Information, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					ShowGridViews(showIt: false);
				}
			}
		}

		private void ClearEvl(ED_V evl, int evlInx)
		{
			CloseRankingList();
			evl.Clear();
			ed.edvl.RemoveAt(evlInx);
		}

		private int Read_OTI_Files(string fileName, string ed_d_file_name_predefined, bool addIt, bool batchMode, bool updateTableAfterReading)
		{
			int num = 0;
			bool flag = false;
			CloseRawView();
			CloseRankingList();
			if (!addIt)
			{
				ed.Clear();
				ClearEventTable();
				if (dgv_Events.SortedColumn != null)
				{
					dgv_Events.SortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
				}
			}
			else if (ed.ED_V_FileIsAlreadyRead(fileName))
			{
				MessageBox.Show(TVStr.SelectedFileIsAlreadyRead, TVStr.Information, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return 6;
			}
			ED_V eD_V = new ED_V(Settings.Default.UseLocalPcTime, Settings.Default.TimeZoneShift);
			int evlInx = ed.edvl.Add(eD_V);
			num = eD_V.Read_ED_V_File(fileName);
			if (num != 0)
			{
				ClearEvl(eD_V, evlInx);
				if (!batchMode)
				{
					MessageBox.Show(TVStr.InvalidOTIVariableFile + fileName + "\n" + TVStr.SelectAnOtherOne, TVStr.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				return num;
			}
			if (!batch_mode)
			{
				SaveRecentFile(fileName);
			}
			string text = "";
			int num2 = 3;
			ED_D eD_D = null;
			ED_T eD_T = null;
			string requestedEEDFileName = "";
			string availableLanguage = "";
			string ed_d_fileName;
			if (ed_d_file_name_predefined.Equals(""))
			{
				num2 = TDSViewUtil.ED_D_FileName(eD_V.fileName, eD_V.header.prj_version, eD_V.header.prj_name, language, ed_d_path_options, out ed_d_fileName, out requestedEEDFileName, out availableLanguage);
			}
			else
			{
				ed_d_fileName = ed_d_file_name_predefined;
				if (File.Exists(ed_d_fileName))
				{
					num2 = 0;
				}
				else if (startWithCommandLineParams)
				{
					MessageBox.Show(TVStr.NotFoundOTI + " " + ed_d_fileName, TVStr.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			if (batch_mode && num2 != 0)
			{
				ClearEvl(eD_V, evlInx);
				return 3;
			}
			eD_D = ed.ED_D_FileIsAlreadyRead(ed_d_fileName);
			bool flag2 = eD_D != null;
			if (eD_D == null)
			{
				if (num2 == 0)
				{
					text = ed_d_fileName;
				}
				else
				{
					bool flag3 = false;
					text = "";
					if (!availableLanguage.Equals(""))
					{
						MissingOTI missingOTI = new MissingOTI();
						missingOTI.availableOTI = availableLanguage;
						missingOTI.ShowDialog();
						flag3 = missingOTI.browseOTI;
						text = missingOTI.selectedOTI;
					}
					else
					{
						flag3 = MessageBox.Show(TVStr.NotFoundOTI + " " + requestedEEDFileName + "\n" + TVStr.SearchIt, TVStr.NotFoundOTI, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
					}
					if (flag3)
					{
						if (open_ED_D_FileDialog.ShowDialog() == DialogResult.Cancel)
						{
							ClearEvl(eD_V, evlInx);
							return 3;
						}
						text = open_ED_D_FileDialog.FileName;
					}
					else if (text.Equals(""))
					{
						ClearEvl(eD_V, evlInx);
						return 3;
					}
					ed_d_path_options = Path.GetDirectoryName(text);
					if (!File.Exists(text))
					{
						if (!batchMode)
						{
							MessageBox.Show(TVStr.NotFoundOTI + " " + text, TVStr.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
						ClearEvl(eD_V, evlInx);
						return 3;
					}
				}
				eD_D = new ED_D();
				ed.eddl.Add(eD_D);
				if (eD_D.Read_ED_D_File(text) > 0)
				{
					if (!batchMode)
					{
						MessageBox.Show(TVStr.InvalidOTIDescr + " " + text, TVStr.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					ClearEvl(eD_V, evlInx);
					return 1;
				}
				if (((ED_D_META)eD_D.meta[0]).ed_version_int < eD_V.header.ed_version_int)
				{
					if (batchMode || Settings.Default.suppressVersionConflictWarning)
					{
						flag = !Settings.Default.suppressVersionConflictWarning;
					}
					else if (MessageBox.Show(TVStr.VersionMisMatch, TVStr.Error, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
					{
						ed.Clear();
						ED_V edv = null;
						ShowGridViews(showIt: false);
						ShowStatusStrip(show: false, edv);
						Text = TVStr.TDSView;
						ClearEvl(eD_V, evlInx);
						eD_D.Clear();
						return 8;
					}
				}
				string text2 = TDSViewUtil.Get_ED_T_FileName(text);
				if (File.Exists(text2))
				{
					eD_T = new ED_T();
					ed.edtl.Add(eD_T);
					if (eD_T.Read_ED_T_File(text2) > 0)
					{
						if (!batchMode)
						{
							MessageBox.Show(TVStr.InvalidOTIDescr + " " + text2, TVStr.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
						eD_T.Clear();
						eD_D.Clear();
						return 1;
					}
				}
				else
				{
					if (Settings.Default.showMissingEDTWarning && !batchMode)
					{
						MessageBox.Show(TVStr.CouldNotFind + " " + text2 + "\n" + TVStr.RepairTextNotShown, TVStr.Warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					text2 = "";
				}
			}
			else
			{
				string fileName2 = TDSViewUtil.Get_ED_T_FileName(ed_d_fileName);
				eD_T = ed.Get_ED_T_Data(fileName2);
			}
			string processId = "";
			IEnumerator enumerator = eD_V.data.GetEnumerator();
			try
			{
				if (enumerator.MoveNext())
				{
					ED_V_Data eD_V_Data = (ED_V_Data)enumerator.Current;
					processId = eD_V_Data.process_id.ToString("X2");
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
			ED_D_EnvBlock odbsEnvBlkFromProcessId = eD_D.getOdbsEnvBlkFromProcessId(processId);
			foreach (ED_V_Data datum in eD_V.data)
			{
				datum.code0 = eD_D.ECode0Descr(datum.errorcode_0);
				datum.code1 = eD_D.ECode1Descr(datum.errorcode_1);
				datum.code2 = eD_D.ECode2Descr(datum.errorcode_2);
				datum.code3 = eD_D.ECode3Descr(datum.errorcode_3);
				foreach (ED_D_Event item in eD_D.eventED)
				{
					short num3 = short.Parse(item.process_id, NumberStyles.AllowHexSpecifier);
					short num4 = short.Parse(item.event_id, NumberStyles.AllowHexSpecifier);
					if (num3 == Convert.ToInt16(datum.process_id) && num4 == Convert.ToInt16(datum.event_id))
					{
						datum.eventDescr = item;
						break;
					}
				}
				if (datum.eventDescr == null)
				{
					continue;
				}
				foreach (ED_D_EnvBlock item2 in eD_D.env_block)
				{
					if (item2.env_block_id == datum.eventDescr.env_block_id)
					{
						datum.envBlock = item2;
					}
				}
				datum.envBlockODBSGrp = odbsEnvBlkFromProcessId;
			}
			if (!flag2)
			{
				foreach (ED_D_EnvBlock item3 in eD_D.env_block)
				{
					foreach (ED_D_EnvSignal item4 in eD_D.env_signal)
					{
						if (item4.env_block_id == item3.env_block_id)
						{
							item3.envSignal.Add(item4);
						}
					}
					IComparer comparer = new CompEnvSignals();
					item3.envSignal.Sort(comparer);
				}
			}
			eD_V.ed_d = eD_D;
			eD_V.ed_t = eD_T;
			if (!batchMode)
			{
				doDefaultSort = true;
				if (updateTableAfterReading)
				{
					UpdateEventTable();
				}
				ShowGridViews(showIt: true);
				if (ed.edvl.Count > 1)
				{
					Text = TVStr.MultipleEventFilesOpen;
				}
				else
				{
					Text = TVStr.TDSView + " " + Path.GetFileName(fileName);
				}
				AutoResizeEventColumns();
			}
			if (flag)
			{
				return 8;
			}
			return 0;
		}

		private void AutoResizeEventColumns()
		{
			WriteTemporaryStatusField(TVStr.ResizeColumns);
			Application.DoEvents();
			dgv_Events.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
			WriteTemporaryStatusField("");
		}

		private void DefineEventTable()
		{
			if (eventTableDefined)
			{
				return;
			}
			eventTableDefined = true;
			AddColumn("RefNr", TVStr.evDgvRefNr);
			AddColumn("StartTime", TVStr.evDgvStartTime);
			AddColumn("EndTime", TVStr.evDgvEndTime);
			AddColumn("Duration", TVStr.evDgvDuration);
			AddColumn("DistText", TVStr.evDgvDistText);
			AddColumn("ErrorCode0", TVStr.evDgvErrorCode0);
			AddColumn("ECode0Desc", TVStr.evDgvECode0Desc);
			AddColumn("SignalName", TVStr.evDgvSignalName);
			AddColumn("EnvBlockId", TVStr.evDgvEnvBlockId);
			AddColumn("Count", TVStr.evDgvCount);
			AddColumn("EventId", TVStr.evDgvEventId);
			AddColumn("Limit", TVStr.evDgvLimit);
			AddColumn("VehiclePos", TVStr.evDgvVehiclePos);
			AddColumn("ProcessId", TVStr.evDgvProcessId);
			AddColumn("Subsystem", TVStr.evDgvSubsystem);
			AddColumn("SubsysDesc", TVStr.evDgvSubsysDesc);
			AddColumn("Priority", TVStr.evDgvPriority);
			AddColumn("ErrorCode1", TVStr.evDgvErrorCode1);
			AddColumn("ECode1Desc", TVStr.evDgvECode1Desc);
			AddColumn("ErrorCode2", TVStr.evDgvErrorCode2);
			AddColumn("ECode2Desc", TVStr.evDgvECode2Desc);
			AddColumn("ErrorCode3", TVStr.evDgvErrorCode3);
			AddColumn("ECode3Desc", TVStr.evDgvECode3Desc);
			AddColumn("UniqueRef", TVStr.evDgvUniqueRef);
			AddColumn("VehicleName", TVStr.evDgvVehicleName);
			AddColumn("User1", TVStr.evDgvUser1);
			AddColumn("User2", TVStr.evDgvUser2);
			AddColumn("User3", TVStr.evDgvUser3);
			AddColumn("User4", TVStr.evDgvUser4);
			AddColumn("User5", TVStr.evDgvUser5);
			AddColumn("Position", TVStr.evDgvPosition);
			AddColumn("Altitude", TVStr.evDgvAltitude);
			AddColumn("Speed", TVStr.evDgvSpeed);
			AddColumn("Heading", TVStr.evDgvHeading);
			AddColumn("UTCTime", TVStr.evDgvUTCTime);
			AddColumn("Odometer", TVStr.evDgvOdometer);
			AddColumn("Trip", TVStr.evDgvTrip);
			AddColumn("rep_prio_active", "rep_prio_active");
			AddColumn("rep_prio_passive", "rep_prio_passive");
			AddColumn("extra_prio", "extra_prio");
			AddColumn("extra_attrib1", "extra attrib1");
			AddColumn("extra_attrib2", "extra attrib2");
			AddColumn("extra_attrib3", "extra attrib3");
			AddColumn("extra_attrib4", "extra attrib4");
			AddColumn("extra_attrib5", "extra attrib5");
			dgv_Events.DataSource = eventTable;
			dgv_Events.Columns["Count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dgv_Events.Columns["RefNr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			stdBackGround = dgv_Events.Columns[1].HeaderCell.Style.BackColor;
			foreach (DataGridViewColumn column in dgv_Events.Columns)
			{
				column.HeaderText = eventTable.Columns[column.Name].Caption;
			}
		}

		private void AddColumn(string name, string header)
		{
			DataColumn dataColumn = new DataColumn(name, typeof(string));
			eventTable.Columns.Add(dataColumn);
			dataColumn.Caption = header;
		}

		private void UpdateEventTable()
		{
			int count = eventTable.Columns.Count;
			bool flag = false;
			string[] array = new string[count];
			bool flag2 = false;
			bool flag3 = false;
			ClearEventTable();
			RemoveAllFilters();
			eventTable.BeginLoadData();
			foreach (ED_V item in ed.edvl)
			{
				WriteTemporaryStatusField(TVStr.Update + " " + Path.GetFileName(item.fileName));
				item.InitCallBack(envVarOvlCallAckDlg);
				int num = ed.edvl.IndexOf(item);
				foreach (ED_V_Data datum in item.data)
				{
					foreach (DataColumn column in eventTable.Columns)
					{
						string columnName = column.ColumnName;
						int ordinal = column.Ordinal;
						array[ordinal] = "";
						if (columnName.Equals("RefNr"))
						{
							array[ordinal] = datum.reference_nr.ToString("D5");
						}
						else if (columnName.Equals("StartTime"))
						{
							if (Settings.Default.StartStopTimeMilliSec)
							{
								array[ordinal] = TDSViewUtil.TimeDate2Str(datum.start_time, datum.start_time.Millisecond);
							}
							else
							{
								array[ordinal] = TDSViewUtil.TimeDate2Str(datum.start_time);
							}
							if (datum.start_time.CompareTo(datum.end_time) > 0)
							{
								flag3 = true;
								if (Settings.Default.showNegativeTimeWarning && !flag2)
								{
									MessageBox.Show(TVStr.StartOlderThanStop, TVStr.ImplausibleData, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
									flag2 = true;
								}
							}
						}
						else if (columnName.Equals("EndTime"))
						{
							if (datum.end_time_sec != uint.MaxValue)
							{
								if (Settings.Default.StartStopTimeMilliSec)
								{
									array[ordinal] = TDSViewUtil.TimeDate2Str(datum.end_time, datum.end_time.Millisecond);
								}
								else
								{
									array[ordinal] = TDSViewUtil.TimeDate2Str(datum.end_time);
								}
							}
						}
						else if (columnName.Equals("Duration"))
						{
							if (datum.end_time_sec != uint.MaxValue)
							{
								if (datum.start_time.CompareTo(datum.end_time) <= 0)
								{
									TimeSpan tSpan = datum.end_time - datum.start_time;
									array[ordinal] = TDSViewUtil.GetSpanString(tSpan, Settings.Default.StartStopTimeMilliSec);
								}
								else
								{
									array[ordinal] = TVStr.Negative;
								}
							}
						}
						else if (columnName.Equals("DistText"))
						{
							if (datum.eventDescr != null)
							{
								array[ordinal] = datum.eventDescr.event_descr;
							}
						}
						else if (columnName.Equals("ErrorCode0"))
						{
							if (Settings.Default.errorcode_as_hex)
							{
								array[ordinal] = datum.errorcode_0.ToString("X4");
							}
							else
							{
								array[ordinal] = datum.errorcode_0.ToString("D4");
							}
						}
						else if (columnName.Equals("ECode0Desc"))
						{
							if (datum.code0 != null)
							{
								array[ordinal] = datum.code0.code_descr;
							}
						}
						else if (columnName.Equals("ECode1Desc"))
						{
							if (datum.code1 != null)
							{
								array[ordinal] = datum.code1.code_descr;
							}
						}
						else if (columnName.Equals("ECode2Desc"))
						{
							if (datum.code2 != null)
							{
								array[ordinal] = datum.code2.code_descr;
							}
						}
						else if (columnName.Equals("ECode3Desc"))
						{
							if (datum.code3 != null)
							{
								array[ordinal] = datum.code3.code_descr;
							}
						}
						else if (columnName.Equals("ErrorCode1"))
						{
							array[ordinal] = datum.errorcode_1.ToString("X4");
						}
						else if (columnName.Equals("ErrorCode2"))
						{
							array[ordinal] = datum.errorcode_2.ToString("X4");
						}
						else if (columnName.Equals("ErrorCode3"))
						{
							array[ordinal] = datum.errorcode_3.ToString("X4");
						}
						else if (columnName.Equals("SignalName"))
						{
							if (datum.eventDescr != null)
							{
								array[ordinal] = datum.eventDescr.event_name;
							}
						}
						else if (columnName.Equals("EnvBlockId"))
						{
							if (datum.eventDescr != null)
							{
								array[ordinal] = datum.eventDescr.env_block_id;
							}
						}
						else if (columnName.Equals("Count"))
						{
							array[ordinal] = datum.event_cnt.ToString("D5");
						}
						else if (columnName.Equals("EventId"))
						{
							array[ordinal] = datum.event_id.ToString("D5");
						}
						else if (columnName.Equals("Limit"))
						{
							array[ordinal] = datum.limit.ToString();
						}
						else if (columnName.Equals("VehiclePos"))
						{
							array[ordinal] = datum.vehicle_pos.ToString();
						}
						else if (columnName.Equals("ProcessId"))
						{
							array[ordinal] = datum.process_id.ToString("D3");
						}
						else if (columnName.Equals("Subsystem"))
						{
							array[ordinal] = item.ed_d.SubSystemName(datum.subsystem_nr);
						}
						else if (columnName.Equals("SubsysDesc"))
						{
							array[ordinal] = item.ed_d.SubSystemDescr(datum.subsystem_nr);
						}
						else if (columnName.Equals("Priority"))
						{
							array[ordinal] = item.ed_d.GetPriorityString(datum.prio);
						}
						else if (columnName.Equals("UniqueRef"))
						{
							array[ordinal] = num + ":" + datum.uniqueRef;
						}
						else if (columnName.Equals("VehicleName"))
						{
							array[ordinal] = item.header.vehicle_name;
						}
						else if (columnName.Equals("User1"))
						{
							bool visible = dgv_Events.Columns["User1"].Visible;
							array[ordinal] = FillUserCol(visible, item, datum, Settings.Default.ColUser1);
						}
						else if (columnName.Equals("User2"))
						{
							bool visible2 = dgv_Events.Columns["User2"].Visible;
							array[ordinal] = FillUserCol(visible2, item, datum, Settings.Default.ColUser2);
						}
						else if (columnName.Equals("User3"))
						{
							bool visible3 = dgv_Events.Columns["User3"].Visible;
							array[ordinal] = FillUserCol(visible3, item, datum, Settings.Default.ColUser3);
						}
						else if (columnName.Equals("User4"))
						{
							bool visible4 = dgv_Events.Columns["User4"].Visible;
							array[ordinal] = FillUserCol(visible4, item, datum, Settings.Default.ColUser4);
						}
						else if (columnName.Equals("User5"))
						{
							bool visible5 = dgv_Events.Columns["User5"].Visible;
							array[ordinal] = FillUserCol(visible5, item, datum, Settings.Default.ColUser5);
						}
						else if (columnName.Equals("Position"))
						{
							if (Settings.Default.takePositionFromEnv)
							{
								string positionVar = GetPositionVar(item, datum, Settings.Default.ColLongitudeHi);
								string positionVar2 = GetPositionVar(item, datum, Settings.Default.ColLongitudeLo);
								string positionVar3 = GetPositionVar(item, datum, Settings.Default.ColLatitudeHi);
								string positionVar4 = GetPositionVar(item, datum, Settings.Default.ColLatitudeLo);
								string text = "";
								string text2 = "";
								bool flag4 = positionVar.Equals("????") || positionVar2.Equals("????") || positionVar3.Equals("????") || positionVar4.Equals("????");
								if (positionVar2 != null && positionVar4 != null && !positionVar2.Equals("") && !positionVar4.Equals("") && !flag4 && Settings.Default.ColLongitudeHi.Equals("COORD_MRVC"))
								{
									float num2 = float.Parse(positionVar4);
									float num3 = float.Parse(positionVar2);
									if (num2 <= 90f && num2 >= -90f && num3 <= 180f && num3 >= -180f)
									{
										array[ordinal] = positionVar4 + "/" + positionVar2;
									}
									else
									{
										array[ordinal] = "";
									}
								}
								else if (positionVar != null && positionVar2 != null && positionVar3 != null && positionVar4 != null && !positionVar.Equals("") && !positionVar2.Equals("") && !positionVar3.Equals("") && !positionVar4.Equals("") && !flag4)
								{
									ushort num4 = ushort.Parse(positionVar, NumberStyles.AllowHexSpecifier);
									ushort num5 = ushort.Parse(positionVar2, NumberStyles.AllowHexSpecifier);
									double num6 = (double)(int)num5 / 60000.0;
									num6 += (double)(num4 & 0xFF);
									if ((num4 & 0xFF00) == 22272)
									{
										text = "-";
									}
									text += num6.ToString("f06");
									num4 = ushort.Parse(positionVar3, NumberStyles.AllowHexSpecifier);
									num5 = ushort.Parse(positionVar4, NumberStyles.AllowHexSpecifier);
									num6 = (double)(int)num5 / 60000.0;
									num6 += (double)(num4 & 0xFF);
									if ((num4 & 0xFF00) == 21248)
									{
										text2 = "-";
									}
									text2 += num6.ToString("f06");
									if (text2.Equals("0") && text.Equals("0"))
									{
										array[ordinal] = "";
									}
									else
									{
										array[ordinal] = text2 + "/" + text;
									}
								}
								else
								{
									array[ordinal] = "";
								}
							}
							else
							{
								array[ordinal] = datum.latitude.ToString("f06") + "/" + datum.longitude.ToString("f06");
							}
						}
						else if (columnName.Equals("Altitude"))
						{
							array[ordinal] = datum.altitude.ToString();
						}
						else if (columnName.Equals("Speed"))
						{
							array[ordinal] = datum.speed.ToString();
						}
						else if (columnName.Equals("Heading"))
						{
							array[ordinal] = datum.heading.ToString();
						}
						else if (columnName.Equals("UTCTime"))
						{
							array[ordinal] = datum.UTC_time.ToString();
						}
						else if (columnName.Equals("Odometer"))
						{
							array[ordinal] = datum.odometer.ToString();
						}
						else if (columnName.Equals("Trip"))
						{
							array[ordinal] = datum.trip.ToString();
						}
						else if (columnName.Equals("rep_prio_active"))
						{
							if (datum.eventDescr != null)
							{
								array[ordinal] = datum.eventDescr.rep_prio_active;
							}
						}
						else if (columnName.Equals("rep_prio_passive"))
						{
							if (datum.eventDescr != null)
							{
								array[ordinal] = datum.eventDescr.rep_prio_passive;
							}
						}
						else if (columnName.Equals("extra_prio"))
						{
							if (datum.eventDescr != null)
							{
								array[ordinal] = datum.eventDescr.extra_prio;
							}
						}
						else if (columnName.Equals("extra_attrib1"))
						{
							if (datum.eventDescr != null)
							{
								array[ordinal] = datum.eventDescr.extra_attrib1;
							}
						}
						else if (columnName.Equals("extra_attrib2"))
						{
							if (datum.eventDescr != null)
							{
								array[ordinal] = datum.eventDescr.extra_attrib2;
							}
						}
						else if (columnName.Equals("extra_attrib3"))
						{
							if (datum.eventDescr != null)
							{
								array[ordinal] = datum.eventDescr.extra_attrib3;
							}
						}
						else if (columnName.Equals("extra_attrib4"))
						{
							if (datum.eventDescr != null)
							{
								array[ordinal] = datum.eventDescr.extra_attrib4;
							}
						}
						else if (columnName.Equals("extra_attrib5"))
						{
							if (datum.eventDescr != null)
							{
								array[ordinal] = datum.eventDescr.extra_attrib5;
							}
						}
						else if (!flag)
						{
							MessageBox.Show(TVStr.UnknownColumnId + columnName + "\n" + TVStr.AskDeveloper, TVStr.FatalError, MessageBoxButtons.OK, MessageBoxIcon.Hand);
							flag = true;
						}
					}
					eventTable.Rows.Add(array);
					if (flag3)
					{
						flag3 = false;
					}
				}
				item.InitCallBack(null);
				envVarShown = false;
			}
			if (doDefaultSort && !Settings.Default.SortColumn.Equals(""))
			{
				doDefaultSort = false;
				foreach (DataGridViewColumn column2 in dgv_Events.Columns)
				{
					if (column2.HeaderText.Equals(Settings.Default.SortColumn))
					{
						ListSortDirection direction = ((!Settings.Default.SortAscending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
						dgv_Events.Sort(dgv_Events.Columns[column2.Name], direction);
						break;
					}
				}
			}
			eventTable.EndLoadData();
		}

		private void dgv_Events_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			_ = e.RowIndex;
			if (e.RowIndex < 0)
			{
				return;
			}
			DataGridViewTextBoxCell dataGridViewTextBoxCell = (DataGridViewTextBoxCell)dgv_Events.Rows[e.RowIndex].Cells["UniqueRef"];
			string text = Convert.ToString(dataGridViewTextBoxCell.Value);
			int num = text.IndexOf(':');
			string value = text.Remove(num);
			int index = Convert.ToInt32(value);
			string value2 = text.Remove(0, num + 1);
			int num2 = Convert.ToInt32(value2);
			dgv_Env.Rows.Clear();
			try
			{
				while (true)
				{
					dgv_Env.Columns.RemoveAt(3);
				}
			}
			catch
			{
			}
			ED_V eD_V = (ED_V)ed.edvl[index];
			bool flag = false;
			foreach (ED_V_Data datum in eD_V.data)
			{
				if (datum.uniqueRef != num2)
				{
					continue;
				}
				ShowStatusStrip(show: true, eD_V);
				int num3 = datum.block_cnt_start + 3;
				string[] array = new string[num3];
				int[] array2 = new int[datum.block_cnt_start];
				int num4 = datum.old_block_idx_start;
				int num5 = 0;
				for (int i = 0; i < datum.block_cnt_start; i++)
				{
					array2[i] = num4;
					if (num4 == datum.trx_block_idx_start)
					{
						num5 = i;
					}
					num4++;
					if (num4 >= datum.block_cnt_start)
					{
						num4 = 0;
					}
				}
				int num6 = 0;
				int num7 = 1;
				if (datum.envBlock != null)
				{
					int num8 = int.Parse(datum.envBlock.cycle_time) / 1000;
					num7 = datum.block_cnt_start;
					for (int j = 1; j <= num7; j++)
					{
						int num9 = (j - num5 - 1) * num8;
						string headerText = num9 + "ms";
						string columnName;
						if (num9 == 0)
						{
							columnName = "ColTrig";
							num6 = j;
						}
						else
						{
							columnName = "ColDataA" + j;
						}
						dgv_Env.Columns.Add(columnName, headerText);
					}
					foreach (ED_D_EnvSignal item in datum.envBlock.envSignal)
					{
						array[0] = item.env_name;
						array[1] = item.env_descr;
						array[2] = item.disp_dim;
						int num10 = 3;
						int[] array3 = array2;
						foreach (int num11 in array3)
						{
							int blkOffset = datum.env_data_cnt_start * num11 * 2;
							array[num10] = eD_V.GetEnvValue(item, blkOffset, datum.envDataArr, valueOnly: true, hexAllowed: true);
							num10++;
						}
						int index2 = dgv_Env.Rows.Add(array);
						item.env_type.Equals("B");
						string text2 = "";
						string text3 = "";
						foreach (DataGridViewCell cell in dgv_Env.Rows[index2].Cells)
						{
							if (cell.ColumnIndex >= 3)
							{
								text2 = cell.Value.ToString();
								if (!text3.Equals(text2) && cell.ColumnIndex >= 4)
								{
									cell.Style.BackColor = Color.Gold;
								}
								text3 = text2;
							}
						}
					}
					flag = true;
				}
				if (datum.envBlockODBSGrp != null)
				{
					foreach (ED_D_EnvSignal item2 in datum.envBlockODBSGrp.envSignal)
					{
						array[0] = item2.env_name;
						array[1] = item2.env_descr;
						array[2] = item2.disp_dim;
						int num12 = 3;
						for (int l = num12; l < num3; l++)
						{
							array[l] = "";
						}
						for (int m = 1; m <= num7; m++)
						{
							if (m == num6)
							{
								array[num12] = eD_V.GetEnvValue(item2, 0, datum.dbs_envDataArr, valueOnly: true, hexAllowed: true);
							}
							else
							{
								array[num12] = "";
							}
							num12++;
						}
						dgv_Env.Rows.Add(array);
					}
					flag = true;
				}
				if (flag)
				{
					break;
				}
			}
			ViewPrePostEnvironmentVariables(tsmnui_AllPrePostEnvironmentVariables.Checked);
		}

		private void ClearEventTable()
		{
			eventTable.Clear();
		}

		private void dgv_Events_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
			if (e.Column.Name == "Count" || e.Column.Name == "EventId" || e.Column.Name == "ProcessId")
			{
				int num = short.Parse(e.CellValue1.ToString());
				int value = short.Parse(e.CellValue2.ToString());
				e.SortResult = num.CompareTo(value);
			}
			else if (e.Column.Name.Contains("User"))
			{
				if (e.CellValue1.ToString().Contains("H") || e.CellValue2.ToString().Contains("H") || e.CellValue1.ToString().Equals("") || e.CellValue2.ToString().Equals(""))
				{
					e.SortResult = string.Compare(e.CellValue1.ToString(), e.CellValue2.ToString());
				}
				else
				{
					try
					{
						double num2 = double.Parse(e.CellValue1.ToString());
						double value2 = double.Parse(e.CellValue2.ToString());
						e.SortResult = num2.CompareTo(value2);
					}
					catch
					{
						string strB = e.CellValue1.ToString();
						string text = e.CellValue2.ToString();
						e.SortResult = text.CompareTo(strB);
					}
				}
			}
			else if (e.Column.Name == "Duration")
			{
				try
				{
					TimeSpan timeSpan = TimeSpan.Parse(e.CellValue1.ToString());
					TimeSpan value3 = TimeSpan.Parse(e.CellValue2.ToString());
					e.SortResult = timeSpan.CompareTo(value3);
				}
				catch
				{
					string strB2 = e.CellValue1.ToString();
					string text2 = e.CellValue2.ToString();
					e.SortResult = text2.CompareTo(strB2);
				}
			}
			else if (e.CellValue1 != null && e.CellValue2 != null)
			{
				e.SortResult = string.Compare(e.CellValue1.ToString(), e.CellValue2.ToString());
			}
			e.Handled = true;
		}

		private void allPrePostEnvironmentVariablesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ViewPrePostEnvironmentVariables(tsmnui_AllPrePostEnvironmentVariables.Checked);
		}

		private void ViewPrePostEnvironmentVariables(bool setIt)
		{
			Settings.Default.prePostEnvVarVisible = setIt;
			tsmnui_AllPrePostEnvironmentVariables.Checked = setIt;
			foreach (DataGridViewColumn column in dgv_Env.Columns)
			{
				if (!(column.Name == "EnvVariableName") && !(column.Name == "EnvUnit") && !(column.Name == "EnvDescr") && !(column.Name == "ColTrig"))
				{
					column.Visible = Settings.Default.prePostEnvVarVisible;
				}
			}
		}

		private bool envVarOvlCallAckDlg(string msg)
		{
			if (!envVarShown)
			{
				msg = msg + " \n" + TVStr.GenerateTextFileOnError;
				MessageBox.Show(msg, TVStr.InconsistentEnvironmentData, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				envVarShown = true;
			}
			return true;
		}

		private bool stsntCallAckDlg(ArrayList tList, bool createDirectOutput)
		{
			STSNTConfigDlg sTSNTConfigDlg = new STSNTConfigDlg(tList, createDirectOutput);
			sTSNTConfigDlg.earliestSelectedTime = earliestSelectedTime;
			sTSNTConfigDlg.latestSelectedTime = latestSelectedTime;
			sTSNTConfigDlg.ShowDialog();
			if (sTSNTConfigDlg.accepted)
			{
				ed.stsntStartTime = sTSNTConfigDlg.selectedStartTime;
				ed.stsntEndTime = sTSNTConfigDlg.selectedEndTime;
			}
			return sTSNTConfigDlg.accepted;
		}

		private void exportToSTSNTToolStripMenuItem_Click(object sender, EventArgs e)
		{
			exportToSTSNT(createDirectOutput: false);
		}

		private void showEnvironmentInSTSNTToolStripMenuItem_Click(object sender, EventArgs e)
		{
			exportToSTSNT(createDirectOutput: true);
		}

		private void exportToSTSNT(bool createDirectOutput)
		{
			bool flag = false;
			bool flag2 = false;
			DateTime dateTime = new DateTime(DateTime.MaxValue.Ticks);
			DateTime dateTime2 = new DateTime(DateTime.MaxValue.Ticks);
			CreateSTSNT createSTSNT = new CreateSTSNT(ed);
			createSTSNT.InitCallBack(stsntCallAckDlg);
			ed.selectionList.Clear();
			int rowCount = dgv_Events.Rows.GetRowCount(DataGridViewElementStates.Selected);
			if (rowCount > 0)
			{
				earliestSelectedTime = new DateTime(DateTime.MaxValue.Ticks);
				latestSelectedTime = new DateTime(DateTime.MinValue.Ticks);
				for (int i = 0; i < rowCount; i++)
				{
					flag = (flag2 = false);
					DataGridViewTextBoxCell dataGridViewTextBoxCell = (DataGridViewTextBoxCell)dgv_Events.SelectedRows[i].Cells["SignalName"];
					string value = dataGridViewTextBoxCell.Value.ToString();
					ed.selectionList.Add(value);
					DataGridViewTextBoxCell dataGridViewTextBoxCell2 = (DataGridViewTextBoxCell)dgv_Events.SelectedRows[i].Cells["StartTime"];
					try
					{
						dateTime2 = TDSViewUtil.Str2TimeDate(dataGridViewTextBoxCell2.Value.ToString());
					}
					catch
					{
						flag = true;
					}
					DataGridViewTextBoxCell dataGridViewTextBoxCell3 = (DataGridViewTextBoxCell)dgv_Events.SelectedRows[i].Cells["EndTime"];
					try
					{
						dateTime = TDSViewUtil.Str2TimeDate(dataGridViewTextBoxCell3.Value.ToString());
					}
					catch
					{
						flag2 = true;
					}
					if (!flag && dateTime2.CompareTo(earliestSelectedTime) < 0)
					{
						earliestSelectedTime = dateTime2;
					}
					if (!flag2 && dateTime.CompareTo(latestSelectedTime) > 0)
					{
						latestSelectedTime = dateTime;
					}
					if (createDirectOutput)
					{
						latestSelectedTime = earliestSelectedTime.AddSeconds(1.0);
					}
				}
			}
			if (!createSTSNT.CreateSTSNTFiles(output_file_path_options, createDirectOutput, Settings.Default.keepGapBetweenEvents))
			{
				return;
			}
			DialogResult dialogResult = DialogResult.No;
			if (!createDirectOutput)
			{
				dialogResult = MessageBox.Show(TVStr.CreationOfSTSNTFiles + " " + createSTSNT.stsntFileNameM + TVStr.StartSTSNT, TVStr.STSNTOutput, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
			}
			if (createDirectOutput || dialogResult == DialogResult.Yes)
			{
				ProcessStartInfo processStartInfo = new ProcessStartInfo();
				processStartInfo.FileName = Settings.Default.pathToSTSNT;
				if (File.Exists(processStartInfo.FileName))
				{
					processStartInfo.Arguments = "\"" + createSTSNT.stsntFileNameM + "\"";
					Process.Start(processStartInfo);
				}
				else
				{
					MessageBox.Show(TVStr.NoSTSNTPath, TVStr.MissingPath, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}

		private void ShowGridViews(bool showIt)
		{
			if (!showIt)
			{
				ClearEventTable();
			}
			splitContainer.Visible = showIt;
			dgv_Events.Visible = showIt;
			dgv_Env.Visible = showIt;
			tsmnui_ExportToSTSNT.Enabled = showIt;
			tsbtn_GeneralInfo.Enabled = showIt;
			tsmnui_GeneralFileInformation.Enabled = showIt;
			tsmnui_FindInEventDescription.Enabled = showIt;
			tsmnui_FindNext.Enabled = showIt;
			tsbtn_Find.Enabled = showIt;
			tsbtn_FindNext.Enabled = showIt;
			tsbtn_STSNT.Enabled = showIt;
			tsmnui_Print.Enabled = showIt;
			tsbtn_Print.Enabled = showIt;
			tsmnui_Close.Enabled = showIt;
			EXCEL_toolStripButton.Enabled = showIt;
			tsmnui_ExportToEXCEL.Enabled = showIt;
			tsmnui_ShowMap.Enabled = showIt;
			tsmnui_ExportToText.Enabled = showIt;
			tsmnui_ShowPathInGoogleEarth.Enabled = showIt;
			tsmnui_ExportToTSV.Enabled = showIt;
			tsmnui_exportToXML.Enabled = showIt;
			tsmnui_Ranking.Enabled = showIt;
		}

		private void SetAlternatingRowColor(bool setIt)
		{
			Settings.Default.alternateRowColor = setIt;
			if (setIt)
			{
				dgv_Events.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;
				dgv_Env.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;
				dgv_RawView.AlternatingRowsDefaultCellStyle.BackColor = Color.Goldenrod;
			}
			else
			{
				dgv_Events.AlternatingRowsDefaultCellStyle.BackColor = Color.Empty;
				dgv_Env.AlternatingRowsDefaultCellStyle.BackColor = Color.Empty;
				dgv_RawView.AlternatingRowsDefaultCellStyle.BackColor = Color.Empty;
			}
		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string[] array = new string[dgv_Events.ColumnCount];
			bool[] array2 = new bool[dgv_Events.ColumnCount];
			bool flag = false;
			int num = 0;
			bool useLocalPcTime = Settings.Default.UseLocalPcTime;
			foreach (DataGridViewColumn column in dgv_Events.Columns)
			{
				array[num] = column.HeaderText;
				if (column.Name.Equals("Position") || column.Name.Equals("Altitude") || column.Name.Equals("Speed") || column.Name.Equals("Heading") || column.Name.Equals("Odometer") || column.Name.Equals("Trip") || column.Name.Equals("UTCTime"))
				{
					string[] array4;
					string[] array3 = (array4 = array);
					int num2 = num;
					IntPtr intPtr = (IntPtr)num2;
					array3[num2] = array4[(long)intPtr] + "         \t(OTI version >= 2.2.0.0)";
				}
				array2[num++] = column.Visible;
			}
			Options options = new Options(array, array2);
			options.ed_d_path = ed_d_path_options;
			options.output_file_path = output_file_path_options;
			options.language = language;
			options.errorCodeAsHex = Settings.Default.errorcode_as_hex;
			options.showNegativeTimeWarning = Settings.Default.showNegativeTimeWarning;
			options.showMissingEDTWarning = Settings.Default.showMissingEDTWarning;
			options.font = Settings.Default.fontGrid;
			options.takePositionFromEnv = Settings.Default.takePositionFromEnv;
			options.longitudeHi = Settings.Default.ColLongitudeHi;
			options.longitudeLo = Settings.Default.ColLongitudeLo;
			options.latitudeHi = Settings.Default.ColLatitudeHi;
			options.latitudeLo = Settings.Default.ColLatitudeLo;
			options.csvFilterSignalName = Settings.Default.CSVFilterSignalName;
			options.ed = ed;
			options.alternateRowColor = Settings.Default.alternateRowColor;
			options.sortColumn = Settings.Default.SortColumn;
			options.sortAscending = Settings.Default.SortAscending;
			options.showTimesWithMilliSec = Settings.Default.StartStopTimeMilliSec;
			options.useLocalPcTime = Settings.Default.UseLocalPcTime;
			options.pathToSTSNT = Settings.Default.pathToSTSNT;
			options.toolLanguage = Settings.Default.ToolLanguage;
			options.timeZoneShift = Settings.Default.TimeZoneShift;
			if (Settings.Default.TimeZoneSel < 0)
			{
				Settings.Default.TimeZoneSel = 0;
			}
			options.selectedTimeZone = Settings.Default.TimeZoneSel;
			options.keepGapBetweenEvents = Settings.Default.keepGapBetweenEvents;
			options.suppressVersionConflictWarning = Settings.Default.suppressVersionConflictWarning;
			options.ShowDialog();
			if (!options.cancel)
			{
				num = 0;
				foreach (DataGridViewColumn column2 in dgv_Events.Columns)
				{
					column2.Visible = options.elements[num++];
				}
				ed_d_path_options = options.ed_d_path;
				output_file_path_options = options.output_file_path;
				if (language.CompareTo(options.language) != 0 || useLocalPcTime != options.useLocalPcTime || Settings.Default.TimeZoneShift != options.timeZoneShift)
				{
					flag = true;
				}
				Settings.Default.language = (language = options.language);
				toolStripStatusLabel_Language.Text = language;
				Settings.Default.errorcode_as_hex = options.errorCodeAsHex;
				Settings.Default.showNegativeTimeWarning = options.showNegativeTimeWarning;
				Settings.Default.showMissingEDTWarning = options.showMissingEDTWarning;
				Settings.Default.fontGrid = options.font;
				Settings.Default.takePositionFromEnv = options.takePositionFromEnv;
				Settings.Default.ColLongitudeHi = options.longitudeHi;
				Settings.Default.ColLongitudeLo = options.longitudeLo;
				Settings.Default.ColLatitudeHi = options.latitudeHi;
				Settings.Default.ColLatitudeLo = options.latitudeLo;
				Settings.Default.alternateRowColor = options.alternateRowColor;
				Settings.Default.CSVFilterSignalName = options.csvFilterSignalName;
				Settings.Default.SortColumn = options.sortColumn;
				Settings.Default.SortAscending = options.sortAscending;
				Settings.Default.StartStopTimeMilliSec = options.showTimesWithMilliSec;
				Settings.Default.UseLocalPcTime = options.useLocalPcTime;
				Settings.Default.pathToSTSNT = options.pathToSTSNT;
				Settings.Default.TimeZoneSel = options.selectedTimeZone;
				Settings.Default.TimeZoneShift = options.timeZoneShift;
				Settings.Default.keepGapBetweenEvents = options.keepGapBetweenEvents;
				Settings.Default.suppressVersionConflictWarning = options.suppressVersionConflictWarning;
				WriteTimeZoneShiftLbl();
				if (!Settings.Default.ToolLanguage.Equals(options.toolLanguage))
				{
					Thread.CurrentThread.CurrentUICulture = new CultureInfo(options.toolLanguage);
					MessageBox.Show(TVStr.NewToolLanguageAdvice, TVStr.NewLanguage);
					Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.ToolLanguage);
					Settings.Default.ToolLanguage = options.toolLanguage;
				}
				SetAlternatingRowColor(Settings.Default.alternateRowColor);
			}
			if (flag)
			{
				MessageBox.Show(TVStr.ActivateLangOrTimeZone, TVStr.ChangeLanguageTimeZone, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else
			{
				doDefaultSort = true;
				UpdateEventTable();
			}
			AutoResizeEventColumns();
			dgv_Events.DefaultCellStyle.Font = Settings.Default.fontGrid;
			dgv_Env.DefaultCellStyle.Font = Settings.Default.fontGrid;
		}

		private void SaveRecentFile(string path)
		{
			mru.Push(path);
			tsmnui_Recent.DropDownItems.Clear();
			FillRecentList();
		}

		private void FillRecentList()
		{
			bool flag = false;
			StringEnumerator enumerator = mru.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					ToolStripMenuItem value = new ToolStripMenuItem(current, null, RecentFile_click);
					tsmnui_Recent.DropDownItems.Add(value);
					flag = true;
				}
			}
			finally
			{
				if (enumerator is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}
			if (flag)
			{
				ToolStripMenuItem value2 = new ToolStripMenuItem(TVStr.ClearRecentList, null, RecentFile_click);
				tsmnui_Recent.DropDownItems.Add(value2);
			}
		}

		private void RecentFile_click(object sender, EventArgs e)
		{
			if (sender.ToString().Contains("..."))
			{
				tsmnui_Recent.DropDownItems.Clear();
				mru.Clear();
			}
			else
			{
				Read_OTI_Files(sender.ToString(), "", addIt: false, batchMode: false, updateTableAfterReading: true);
			}
		}

		private void findInEventDescriptionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bool flag = true;
			FindText findText = new FindText();
			findText.searchInEventDescription = searchInEventDescr;
			findText.searchInSignalName = searchInSignalName;
			findText.searchAll = searchAll;
			findText.searchString = actualSearchString;
			findText.markAll = markAllFindings;
			findText.ShowDialog();
			_ = findText.DialogResult;
			actualSearchString = findText.searchString;
			searchInEventDescr = findText.searchInEventDescription;
			searchInSignalName = findText.searchInSignalName;
			searchAll = findText.searchAll;
			markAllFindings = findText.markAll;
			if (findText.DialogResult == DialogResult.Cancel)
			{
				return;
			}
			if (findText.markAll)
			{
				actualSearchInx = searchText(0, findText.searchString, !findText.keepAndFind);
				if (actualSearchInx >= 0)
				{
					while (actualSearchInx >= 0)
					{
						actualSearchInx = searchText(actualSearchInx, findText.searchString, clearBeforeSelect: false);
					}
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				actualSearchInx = searchText(0, findText.searchString, clearBeforeSelect: true);
				if (actualSearchInx < 0)
				{
					flag = false;
				}
			}
			if (!flag)
			{
				MessageBox.Show(TVStr.StringNotFound, TVStr.FindText, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
		{
			actualSearchInx = searchText(actualSearchInx, actualSearchString, clearBeforeSelect: true);
			if (actualSearchInx < 0)
			{
				MessageBox.Show(TVStr.NoMoreOccurences, TVStr.FindText, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		private int searchText(int startInx, string searchStr, bool clearBeforeSelect)
		{
			bool flag = false;
			bool flag2 = false;
			int result = -1;
			string text = "";
			string text2 = "";
			if (startInx < 0)
			{
				startInx = 0;
			}
			searchStr = searchStr.ToUpperInvariant();
			int i;
			for (i = startInx; i < dgv_Events.Rows.Count; i++)
			{
				if (searchAll)
				{
					foreach (DataGridViewCell cell in dgv_Events.Rows[i].Cells)
					{
						if (cell.Visible)
						{
							flag2 = cell.Value.ToString().ToUpperInvariant().Contains(searchStr);
							if (flag2)
							{
								break;
							}
						}
					}
				}
				else
				{
					if (searchInEventDescr)
					{
						text = dgv_Events.Rows[i].Cells["DistText"].Value.ToString().ToUpperInvariant();
					}
					if (searchInSignalName)
					{
						text2 = dgv_Events.Rows[i].Cells["SignalName"].Value.ToString().ToUpperInvariant();
					}
				}
				if (flag2 || (text.Contains(searchStr) && searchInEventDescr) || (text2.Contains(searchStr) && searchInSignalName))
				{
					i = dgv_Events.Rows[i].Index;
					flag = true;
					break;
				}
			}
			if (flag)
			{
				if (clearBeforeSelect)
				{
					dgv_Events.ClearSelection();
				}
				dgv_Events.Rows[i].Selected = true;
				dgv_Events.FirstDisplayedScrollingRowIndex = i;
				result = i + 1;
			}
			return result;
		}

		private void generalFileInformationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			InformationForm informationForm = new InformationForm(edvRawView);
			if (edvRawView)
			{
				informationForm.edv = evl;
			}
			else
			{
				informationForm.ed = ed;
			}
			informationForm.ShowDialog();
		}

		private void printToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ED_V eD_V = (ED_V)ed.edvl[0];
			string text = "";
			if (ed.edvl.Count > 1)
			{
				text = " " + TVStr.AndOtherEventFiles;
			}
			DialogResult dialogResult = printDialog.ShowDialog();
			if (dialogResult == DialogResult.OK)
			{
				string printTitleStr = "Project: " + eD_V.header.prj_name + " " + TDSViewUtil.StripVersion(eD_V.header.prj_version) + " " + eD_V.header.vehicle_name + text;
				PrintDGV.Print_DataGridView(dgv_Events, printDialog, printTitleStr, eD_V.fileName);
			}
		}

		private void ShowStatusStrip(bool show, ED_V edv)
		{
			if (show && edv != null)
			{
				toolStripStatusLabel1.BorderSides = ToolStripStatusLabelBorderSides.All;
				toolStripStatusLabel2.BorderSides = ToolStripStatusLabelBorderSides.All;
				toolStripStatusLabel3.BorderSides = ToolStripStatusLabelBorderSides.All;
				toolStripStatusLabel4.BorderSides = ToolStripStatusLabelBorderSides.All;
				toolStripStatusLabel1.Text = TVStr.Project + " " + edv.header.prj_name;
				toolStripStatusLabel2.Text = TVStr.Version + " " + TDSViewUtil.StripVersion(edv.header.prj_version);
				toolStripStatusLabel3.Text = TVStr.Vehicle + " " + edv.header.vehicle_name;
				toolStripStatusLabel4.Text = TVStr.OpenedFile + " " + Path.GetFileName(edv.fileName);
			}
			else
			{
				toolStripStatusLabel1.BorderSides = ToolStripStatusLabelBorderSides.None;
				toolStripStatusLabel2.BorderSides = ToolStripStatusLabelBorderSides.None;
				toolStripStatusLabel3.BorderSides = ToolStripStatusLabelBorderSides.None;
				toolStripStatusLabel4.BorderSides = ToolStripStatusLabelBorderSides.None;
				toolStripStatusLabel1.Text = "";
				toolStripStatusLabel2.Text = "";
				toolStripStatusLabel3.Text = "";
				toolStripStatusLabel4.Text = "";
			}
			toolStripStatusLabel5.Text = "";
		}

		private void WriteTemporaryStatusField(string status)
		{
			if (!batch_mode)
			{
				toolStripStatusLabel5.Text = status;
				Update();
				Application.DoEvents();
			}
		}

		private void WriteTimeZoneShiftLbl()
		{
			tsslbl_TimeShift.Text = TVStr.TimeShift + ": " + Settings.Default.TimeZoneShift + " h";
			_ = tsslbl_TimeShift.BackColor;
			if (Settings.Default.TimeZoneShift == 0.0)
			{
				tsslbl_TimeShift.ForeColor = SystemColors.ControlText;
			}
			else
			{
				tsslbl_TimeShift.ForeColor = Color.Red;
			}
		}

		private void repairTextToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!clickedContextCellIsOutOfEventGrid)
			{
				DataGridViewTextBoxCell dataGridViewTextBoxCell = (DataGridViewTextBoxCell)dgv_Events.Rows[clickedContextCell.RowIndex].Cells["SignalName"];
				string signalName = dataGridViewTextBoxCell.Value.ToString();
				ED_V eD_V = (ED_V)ed.edvl[0];
				if (eD_V.ed_t != null)
				{
					string repairText = eD_V.ed_t.GetRepairText(signalName);
					MessageBox.Show(repairText, TVStr.EventRelatedText, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				else
				{
					MessageBox.Show(TVStr.NoRepairText, TVStr.EventRelatedText, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
		}

		private void dataGridViewEvents_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
			{
				return;
			}
			DataGridView.HitTestInfo hitTestInfo = dgv_Events.HitTest(e.X, e.Y);
			if (hitTestInfo.Type != DataGridViewHitTestType.None)
			{
				string name = dgv_Events.Columns[hitTestInfo.ColumnIndex].Name;
				selectedColIsUserCol = name.Equals("User1") || name.Equals("User2") || name.Equals("User3") || name.Equals("User4") || name.Equals("User5");
				selectedColIsFilterable = true;
				selectedColName = "";
				if (selectedColIsFilterable)
				{
					selectedColName = name;
				}
				clickedContextCellIsOutOfEventGrid = false;
				selectedUserColInx = -1;
				if (hitTestInfo.Type == DataGridViewHitTestType.Cell)
				{
					clickedContextCell = dgv_Events.Rows[hitTestInfo.RowIndex].Cells[hitTestInfo.ColumnIndex];
					dgv_Events.ClearSelection();
					dgv_Events.Rows[clickedContextCell.RowIndex].Selected = true;
				}
				else if (hitTestInfo.Type == DataGridViewHitTestType.ColumnHeader)
				{
					selectedUserColInx = hitTestInfo.ColumnIndex;
					clickedContextCellIsOutOfEventGrid = true;
				}
				else
				{
					clickedContextCellIsOutOfEventGrid = true;
				}
			}
		}

		private void contextMenuStrip_Events_Opening(object sender, CancelEventArgs e)
		{
			contextMenuStrip_Events.Items["tsmnui_RemoveUserColumn"].Visible = selectedColIsUserCol;
			contextMenuStrip_Events.Items["tsmnui_Filter"].Visible = selectedColIsFilterable;
			selectedColIsFilterable = false;
			selectedColIsUserCol = false;
		}

		private void dataGridViewEnv_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				UpdateUserColMenu();
				DataGridView.HitTestInfo hitTestInfo = dgv_Env.HitTest(e.X, e.Y);
				clickedContextCellIsOutOfEnvGrid = false;
				if (hitTestInfo.Type == DataGridViewHitTestType.Cell)
				{
					clickedContextEnvCell = dgv_Env.Rows[hitTestInfo.RowIndex].Cells[hitTestInfo.ColumnIndex];
					dgv_Env.ClearSelection();
					dgv_Env.Rows[clickedContextEnvCell.RowIndex].Selected = true;
				}
				else
				{
					clickedContextCellIsOutOfEnvGrid = true;
				}
			}
		}

		private void hideDescriptionColumnToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (dgv_Env.Columns["EnvDescr"].Visible)
			{
				dgv_Env.Columns["EnvDescr"].Visible = false;
				hideDescriptionColumnToolStripMenuItem.Text = TVStr.EnvGridShowDescrCol;
			}
			else
			{
				dgv_Env.Columns["EnvDescr"].Visible = true;
				hideDescriptionColumnToolStripMenuItem.Text = TVStr.EnvGridHideDescrCol;
			}
		}

		private string FillUserCol(bool colVisible, ED_V edvl, ED_V_Data ev, string varName)
		{
			string result = "";
			if (colVisible && ev.envBlock != null)
			{
				bool flag = false;
				int num = 0;
				foreach (ED_D_EnvSignal item in ev.envBlock.envSignal)
				{
					if (item.env_name.Equals(varName))
					{
						flag = true;
						break;
					}
					num++;
				}
				if (flag)
				{
					int trx_block_idx_start = ev.trx_block_idx_start;
					int blkOffset = ev.env_data_cnt_start * trx_block_idx_start * 2;
					ED_D_EnvSignal evs = (ED_D_EnvSignal)ev.envBlock.envSignal[num];
					result = edvl.GetEnvValue(evs, blkOffset, ev.envDataArr, valueOnly: true, hexAllowed: true);
				}
				else if (ev.envBlockODBSGrp != null)
				{
					num = 0;
					foreach (ED_D_EnvSignal item2 in ev.envBlockODBSGrp.envSignal)
					{
						if (item2.env_name.Equals(varName))
						{
							flag = true;
							break;
						}
						num++;
					}
					if (flag)
					{
						int dbs_trx_block_idx_start = ev.dbs_trx_block_idx_start;
						int blkOffset2 = ev.dbs_env_data_cnt_start * dbs_trx_block_idx_start * 2;
						ED_D_EnvSignal evs2 = (ED_D_EnvSignal)ev.envBlockODBSGrp.envSignal[num];
						result = edvl.GetEnvValue(evs2, blkOffset2, ev.dbs_envDataArr, valueOnly: true, hexAllowed: true);
					}
				}
			}
			return result;
		}

		private void removeUserColumnToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (clickedContextCellIsOutOfEventGrid && selectedUserColInx < 0)
			{
				return;
			}
			DataGridViewColumn dataGridViewColumn;
			if (selectedUserColInx >= 0)
			{
				dataGridViewColumn = dgv_Events.Columns[selectedUserColInx];
				selectedUserColInx = -1;
			}
			else
			{
				dataGridViewColumn = dgv_Events.Columns[clickedContextCell.ColumnIndex];
			}
			if (dataGridViewColumn.Name.Contains("User"))
			{
				dataGridViewColumn.Visible = false;
				dataGridViewColumn.HeaderText = dataGridViewColumn.Name;
				if (dataGridViewColumn.Name.Equals("User1"))
				{
					Settings.Default.ColUser1 = "User1";
				}
				if (dataGridViewColumn.Name.Equals("User2"))
				{
					Settings.Default.ColUser2 = "User2";
				}
				if (dataGridViewColumn.Name.Equals("User3"))
				{
					Settings.Default.ColUser3 = "User3";
				}
				if (dataGridViewColumn.Name.Equals("User4"))
				{
					Settings.Default.ColUser4 = "User4";
				}
				if (dataGridViewColumn.Name.Equals("User5"))
				{
					Settings.Default.ColUser5 = "User5";
				}
			}
		}

		private void HandleUserEnvColumn(string colName)
		{
			if (!clickedContextCellIsOutOfEnvGrid)
			{
				DataGridViewTextBoxCell dataGridViewTextBoxCell = (DataGridViewTextBoxCell)dgv_Env.Rows[clickedContextEnvCell.RowIndex].Cells["EnvVariableName"];
				string text = dataGridViewTextBoxCell.Value.ToString();
				dgv_Events.Columns[colName].HeaderText = text;
				dgv_Events.Columns[colName].Visible = true;
				if (colName.Equals("User1"))
				{
					Settings.Default.ColUser1 = text;
				}
				else if (colName.Equals("User2"))
				{
					Settings.Default.ColUser2 = text;
				}
				else if (colName.Equals("User3"))
				{
					Settings.Default.ColUser3 = text;
				}
				else if (colName.Equals("User4"))
				{
					Settings.Default.ColUser4 = text;
				}
				else if (colName.Equals("User5"))
				{
					Settings.Default.ColUser5 = text;
				}
				UpdateEventTable();
				AutoResizeEventColumns();
			}
		}

		private void user1ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			HandleUserEnvColumn("User1");
		}

		private void user2ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			HandleUserEnvColumn("User2");
		}

		private void user3ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			HandleUserEnvColumn("User3");
		}

		private void user4ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			HandleUserEnvColumn("User4");
		}

		private void user5ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			HandleUserEnvColumn("User5");
		}

		private string userMenuText(string itemStr, string settingText)
		{
			string text = itemStr;
			if (!settingText.Contains("User"))
			{
				text = text + ": " + settingText;
			}
			return text;
		}

		private void addToEventColumnToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UpdateUserColMenu();
		}

		private void UpdateUserColMenu()
		{
			tsmnui_User1.Text = userMenuText("User1", Settings.Default.ColUser1);
			tsmnui_User2.Text = userMenuText("User2", Settings.Default.ColUser2);
			tsmnui_User3.Text = userMenuText("User3", Settings.Default.ColUser3);
			tsmnui_User4.Text = userMenuText("User4", Settings.Default.ColUser4);
			tsmnui_User5.Text = userMenuText("User5", Settings.Default.ColUser5);
		}

		private void tsmnui_Filter_Click(object sender, EventArgs e)
		{
			bool isLastFilter = true;
			bool flag = false;
			if (fltStack.Count != 0)
			{
				isLastFilter = fltStack.Peek().Equals(selectedColName);
			}
			if (!fltStack.Contains(selectedColName))
			{
				fltStack.Push(selectedColName);
				isLastFilter = true;
				flag = true;
			}
			Filter filter = new Filter(ref dgv_Events, eventTable, selectedColName, isLastFilter);
			filter.ShowDialog();
			if (filter.cancel)
			{
				if (flag)
				{
					fltStack.Pop();
				}
				return;
			}
			bool flag2 = false;
			if (filter.removedAllFilters)
			{
				dgv_Events.EnableHeadersVisualStyles = true;
				RemoveAllFilters();
			}
			else
			{
				string text = "";
				bool flag3 = true;
				eventTable.Columns[selectedColName].ExtendedProperties.Clear();
				if (filter.allItemsSelected)
				{
					fltStack.Pop();
					dgv_Events.Columns[selectedColName].HeaderCell.Style.BackColor = stdBackGround;
					flag2 = true;
				}
				else
				{
					eventTable.Columns[selectedColName].ExtendedProperties.Add("Expr", filter.filterExpression);
				}
				foreach (DataColumn column in eventTable.Columns)
				{
					if (column.ExtendedProperties.Count > 0 && !column.ExtendedProperties["Expr"].Equals(""))
					{
						if (flag3)
						{
							object obj = text;
							text = string.Concat(obj, "(", column.ExtendedProperties["Expr"], ")");
							flag3 = false;
						}
						else
						{
							object obj2 = text;
							text = string.Concat(obj2, " AND (", column.ExtendedProperties["Expr"], ")");
						}
					}
				}
				eventTable.DefaultView.RowFilter = text;
			}
			if (filter.newLastFilter && !flag2)
			{
				Stack<string> stack = new Stack<string>();
				stack.Push(selectedColName);
				while (fltStack.Count > 0)
				{
					if (fltStack.Peek().Equals(selectedColName))
					{
						fltStack.Pop();
					}
					else
					{
						stack.Push(fltStack.Pop());
					}
				}
				while (stack.Count > 0)
				{
					fltStack.Push(stack.Pop());
				}
			}
			Stack<string> stack2 = new Stack<string>(fltStack.ToArray());
			Stack<string> stack3 = new Stack<string>(stack2.ToArray());
			bool flag4 = true;
			while (stack3.Count > 0)
			{
				string columnName = stack3.Pop();
				Color backColor = Color.Gold;
				if (flag4)
				{
					dgv_Events.EnableHeadersVisualStyles = false;
					backColor = Color.LightGreen;
					flag4 = false;
				}
				dgv_Events.Columns[columnName].HeaderCell.Style.BackColor = backColor;
			}
		}

		private void RemoveAllFilters()
		{
			fltStack.Clear();
			eventTable.DefaultView.RowFilter = "";
			foreach (DataColumn column in eventTable.Columns)
			{
				column.ExtendedProperties.Clear();
			}
			fltStack.Clear();
			foreach (DataGridViewColumn column2 in dgv_Events.Columns)
			{
				column2.HeaderCell.Style.BackColor = stdBackGround;
			}
			dgv_Events.EnableHeadersVisualStyles = true;
		}

		private void savePackageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			saveFileDialogPackage.Title = TVStr.SaveOpenedEventFilesAsPackage;
			saveFileDialogPackage.Filter = TVStr.TDSViewPackageFiles + " (*.tvp)|*.tvp";
			if (saveFileDialogPackage.ShowDialog() == DialogResult.OK)
			{
				ed.SavePackage(saveFileDialogPackage.FileName);
			}
		}

		private void openPackageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			openFileDialogPackage.Title = TVStr.OpenEventFilePackage;
			openFileDialogPackage.Filter = TVStr.TDSViewPackageFiles + " (*.tvp)|*.tvp";
			if (openFileDialogPackage.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			TDSV_RetVal tDSV_RetVal = ed.OpenPackage(openFileDialogPackage.FileName);
			if (tDSV_RetVal.rV == TDSV_RetVal.ReturnValue.OK)
			{
				bool addIt = false;
				foreach (ed_package package in ed.packageList)
				{
					Read_OTI_Files(package.ed_v_file, package.ed_d_file, addIt, batchMode: false, updateTableAfterReading: false);
					addIt = true;
				}
				UpdateEventTable();
				AutoResizeEventColumns();
			}
			else
			{
				MessageBox.Show(TVStr.FileInPackageUnknown + " " + tDSV_RetVal.str, TVStr.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void exportToEXCELToolStripMenuItem_Click(object sender, EventArgs e)
		{
			WriteTemporaryStatusField(TVStr.ExportToExcel);
			ED_V eD_V = (ED_V)ed.edvl[0];
			string text = "";
			if (ed.edvl.Count > 1)
			{
				text = "_AndOthers";
			}
			string text2 = Path.GetExtension(eD_V.fileName).Substring(1);
			if (output_file_path_options.Equals(""))
			{
				saveFileDialog.FileName = Path.GetDirectoryName(eD_V.fileName) + "\\" + Path.GetFileNameWithoutExtension(eD_V.fileName) + "_" + text2 + text;
			}
			else
			{
				saveFileDialog.FileName = output_file_path_options + "\\" + Path.GetFileNameWithoutExtension(eD_V.fileName) + "_" + text2 + text;
			}
			saveFileDialog.Title = TVStr.SaveEventsAsEXCEL;
			saveFileDialog.Filter = TVStr.ExcelFiles + " (*.xlsx)|*.xlsx";
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				base.UseWaitCursor = true;
				CreateExcel.CreateExcelFile(saveFileDialog.FileName, dgv_Events);
				base.UseWaitCursor = false;
				MessageBox.Show(TVStr.ExportToExcel + " " + saveFileDialog.FileName + " " + TVStr.Done, TVStr.ExportToExcel, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			WriteTemporaryStatusField("");
		}

		private void exportToTextToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CreateText createText = new CreateText(ed, xml: false, givenName: false, language);
			string outfileName = createText.GetOutfileName(((ED_V)ed.edvl[0]).fileName);
			createText.CreateTextFile(outfileName);
			MessageBox.Show(TVStr.TextFileExportTo + " " + outfileName + " " + TVStr.Done, TVStr.ExportToText, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		private void exportToXMLToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CreateText createText = new CreateText(ed, xml: true, givenName: false, language);
			string outfileName = createText.GetOutfileName(((ED_V)ed.edvl[0]).fileName);
			createText.CreateTextFile(outfileName);
			outfileName = Path.ChangeExtension(outfileName, ".XML");
			MessageBox.Show(TVStr.TextFileExportTo + " " + outfileName + " " + TVStr.Done, TVStr.ExportToText, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		private void exportToCSVToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CreateCsv createCsv = new CreateCsv(ed, language);
			string outfileName = createCsv.GetOutfileName(((ED_V)ed.edvl[0]).fileName);
			int num = createCsv.CreateTextFile(outfileName, Settings.Default.CSVFilterSignalName);
			if (num == 9)
			{
				MessageBox.Show(TVStr.NoEventsMatchesToSetting, TVStr.ExportToCSV, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			MessageBox.Show(TVStr.TSVFileExportTo + " " + outfileName + " " + TVStr.Done, TVStr.ExportToCSV, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		private void MainForm_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop, autoConvert: false))
			{
				e.Effect = DragDropEffects.Link;
			}
		}

		private void MainForm_DragDrop(object sender, DragEventArgs e)
		{
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			bool addIt = false;
			string[] array2 = array;
			foreach (string text in array2)
			{
				WriteTemporaryStatusField(TVStr.Read + " " + Path.GetFileName(text));
				Read_OTI_Files(text, "", addIt, batchMode: false, updateTableAfterReading: false);
				addIt = true;
			}
			UpdateEventTable();
			AutoResizeEventColumns();
			WriteTemporaryStatusField("");
		}

		private string GetPositionVar(ED_V edvl, ED_V_Data ev, string varName)
		{
			bool flag = false;
			int num = 0;
			string result = "";
			if (ev.envBlock != null)
			{
				foreach (ED_D_EnvSignal item in ev.envBlock.envSignal)
				{
					if (item.env_name.Contains(varName))
					{
						flag = true;
						break;
					}
					num++;
				}
				if (flag)
				{
					int trx_block_idx_start = ev.trx_block_idx_start;
					int blkOffset = ev.env_data_cnt_start * trx_block_idx_start * 2;
					ED_D_EnvSignal evs = (ED_D_EnvSignal)ev.envBlock.envSignal[num];
					result = edvl.GetEnvValue(evs, blkOffset, ev.envDataArr, valueOnly: true, hexAllowed: true);
				}
				else if (ev.envBlockODBSGrp != null)
				{
					num = 0;
					foreach (ED_D_EnvSignal item2 in ev.envBlockODBSGrp.envSignal)
					{
						if (item2.env_name.Contains(varName))
						{
							flag = true;
							break;
						}
						num++;
					}
					if (flag)
					{
						int dbs_trx_block_idx_start = ev.dbs_trx_block_idx_start;
						int blkOffset2 = ev.dbs_env_data_cnt_start * dbs_trx_block_idx_start * 2;
						ED_D_EnvSignal evs2 = (ED_D_EnvSignal)ev.envBlockODBSGrp.envSignal[num];
						result = edvl.GetEnvValue(evs2, blkOffset2, ev.dbs_envDataArr, valueOnly: true, hexAllowed: true);
					}
				}
			}
			return result;
		}

		private void showMapToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			bool flag = true;
			int rowCount = dgv_Events.Rows.GetRowCount(DataGridViewElementStates.Selected);
			if (rowCount > 0)
			{
				DataGridViewTextBoxCell dataGridViewTextBoxCell = (DataGridViewTextBoxCell)dgv_Events.SelectedRows[0].Cells["Position"];
				text3 = dataGridViewTextBoxCell.Value.ToString();
				DataGridViewTextBoxCell dataGridViewTextBoxCell2 = (DataGridViewTextBoxCell)dgv_Events.SelectedRows[0].Cells["DistText"];
				string text4 = dataGridViewTextBoxCell2.Value.ToString();
				text4 = text4.Replace(" ", "+");
				text4 = text4.Replace("(", "{");
				text4 = text4.Replace(")", "}");
				char[] separator = new char[1] { '/' };
				string[] array = text3.Split(separator);
				if (array.Length == 2)
				{
					flag = false;
					text = array[0];
					text2 = array[1];
					if (browser == null || !browser.isOpen)
					{
						browser = new IBrowser("http://maps.google.ch/maps?q=" + text + ",+" + text2 + " %28" + text4 + "+%29&iwloc=A&hl=en", TVStr.TDSViewPositionTracker, canBeMaximized: true);
						browser.Show();
					}
					else
					{
						browser.NavigateTo("http://maps.google.ch/maps?q=" + text + ",+" + text2 + " %28" + text4 + "+%29&iwloc=A&hl=en");
					}
				}
			}
			if (flag)
			{
				MessageBox.Show(TVStr.NoValidGPSPosition, TVStr.EventMap, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void showPathInGoogleEarthToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CreateMap createMap = new CreateMap();
			for (int i = 0; i < dgv_Events.Rows.Count; i++)
			{
				DataGridViewTextBoxCell dataGridViewTextBoxCell = (DataGridViewTextBoxCell)dgv_Events.Rows[i].Cells["Position"];
				string value = dataGridViewTextBoxCell.Value.ToString();
				createMap.coordinate.Add(value);
			}
			string text = "TDSViewPfad.kml";
			IEnumerator enumerator = ed.edvl.GetEnumerator();
			try
			{
				if (enumerator.MoveNext())
				{
					ED_V eD_V = (ED_V)enumerator.Current;
					string directoryName = Path.GetDirectoryName(eD_V.fileName);
					text = Path.Combine(directoryName, text);
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
			string pathDescription = "Source: " + Text;
			createMap.CreateKMLFile(text, pathDescription);
			if (googleEarth != null && !googleEarth.HasExited)
			{
				googleEarth.Kill();
			}
			googleEarth = new Process();
			googleEarth.StartInfo.FileName = text;
			googleEarth.Start();
		}

		private void contentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Help.ShowHelp(this, "TDSView.chm");
		}

		private void indexToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Help.ShowHelpIndex(this, "TDSView.chm");
		}

		private void aboutTDSViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutBox aboutBox = new AboutBox();
			aboutBox.Show();
		}

		private void updateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (browser == null || !browser.isOpen)
			{
				browser = new IBrowser("http://www.boss-ard.ch/btprogs/tdsview.htm", TVStr.Update, canBeMaximized: false);
				browser.Show();
			}
		}

		private void rankingListToolStripMenuItem_Click(object sender, EventArgs e)
		{
			rList = new RankingList(ed, Settings.Default.alternateRowColor);
			rList.Show();
		}

		private void CloseRankingList()
		{
			if (rList != null)
			{
				rList.Close();
				rList = null;
			}
		}

		private void openInTreeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenTree openTree = new OpenTree(Settings.Default.edvOverviewRoot, Settings.Default.edvOverviewTreeView);
			openTree.edvSelectionUpdated += oT_edvSelectionUpdated;
			openTree.OptionsUpdated += oT_optionsUpdated;
			openTree.ShowTree();
			openTree.FormClosed += oT_FormClosed;
			openTree.Show();
			openInTreeToolStripMenuItem.Enabled = false;
		}

		private void oT_FormClosed(object sender, EventArgs e)
		{
			openInTreeToolStripMenuItem.Enabled = true;
		}

		private void oT_edvSelectionUpdated(object sender, SelectedEDVFileEventArgs e)
		{
			int num = 0;
			if (e == null || e.Name[0] == null)
			{
				return;
			}
			new Stopwatch();
			base.TopMost = true;
			Update();
			Application.DoEvents();
			base.TopMost = false;
			for (int i = 0; i < e.Name.Length; i++)
			{
				WriteTemporaryStatusField(TVStr.Read + " " + Path.GetFileName(e.Name[i]));
				if (Read_OTI_Files(e.Name[i], "", i != 0 || e.addIt, batchMode: false, updateTableAfterReading: false) != 0)
				{
					break;
				}
			}
			UpdateEventTable();
			AutoResizeEventColumns();
		}

		private void oT_optionsUpdated(object sender, OptionsEventArgs e)
		{
			if (e != null && e.rootDir != null)
			{
				Settings.Default.edvOverviewRoot = e.rootDir;
				Settings.Default.edvOverviewTreeView = e.treeSel;
			}
		}

		private void SetElementText()
		{
			configurationsToolStripMenuItem.Text = TVStr.configurationsToolStripMenuItem_Text;
			EnvDescr.HeaderText = TVStr.EnvDescr_HeaderText;
			EnvUnit.HeaderText = TVStr.EnvUnit_HeaderText;
			EnvVariableName.HeaderText = TVStr.EnvVariableName_HeaderText;
			EXCEL_toolStripButton.ToolTipText = TVStr.EXCEL_toolStripButton_ToolTipText;
			hideDescriptionColumnToolStripMenuItem.Text = TVStr.hideDescriptionColumnToolStripMenuItem_Text;
			loadConfigurationDialog.Filter = TVStr.loadConfigurationDialog_Filter;
			open_ED_D_FileDialog.Filter = TVStr.open_ED_D_FileDialog_Filter;
			open_ED_D_FileDialog.Title = TVStr.open_ED_D_FileDialog_Title;
			openEDVForRawDataViewToolStripMenuItem.Text = TVStr.openEDVForRawDataViewToolStripMenuItem_Text;
			openFileDialog_ED_V.Filter = TVStr.openFileDialog_ED_V_Filter;
			openFileDialogRawView.Filter = TVStr.openFileDialogRawView_Filter;
			openInTreeToolStripMenuItem.Text = TVStr.openInTreeToolStripMenuItem_Text;
			saveConfigurationDialog.Filter = TVStr.saveConfigurationDialog_Filter;
			tsbtn_Find.ToolTipText = TVStr.tsbtn_Find_ToolTipText;
			tsbtn_FindNext.ToolTipText = TVStr.tsbtn_FindNext_ToolTipText;
			tsbtn_GeneralInfo.ToolTipText = TVStr.tsbtn_GeneralInfo_ToolTipText;
			tsbtn_Open_ED_V.ToolTipText = TVStr.tsbtn_Open_ED_V_ToolTipText;
			tsbtn_Print.ToolTipText = TVStr.tsbtn_Print_ToolTipText;
			tsbtn_STSNT.ToolTipText = TVStr.tsbtn_STSNT_ToolTipText;
			tsmnui_AboutTDSView.Text = TVStr.tsmnui_AboutTDSView_Text;
			tsmnui_AddEDVFile.Text = TVStr.tsmnui_AddEDVFile_Text;
			tsmnui_AddToEventColumn.Text = TVStr.tsmnui_AddToEventColumn_Text;
			tsmnui_AllPrePostEnvironmentVariables.Text = TVStr.tsmnui_AllPrePostEnvironmentVariables_Text;
			tsmnui_Close.Text = TVStr.tsmnui_Close_Text;
			tsmnui_Exit.Text = TVStr.tsmnui_Exit_Text;
			tsmnui_ExportToEXCEL.Text = TVStr.tsmnui_ExportToEXCEL_Text;
			tsmnui_ExportToSTSNT.Text = TVStr.tsmnui_ExportToSTSNT_Text;
			tsmnui_ExportToText.Text = TVStr.tsmnui_ExportToText_Text;
			tsmnui_ExportToTSV.Text = TVStr.tsmnui_ExportToTSV_Text;
			tsmnui_exportToXML.Text = TVStr.tsmnui_exportToXML_Text;
			tsmnui_File.Text = TVStr.tsmnui_File_Text;
			tsmnui_Filter.Text = TVStr.tsmnui_Filter_Text;
			tsmnui_FindInEventDescription.Text = TVStr.tsmnui_FindInEventDescription_Text;
			tsmnui_FindNext.Text = TVStr.tsmnui_FindNext_Text;
			tsmnui_GeneralFileInformation.Text = TVStr.tsmnui_GeneralFileInformation_Text;
			tsmnui_getSettings.Text = TVStr.tsmnui_getSettings_Text;
			tsmnui_Help.Text = TVStr.tsmnui_Help_Text;
			tsmnui_HelpContent.Text = TVStr.tsmnui_HelpContent_Text;
			tsmnui_HelpIndex.Text = TVStr.tsmnui_HelpIndex_Text;
			tsmnui_OpenEDVFile.Text = TVStr.tsmnui_OpenEDVFile_Text;
			tsmnui_OpenPackage.Text = TVStr.tsmnui_OpenPackage_Text;
			tsmnui_Options.Text = TVStr.tsmnui_Options_Text;
			tsmnui_Print.Text = TVStr.tsmnui_Print_Text;
			tsmnui_Ranking.Text = TVStr.tsmnui_Ranking_Text;
			tsmnui_Recent.Text = TVStr.tsmnui_Recent_Text;
			tsmnui_RemoveUserColumn.Text = TVStr.tsmnui_RemoveUserColumn_Text;
			tsmnui_RemoveUserColumn.ToolTipText = TVStr.tsmnui_RemoveUserColumn_ToolTipText;
			tsmnui_resetSettingsToDefault.Text = TVStr.tsmnui_resetSettingsToDefault_Text;
			tsmnui_SavePackage.Text = TVStr.tsmnui_SavePackage_Text;
			tsmnui_saveSettings.Text = TVStr.tsmnui_saveSettings_Text;
			tsmnui_ShowEnvironmentInSTSNT.Text = TVStr.tsmnui_ShowEnvironmentInSTSNT_Text;
			tsmnui_ShowEnvironmentInSTSNT.ToolTipText = TVStr.tsmnui_ShowEnvironmentInSTSNT_ToolTipText;
			tsmnui_ShowMap.Text = TVStr.tsmnui_ShowMap_Text;
			tsmnui_ShowMap1.Text = TVStr.tsmnui_ShowMap1_Text;
			tsmnui_ShowMap1.ToolTipText = TVStr.tsmnui_ShowMap1_ToolTipText;
			tsmnui_ShowPathInGoogleEarth.Text = TVStr.tsmnui_ShowPathInGoogleEarth_Text;
			tsmnui_ShowRepairAndOtherText.Text = TVStr.tsmnui_ShowRepairAndOtherText_Text;
			tsmnui_Tools.Text = TVStr.tsmnui_Tools_Text;
			tsmnui_User1.Text = TVStr.tsmnui_User1_Text;
			tsmnui_User2.Text = TVStr.tsmnui_User2_Text;
			tsmnui_User3.Text = TVStr.tsmnui_User3_Text;
			tsmnui_User4.Text = TVStr.tsmnui_User4_Text;
			tsmnui_User5.Text = TVStr.tsmnui_User5_Text;
			tsmnui_View.Text = TVStr.tsmnui_View_Text;
			tsmnui_Update.Text = TVStr.Update;
		}
	}
}
