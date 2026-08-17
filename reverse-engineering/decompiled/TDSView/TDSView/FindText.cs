using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace TDSView
{
	public class FindText : Form
	{
		private IContainer components;

		private Button btn_Find;

		private Button btn_Cancel;

		private TextBox tb_SearchString;

		private CheckBox cb_EventDescription;

		private CheckBox cb_SignalName;

		private CheckBox cb_MarkAll;

		private Button btn_KeepAndFind;

		private CheckBox cb_SearchAll;

		public string searchString;

		public bool searchInEventDescription;

		public bool searchInSignalName;

		public bool searchAll;

		public bool markAll;

		public bool keepAndFind;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TDSView.FindText));
			this.btn_Find = new System.Windows.Forms.Button();
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.tb_SearchString = new System.Windows.Forms.TextBox();
			this.cb_EventDescription = new System.Windows.Forms.CheckBox();
			this.cb_SignalName = new System.Windows.Forms.CheckBox();
			this.cb_MarkAll = new System.Windows.Forms.CheckBox();
			this.btn_KeepAndFind = new System.Windows.Forms.Button();
			this.cb_SearchAll = new System.Windows.Forms.CheckBox();
			base.SuspendLayout();
			this.btn_Find.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this.btn_Find, "btn_Find");
			this.btn_Find.Name = "btn_Find";
			this.btn_Find.UseVisualStyleBackColor = true;
			this.btn_Find.Click += new System.EventHandler(btn_Find_Click);
			this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.btn_Cancel, "btn_Cancel");
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new System.EventHandler(btn_Cancel_Click);
			resources.ApplyResources(this.tb_SearchString, "tb_SearchString");
			this.tb_SearchString.Name = "tb_SearchString";
			resources.ApplyResources(this.cb_EventDescription, "cb_EventDescription");
			this.cb_EventDescription.Name = "cb_EventDescription";
			this.cb_EventDescription.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.cb_SignalName, "cb_SignalName");
			this.cb_SignalName.Name = "cb_SignalName";
			this.cb_SignalName.UseVisualStyleBackColor = true;
			resources.ApplyResources(this.cb_MarkAll, "cb_MarkAll");
			this.cb_MarkAll.Name = "cb_MarkAll";
			this.cb_MarkAll.UseVisualStyleBackColor = true;
			this.cb_MarkAll.CheckedChanged += new System.EventHandler(cb_MarkAll_CheckedChanged);
			this.btn_KeepAndFind.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this.btn_KeepAndFind, "btn_KeepAndFind");
			this.btn_KeepAndFind.Name = "btn_KeepAndFind";
			this.btn_KeepAndFind.UseVisualStyleBackColor = true;
			this.btn_KeepAndFind.Click += new System.EventHandler(btn_KeepAndFind_Click);
			resources.ApplyResources(this.cb_SearchAll, "cb_SearchAll");
			this.cb_SearchAll.Name = "cb_SearchAll";
			this.cb_SearchAll.UseVisualStyleBackColor = true;
			this.cb_SearchAll.CheckedChanged += new System.EventHandler(cb_SearchAll_CheckedChanged);
			base.AcceptButton = this.btn_Find;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.btn_Cancel;
			base.Controls.Add(this.cb_SearchAll);
			base.Controls.Add(this.btn_KeepAndFind);
			base.Controls.Add(this.cb_MarkAll);
			base.Controls.Add(this.cb_SignalName);
			base.Controls.Add(this.cb_EventDescription);
			base.Controls.Add(this.tb_SearchString);
			base.Controls.Add(this.btn_Cancel);
			base.Controls.Add(this.btn_Find);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.Name = "FindText";
			base.Load += new System.EventHandler(FindText_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FindText()
		{
			InitializeComponent();
			InitText();
			searchString = "";
			btn_KeepAndFind.Visible = markAll;
		}

		private void InitText()
		{
			Text = TVStr.FindText;
			btn_Cancel.Text = TVStr.Cancel;
			btn_Find.Text = TVStr.Find;
			btn_KeepAndFind.Text = TVStr.KeepSelAndFind;
			cb_EventDescription.Text = TVStr.SearchInEventDescription;
			cb_MarkAll.Text = TVStr.MarkAll;
			cb_SearchAll.Text = TVStr.SearchInAllVisibleEntries;
			cb_SignalName.Text = TVStr.SearchInSignalName;
		}

		private void btn_Find_Click(object sender, EventArgs e)
		{
			searchString = tb_SearchString.Text;
			searchInEventDescription = cb_EventDescription.Checked;
			searchInSignalName = cb_SignalName.Checked;
			searchAll = cb_SearchAll.Checked;
			markAll = cb_MarkAll.Checked;
			keepAndFind = false;
			Close();
		}

		private void btn_Cancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void FindText_Load(object sender, EventArgs e)
		{
			tb_SearchString.Text = searchString;
			cb_EventDescription.Checked = searchInEventDescription;
			cb_SignalName.Checked = searchInSignalName;
			cb_MarkAll.Checked = markAll;
			cb_SearchAll.Checked = searchAll;
			if (searchAll)
			{
				cb_SignalName.Enabled = false;
				cb_EventDescription.Enabled = false;
			}
		}

		private void btn_KeepAndFind_Click(object sender, EventArgs e)
		{
			searchString = tb_SearchString.Text;
			searchInEventDescription = cb_EventDescription.Checked;
			searchInSignalName = cb_SignalName.Checked;
			searchAll = cb_SearchAll.Checked;
			markAll = cb_MarkAll.Checked;
			keepAndFind = true;
			Close();
		}

		private void cb_MarkAll_CheckedChanged(object sender, EventArgs e)
		{
			btn_KeepAndFind.Visible = cb_MarkAll.Checked;
		}

		private void cb_SearchAll_CheckedChanged(object sender, EventArgs e)
		{
			if (cb_SearchAll.Checked)
			{
				cb_EventDescription.Enabled = false;
				cb_SignalName.Enabled = false;
			}
			else
			{
				cb_EventDescription.Enabled = true;
				cb_SignalName.Enabled = true;
			}
		}
	}
}
