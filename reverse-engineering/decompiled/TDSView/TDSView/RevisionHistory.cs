using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TDSView
{
	public class RevisionHistory : Form
	{
		private const int empty = 0;

		private const int newF = 1;

		private const int corr = 2;

		private const int head = 3;

		private const int todo = 4;

		private IContainer components;

		private DataGridView dgv_RevHistory;

		private DataGridViewTextBoxColumn Version;

		private DataGridViewTextBoxColumn Type;

		private DataGridViewTextBoxColumn Description;

		public RevisionHistory()
		{
			InitializeComponent();
			History();
		}

		private void History()
		{
			AddEntry(3, "2.0.0.0", "25.04.2014");
			AddEntry(1, "", "Installation package and product name changed consistent from TDSViewer to TDSView");
			AddEntry(1, "", "Add new attributes:\n- rep_prio_active\n- rep_prio_passive\n- extra_prio\n- extra_attrib1\n- extra_attrib2\n- extra_attrib3\n- extra_attrib4\n- extra_attrib5");
			AddEntry(2, "", "Correct handling of read ED_V files with a file length < ED_V-Header length");
			AddEntry(3, "1.5.7.0", "03.04.2014");
			AddEntry(1, "", "TDSView version update via Web");
			AddEntry(2, "", "Handle TSV and TXT output correctly in case of missing ODBS Env Group");
			AddEntry(3, "1.5.6.0", "31.03.2014");
			AddEntry(1, "", "Restore the windows size to the non maximized size");
			AddEntry(1, "", "Open ED_V in raw view, i.e. no ED_D file is required");
			AddEntry(1, "", "Export as tab separated raw file (/RAW option)");
			AddEntry(1, "", "Some updates for multi language features");
			AddEntry(3, "1.5.5.0", "19.03.2014");
			AddEntry(1, "", "Allow decimal representation of double types");
			AddEntry(1, "", "Support MRVC GPS position format (two 16-Bit-Analog values)");
			AddEntry(1, "", "Save the window width and height and the splitter position between Events and Env Data");
			AddEntry(1, "", "Description column of environment data can be hided by context menu");
			AddEntry(2, "", "Global environment data also available for  user columns");
			AddEntry(2, "", "Select the correct ODBS environment data group (if more than one is defined)");
			AddEntry(2, "", "Add the ODBS environment data group signals also to TSV and TXT output files");
			AddEntry(3, "1.5.4.0", "27.02.2014");
			AddEntry(2, "", "Modify status texts in bottom bar");
			AddEntry(2, "", "Improve the performance of auto-column-width");
			AddEntry(2, "", "Handle incompatibility between ED_V and ED_D version related to the ODBS Environment Group");
			AddEntry(3, "1.5.3.0", "18.02.2014");
			AddEntry(1, "", "Using new data structure to increase the filter performance");
			AddEntry(2, "", "Handle right mouse click out of data grid");
			AddEntry(2, "", "Removed unused text label in Find Text dialog ");
			AddEntry(3, "1.5.2.0", "07.02.2014");
			AddEntry(1, "", "Add filter for event table");
			AddEntry(2, "", "Google Earth Map: Use '.' as decimal separation character for coords independent of Windows Regional Setting");
			AddEntry(2, "", "Update of context menu (User column) in environment window corrected");
			AddEntry(3, "1.5.1.0", "15.01.2014");
			AddEntry(1, "", "Other background color of environment data cells that changes the value compared to the previous one");
			AddEntry(2, "", "Google Earth Map: Save .KML path file in the directory of the first ED_V file instead of installation directory (Windows 7, 8");
			AddEntry(2, "", "Tree overview: Break multiple read in case of failed read operation (e.g. OTI not found and not selected)");
			AddEntry(2, "", "TextOutput and CsvOutput: Write empty string in case of no event end time");
			AddEntry(2, "", "Improve multiple ed_v file read performance by updating event grid at the very end");
			AddEntry(2, "", "Add field for temporary status information like \"Read ED_V in progress\"");
			AddEntry(3, "1.5.0.0", "17.12.2013");
			AddEntry(2, "", "Initialization at startup of user column context menu ");
			AddEntry(2, "", "Add ODBS environment group in case of ED_V protocol 2.2.0.0");
			AddEntry(3, "1.4.12.0", "10.12.2013");
			AddEntry(1, "", "In case of missing OTI: Show available languages and let select them");
			AddEntry(1, "", "Tree overview: Show only ed_v-path relative to given root path");
			AddEntry(1, "", "EXCEL Export: If more than one row is selected, only the selected rows will be exported");
			AddEntry(3, "1.4.11.0", "06.12.2013");
			AddEntry(1, "", "Modified commandline interface");
			AddEntry(1, "", "Text output encoding according selected event languange");
			AddEntry(1, "", "TSV output: Add \"Duration\" of the event");
			AddEntry(1, "", "Add TSV2EXCEL.EXE to the distribution");
			AddEntry(3, "1.4.10.1", "13.11.2013");
			AddEntry(1, "", "Add $ALL_ENV$ option to TSV creation");
			AddEntry(3, "1.4.10.0", "12.11.2013");
			AddEntry(2, "", "Formatting and performance for EXCEL output improved");
			AddEntry(2, "", "Time Format changed: YYYY-MM-DD (instead of DD-MM-YYYY): Better performance for sorting)");
			AddEntry(2, "", "Modification in About dialog (Credits)");
			AddEntry(2, "", "Remove Column Header Sort Glyph when load a new event file");
			AddEntry(2, "", "Mismatch between ED_V and ED_D warning only if ED_D version is lower than ED_V version");
			AddEntry(1, "", "Option to suppress the version mismatch between ED_V and ED_D warning");
			AddEntry(3, "1.4.9.0", "30.10.2013");
			AddEntry(1, "", "Add ED_V files overview");
			AddEntry(3, "1.4.2.0", "16.08.2013");
			AddEntry(2, "", "Bug fix to support OTI 2.1.0.0 (version 2.1.0.0 supports only 2 EXTRA_ATTRIB attributes).");
			AddEntry(2, "", "Use \"xslx\" as default EXCEL export extension.");
			AddEntry(3, "1.4.1.0", "14.06.2013");
			AddEntry(1, "", "Export to XML: Exchange special characters like <, >, etc. by XML format &lt, &gt etc.");
			AddEntry(3, "1.4.0.0", "26.04.2013");
			AddEntry(1, "", "Export as XML file; For Batch mode add \"_X\" behind the languange");
			AddEntry(3, "1.3.4.0", "11.02.2013");
			AddEntry(2, "", "bugfix: Environment data type F now correct output");
			AddEntry(3, "1.3.3.0", "08.02.2013");
			AddEntry(1, "", "Ignore difference between ED_D and ED_V files version in batch mode");
			AddEntry(3, "1.3.2.0", "05.02.2013");
			AddEntry(1, "", "Output Format TSV: If signal filter text is $ALL$, all events will be printed without environment data");
			AddEntry(1, "", "Output Format TSV: Add columns for vehicle _name and project_version");
			AddEntry(1, "", "Support of environment data type F; Display format is x.xx");
			AddEntry(3, "1.3.1.0", "21.01.2013");
			AddEntry(2, "", "Allow mismatch between different ED_D and ED_V files version (ED_D.cs)");
			AddEntry(3, "1.3.0.0", "3.12.2012");
			AddEntry(1, "", "Save, load and reset settings");
			AddEntry(1, "", "Find in all columns possible");
			AddEntry(3, "1.2.0.0", "08.11.2012");
			AddEntry(1, "", "New Option: Keep the values of environment variables between events in STSNT output");
			AddEntry(3, "1.1.0.0", "17.09.2012");
			AddEntry(1, "", "Add the feature to select a time shift");
			AddEntry(3, "1.0.1.0", "22.08.2012");
			AddEntry(2, "", "Close the ranking list window if main window changes the content");
			AddEntry(2, "", "Remove allocated ED_V data structure in case of invalid ED_D file");
			AddEntry(3, "1.0.0.0", "22.06.2012");
			AddEntry(1, "", "Show selected project language in status strip");
			AddEntry(1, "", "Allow to clear the recent list");
			AddEntry(1, "", "Add ranking list");
			AddEntry(2, "", "Prohibit unwanted change of project language in case of options change");
			AddEntry(2, "", "Remove user column also if header line is double clicked");
			AddEntry(2, "", "Show \"Remove User Column\" only if a user column is selected");
			AddEntry(2, "", "Handle sorting of user columns corrected");
			AddEntry(3, "0.9.6.0", "11.06.2012");
			AddEntry(1, "", "Support Chinese as tool language");
			AddEntry(1, "", "Ask for search ED_D file if not found");
			AddEntry(1, "", "Add a Recent List");
			AddEntry(2, "", "Correct representation of milliseconds in start and stop time column");
			AddEntry(2, "", "Don't tell 'no events' if ED_D reading is canceled");
			AddEntry(3, "0.9.5.0", "06.06.2012");
			AddEntry(1, "", "Support German as tool language");
			AddEntry(3, "0.9.4.0", "04.06.2012");
			AddEntry(2, "", "Correct the representation of the trailing 0's in duration column");
			AddEntry(1, "", "Start STSNT if requested; Define the path to STSNT in Options dialog");
			AddEntry(1, "", "Add context related call of STSNT");
			AddEntry(3, "0.9.3.1", "11.05.2012");
			AddEntry(1, "", "Allow to display environment datatype \"byte\" as decimal signed number (support now DISP_TYPE O, M and H)");
			AddEntry(3, "0.9.3.0", "09.05.2012");
			AddEntry(1, "", "Allow to display environment datatype \"byte\" as decimal unsigned number");
			AddEntry(3, "0.9.2.0", "26.04.2012");
			AddEntry(2, "", "STSNT output improved to handle environment cycles of 1 ms");
			AddEntry(3, "0.9.1.0", "08.03.2012");
			AddEntry(1, "", "New option: Use PC Time Zone Setting");
			AddEntry(2, "", "Google Maps call parameters adjusted for Show Map");
			AddEntry(2, "", "Error Message if no valid call of browser in Show Map");
			AddEntry(3, "0.9.0.0", "01.02.2012");
			AddEntry(1, "", "Support OTI format 2.3.0.0");
			AddEntry(1, "", "Support event file names (standard ED_V and TWCS-like)");
			AddEntry(2, "", "Allow continuation in case of incompatible ED_V and ED_D version");
			AddEntry(3, "0.8.4.0", "3.10.2011");
			AddEntry(1, "", "Option to show start and stop time with milli seconds");
			AddEntry(3, "0.8.3.0", "17.07.2011");
			AddEntry(1, "", "Command line parameter to open event files directly:\n\"/OP <ED_V_file> <ED_D_file> ");
			AddEntry(2, "", "New About logo");
			AddEntry(3, "0.8.2.0", "17.06.2011");
			AddEntry(2, "", "Suppress duplicated environment data in case of more than one opened event files");
			AddEntry(1, "", "Allow to define a default sort column in the options dialog");
			AddEntry(3, "0.8.1.0", "30.05.2011");
			AddEntry(2, "", "Create message in case of inconsistent environment data");
			AddEntry(3, "0.8.0.1", "26.05.2011");
			AddEntry(2, "", "Handle GPS-position in case of invalid environment data");
			AddEntry(3, "0.8.0.0", "03.12.2010");
			AddEntry(1, "", "Create tab separated text file (TSV) with selectable event");
			AddEntry(2, "", "Replace \"Signal Name\" by \"Event Name\"");
			AddEntry(3, "0.7.3.0", "18.10.2010");
			AddEntry(2, "", "Allow multi select of environment data");
			AddEntry(3, "0.7.2.0", "13.08.2010");
			AddEntry(2, "", "Handle the analog format independent of culture decimal point setting");
			AddEntry(2, "", "Behavior of ED_D path in Option menu changed");
			AddEntry(3, "0.7.1.0", "09.08.2010");
			AddEntry(2, "", "Handle Errorcode Null-Pointer in CreateText");
			AddEntry(3, "0.7.0.0", "06.07.2010");
			AddEntry(1, "", "Add Error Code 0..3 Name and Description");
			AddEntry(1, "", "Add environment data display parameters to text output");
			AddEntry(1, "", "Add $SELECT tag to STSNT output (all events will be selected in STSNT by default)");
			AddEntry(3, "0.6.1.0", "15.06.2010");
			AddEntry(2, "", "Fix inconsistent environment data description");
			AddEntry(1, "", "Find text possible with multi event selection");
			AddEntry(3, "0.6.0.0", "27.04.2010");
			AddEntry(2, "", "Small modifications");
			AddEntry(3, "0.5.0.0", "07.04.2010");
			AddEntry(1, "", "Help added");
			AddEntry(3, "0.4.0.1", "23.03.2010");
			AddEntry(2, "", "Null pointer handled in text output");
			AddEntry(2, "", "Corrected some small issues in text file creation");
			AddEntry(3, "0.4.0.0", "21.03.2010");
			AddEntry(1, "", "Support OTI Format 2.2.0.0 (prov. solution until valid 2.2.0.0-OTI is available to test)");
			AddEntry(1, "", "Show Map (Google Maps) for position of selected event");
			AddEntry(1, "", "Show Path (Google Earth) of all opened events");
			AddEntry(1, "", "Allow drag'n'drop for ED_V files");
			AddEntry(3, "0.3.2.0", "10.03.2010");
			AddEntry(1, "", "Generate text file");
			AddEntry(3, "0.3.1.0", "08.03.2010");
			AddEntry(1, "", "Fix first 3 columns of environment data");
			AddEntry(3, "0.3.0.0", "08.03.2010");
			AddEntry(1, "", "Allow max. 5 user definable columns with environment data");
			AddEntry(2, "", "Numeric sort criteria for ProcessId and EventID (not text based)");
			AddEntry(3, "0.2.1.0", "04.03.2010");
			AddEntry(1, "", "Save setting of moved columns in event table");
			AddEntry(2, "", "Add more language codes");
			AddEntry(3, "0.2.0.0", "03.03.2010");
			AddEntry(1, "", "Options to suppress some warnings");
			AddEntry(2, "", "Take over settings of previous version");
			AddEntry(3, "0.1.1.2", "01.03.2010");
			AddEntry(2, "", "Write EXCEL cells in text format");
			AddEntry(1, "", "Save and open packages");
			AddEntry(1, "", "Add column: Subsystem Descr");
			AddEntry(1, "", "Prepare environment data for STSNT never in HEX");
			AddEntry(3, "0.1.1.1", "22.02.2010");
			AddEntry(1, "", "Do not write any end-time in case of end-time = 0");
			AddEntry(3, "0.1.1.0", "18.02.2010");
			AddEntry(1, "", "Present Multiple event data files");
			AddEntry(3, "0.1.0.0", "12.02. 2010");
			AddEntry(1, "", "Export event table to Excel file");
			AddEntry(3, "0.0.0.8", "04.02.2010");
			AddEntry(2, "", "Correct wrong signal numbering in STSNT output");
			AddEntry(2, "", "Date and time representation foir STSNT output");
			AddEntry(3, "0.0.0.7", "02.02.2010");
			AddEntry(2, "", "Correct ANALOG value representation in environment view");
			AddEntry(3, "0.0.0.6", "22.01.2010");
			AddEntry(1, "", "Sort environment variables in order of bit offset");
			AddEntry(3, "0.0.0.5", "20.01.2010");
			AddEntry(1, "", "Selectable Error Code Format: Hex or Decimal");
			AddEntry(1, "", "Definition of Time Span for STSNT output");
			AddEntry(1, "", "Open ED_V File Dialog in Detail representation");
			AddEntry(2, "", "Correct representation of signed environment variables");
		}

		private void AddEntry(int type, string version, string description)
		{
			string[] array = new string[3] { version, null, null };
			switch (type)
			{
			case 0:
				array[1] = "";
				break;
			case 1:
				array[1] = "New";
				break;
			case 2:
				array[1] = "Corr";
				break;
			case 3:
				array[1] = "";
				break;
			case 4:
				array[1] = "To Do";
				break;
			default:
				array[1] = "";
				break;
			}
			array[2] = description;
			int index = dgv_RevHistory.Rows.Add(array);
			if (type == 3)
			{
				Font font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
				dgv_RevHistory.Rows[index].DefaultCellStyle.BackColor = Color.LightGray;
				dgv_RevHistory.Rows[index].DefaultCellStyle.Font = font;
			}
		}

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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TDSView.RevisionHistory));
			this.dgv_RevHistory = new System.Windows.Forms.DataGridView();
			this.Version = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)this.dgv_RevHistory).BeginInit();
			base.SuspendLayout();
			this.dgv_RevHistory.AllowUserToAddRows = false;
			this.dgv_RevHistory.AllowUserToDeleteRows = false;
			this.dgv_RevHistory.AllowUserToResizeColumns = false;
			this.dgv_RevHistory.AllowUserToResizeRows = false;
			this.dgv_RevHistory.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.dgv_RevHistory.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.dgv_RevHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgv_RevHistory.Columns.AddRange(this.Version, this.Type, this.Description);
			dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			dataGridViewCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgv_RevHistory.DefaultCellStyle = dataGridViewCellStyle;
			this.dgv_RevHistory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgv_RevHistory.Location = new System.Drawing.Point(0, 0);
			this.dgv_RevHistory.Name = "dgv_RevHistory";
			this.dgv_RevHistory.ReadOnly = true;
			this.dgv_RevHistory.RowHeadersVisible = false;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgv_RevHistory.RowsDefaultCellStyle = dataGridViewCellStyle2;
			this.dgv_RevHistory.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgv_RevHistory.RowTemplate.ReadOnly = true;
			this.dgv_RevHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgv_RevHistory.Size = new System.Drawing.Size(460, 384);
			this.dgv_RevHistory.TabIndex = 0;
			this.Version.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.Version.HeaderText = "Version";
			this.Version.Name = "Version";
			this.Version.ReadOnly = true;
			this.Version.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Version.Width = 48;
			this.Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.Type.HeaderText = "Type";
			this.Type.Name = "Type";
			this.Type.ReadOnly = true;
			this.Type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Type.Width = 37;
			this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Description.HeaderText = "Description";
			this.Description.Name = "Description";
			this.Description.ReadOnly = true;
			this.Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(460, 384);
			base.Controls.Add(this.dgv_RevHistory);
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "RevisionHistory";
			this.Text = "RevisionHistory";
			((System.ComponentModel.ISupportInitialize)this.dgv_RevHistory).EndInit();
			base.ResumeLayout(false);
		}
	}
}
