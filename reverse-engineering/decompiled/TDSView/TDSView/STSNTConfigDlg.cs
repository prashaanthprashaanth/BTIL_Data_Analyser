using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace TDSView
{
	public class STSNTConfigDlg : Form
	{
		private ArrayList tL;

		public DateTime selectedStartTime;

		public DateTime selectedEndTime;

		public bool accepted;

		public DateTime earliestSelectedTime;

		public DateTime latestSelectedTime;

		private string helpTakeOverSelection;

		private string helpAccept;

		private string helpListBoxStartTime;

		private string helpListBoxEndTime;

		private bool createOutputDirectly;

		private IContainer components;

		private Button btn_Accept;

		private ListBox lb_StartTime;

		private ListBox lb_BoxEndTime;

		private GroupBox gb_StartTime;

		private GroupBox gb_EndTime;

		private Button btn_Cancel;

		private Button btn_TakeOverSelection;

		private HelpProvider helpProvider;

		private DateTime SetSecondsToZero(DateTime inTime)
		{
			int millisecond = inTime.Millisecond;
			DateTime dateTime = inTime.AddMilliseconds(-millisecond);
			millisecond = inTime.Second;
			return dateTime.AddSeconds(-millisecond);
		}

		public STSNTConfigDlg(ArrayList tList, bool createOutputFromActualEvent)
		{
			InitializeComponent();
			tL = tList;
			int index = tL.Count - 1;
			ED_V_TimeListEntry eD_V_TimeListEntry = (ED_V_TimeListEntry)tL[0];
			ED_V_TimeListEntry eD_V_TimeListEntry2 = (ED_V_TimeListEntry)tL[index];
			DateTime secondsToZero = new DateTime(DateTime.MinValue.Ticks);
			secondsToZero = SetSecondsToZero(secondsToZero);
			foreach (ED_V_TimeListEntry item in tL)
			{
				SetSecondsToZero(item.time);
				if (secondsToZero.CompareTo(SetSecondsToZero(item.time)) < 0)
				{
					lb_StartTime.Items.Add(item.time.ToString("d") + " " + item.time.ToString("t"));
				}
				secondsToZero = SetSecondsToZero(item.time);
			}
			secondsToZero = new DateTime(DateTime.MinValue.Ticks);
			secondsToZero = SetSecondsToZero(secondsToZero);
			foreach (ED_V_TimeListEntry item2 in tL)
			{
				SetSecondsToZero(item2.time);
				if (secondsToZero.CompareTo(SetSecondsToZero(item2.time)) < 0)
				{
					lb_BoxEndTime.Items.Add(item2.time.ToString("d") + " " + item2.time.ToString("t"));
				}
				secondsToZero = SetSecondsToZero(item2.time);
			}
			lb_StartTime.SetSelected(0, value: true);
			lb_BoxEndTime.SetSelected(lb_BoxEndTime.Items.Count - 1, value: true);
			selectedStartTime = eD_V_TimeListEntry.time;
			selectedEndTime = eD_V_TimeListEntry2.time;
			defineHelpText();
			helpProvider.SetHelpString(btn_TakeOverSelection, helpTakeOverSelection);
			helpProvider.SetHelpString(btn_Accept, helpAccept);
			helpProvider.SetHelpString(lb_StartTime, helpListBoxStartTime);
			helpProvider.SetHelpString(lb_BoxEndTime, helpListBoxEndTime);
			createOutputDirectly = createOutputFromActualEvent;
			SetElementText();
		}

		private void SetElementText()
		{
			Text = TVStr.STSNTConfigDialog;
			btn_Accept.Text = TVStr.Accept;
			btn_Cancel.Text = TVStr.Cancel;
			btn_TakeOverSelection.Text = TVStr.TakeOverSelection;
			gb_EndTime.Text = TVStr.EndTime;
			gb_StartTime.Text = TVStr.StartTime;
		}

		private void STSNTConfigDlg_Load(object sender, EventArgs e)
		{
			if (createOutputDirectly)
			{
				buttonTakeOverSelectionPrc();
				buttonAcceptPrc();
			}
		}

		private void buttonAccept_Click(object sender, EventArgs e)
		{
			buttonAcceptPrc();
		}

		private void buttonAcceptPrc()
		{
			if (selectedStartTime.CompareTo(selectedEndTime) > 0)
			{
				MessageBox.Show(TVStr.StopLTStartTime, TVStr.MakesNoSense, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			selectedStartTime = selectedStartTime.AddMinutes(-1.0);
			selectedEndTime = selectedEndTime.AddMinutes(1.0);
			accepted = true;
			Close();
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			accepted = false;
			Close();
		}

		private void selectSelectedTime(ListBox lb, DateTime selTime)
		{
			int num = 0;
			selTime = SetSecondsToZero(selTime);
			int i;
			for (i = 0; i < lb.Items.Count; i++)
			{
				DateTime value = DateTime.Parse(lb.Items[i].ToString());
				if (selTime.CompareTo(value) <= 0)
				{
					num = i;
					if (num < 0)
					{
						num = 0;
					}
					break;
				}
			}
			if (num == 0 && i > 1)
			{
				num = lb.Items.Count - 1;
			}
			lb.SetSelected(num, value: true);
		}

		private void listBoxStartTime_SelectedIndexChanged(object sender, EventArgs e)
		{
			selectedStartTime = DateTime.Parse(lb_StartTime.SelectedItem.ToString());
		}

		private void listBoxEndTime_SelectedIndexChanged(object sender, EventArgs e)
		{
			selectedEndTime = DateTime.Parse(lb_BoxEndTime.SelectedItem.ToString());
		}

		private void buttonTakeOverSelection_Click(object sender, EventArgs e)
		{
			buttonTakeOverSelectionPrc();
		}

		private void buttonTakeOverSelectionPrc()
		{
			selectSelectedTime(lb_StartTime, earliestSelectedTime);
			selectSelectedTime(lb_BoxEndTime, latestSelectedTime);
		}

		private void defineHelpText()
		{
			helpTakeOverSelection = TVStr.HelpSTSNTIfYouPress;
			helpAccept = TVStr.HelpSTSNTAccept;
			helpListBoxStartTime = TVStr.HelpSTSNTSelectStartTime;
			helpListBoxEndTime = TVStr.HelpSTSNTSelectEndTime;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TDSView.STSNTConfigDlg));
			this.btn_Accept = new System.Windows.Forms.Button();
			this.lb_StartTime = new System.Windows.Forms.ListBox();
			this.lb_BoxEndTime = new System.Windows.Forms.ListBox();
			this.gb_StartTime = new System.Windows.Forms.GroupBox();
			this.gb_EndTime = new System.Windows.Forms.GroupBox();
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.btn_TakeOverSelection = new System.Windows.Forms.Button();
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			this.gb_StartTime.SuspendLayout();
			this.gb_EndTime.SuspendLayout();
			base.SuspendLayout();
			resources.ApplyResources(this.btn_Accept, "btn_Accept");
			this.btn_Accept.Name = "btn_Accept";
			this.helpProvider.SetShowHelp(this.btn_Accept, (bool)resources.GetObject("btn_Accept.ShowHelp"));
			this.btn_Accept.UseVisualStyleBackColor = true;
			this.btn_Accept.Click += new System.EventHandler(buttonAccept_Click);
			this.lb_StartTime.FormattingEnabled = true;
			resources.ApplyResources(this.lb_StartTime, "lb_StartTime");
			this.lb_StartTime.Name = "lb_StartTime";
			this.helpProvider.SetShowHelp(this.lb_StartTime, (bool)resources.GetObject("lb_StartTime.ShowHelp"));
			this.lb_StartTime.SelectedIndexChanged += new System.EventHandler(listBoxStartTime_SelectedIndexChanged);
			this.lb_BoxEndTime.FormattingEnabled = true;
			resources.ApplyResources(this.lb_BoxEndTime, "lb_BoxEndTime");
			this.lb_BoxEndTime.Name = "lb_BoxEndTime";
			this.helpProvider.SetShowHelp(this.lb_BoxEndTime, (bool)resources.GetObject("lb_BoxEndTime.ShowHelp"));
			this.lb_BoxEndTime.SelectedIndexChanged += new System.EventHandler(listBoxEndTime_SelectedIndexChanged);
			this.gb_StartTime.Controls.Add(this.lb_StartTime);
			resources.ApplyResources(this.gb_StartTime, "gb_StartTime");
			this.gb_StartTime.Name = "gb_StartTime";
			this.helpProvider.SetShowHelp(this.gb_StartTime, (bool)resources.GetObject("gb_StartTime.ShowHelp"));
			this.gb_StartTime.TabStop = false;
			this.gb_EndTime.Controls.Add(this.lb_BoxEndTime);
			resources.ApplyResources(this.gb_EndTime, "gb_EndTime");
			this.gb_EndTime.Name = "gb_EndTime";
			this.helpProvider.SetShowHelp(this.gb_EndTime, (bool)resources.GetObject("gb_EndTime.ShowHelp"));
			this.gb_EndTime.TabStop = false;
			this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.btn_Cancel, "btn_Cancel");
			this.btn_Cancel.Name = "btn_Cancel";
			this.helpProvider.SetShowHelp(this.btn_Cancel, (bool)resources.GetObject("btn_Cancel.ShowHelp"));
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new System.EventHandler(buttonCancel_Click);
			this.helpProvider.SetHelpString(this.btn_TakeOverSelection, resources.GetString("btn_TakeOverSelection.HelpString"));
			resources.ApplyResources(this.btn_TakeOverSelection, "btn_TakeOverSelection");
			this.btn_TakeOverSelection.Name = "btn_TakeOverSelection";
			this.helpProvider.SetShowHelp(this.btn_TakeOverSelection, (bool)resources.GetObject("btn_TakeOverSelection.ShowHelp"));
			this.btn_TakeOverSelection.UseVisualStyleBackColor = true;
			this.btn_TakeOverSelection.Click += new System.EventHandler(buttonTakeOverSelection_Click);
			base.AcceptButton = this.btn_Accept;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			base.CancelButton = this.btn_Cancel;
			base.Controls.Add(this.btn_TakeOverSelection);
			base.Controls.Add(this.btn_Cancel);
			base.Controls.Add(this.btn_Accept);
			base.Controls.Add(this.gb_StartTime);
			base.Controls.Add(this.gb_EndTime);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.HelpButton = true;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "STSNTConfigDlg";
			this.helpProvider.SetShowHelp(this, (bool)resources.GetObject("$this.ShowHelp"));
			base.Load += new System.EventHandler(STSNTConfigDlg_Load);
			this.gb_StartTime.ResumeLayout(false);
			this.gb_EndTime.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
