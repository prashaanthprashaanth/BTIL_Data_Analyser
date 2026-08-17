using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TDSView
{
	public class PrintOptions : Form
	{
		private IContainer components;

		internal RadioButton rb_SelectedRows;

		internal RadioButton rb_AllRows;

		internal CheckBox cb_FitToPageWidth;

		internal Label lbl_Title;

		internal TextBox tb_txtTitle;

		internal GroupBox gbox_RowsToPrint;

		internal Label lbl_ColumnsToPrint;

		protected Button btn_OK;

		protected Button btn_Cancel;

		internal CheckedListBox clb_PrintCol;

		public string PrintTitle => tb_txtTitle.Text;

		public bool PrintAllRows => rb_AllRows.Checked;

		public bool FitToPageWidth => cb_FitToPageWidth.Checked;

		public PrintOptions()
		{
			InitializeComponent();
			SetElementText();
		}

		public PrintOptions(List<string> availableFields)
		{
			InitializeComponent();
			foreach (string availableField in availableFields)
			{
				clb_PrintCol.Items.Add(availableField, isChecked: true);
			}
			SetElementText();
		}

		private void SetElementText()
		{
			Text = TVStr.PrintOptions;
			btn_Cancel.Text = TVStr.Cancel;
			btn_OK.Text = TVStr.OK;
			cb_FitToPageWidth.Text = TVStr.FitToPageWidth;
			gbox_RowsToPrint.Text = TVStr.RowsToPrint;
			lbl_ColumnsToPrint.Text = TVStr.ColumnsToPrint;
			lbl_Title.Text = TVStr.TitleOfPrint;
			rb_AllRows.Text = TVStr.AllRows;
			rb_SelectedRows.Text = TVStr.SelectedRows;
		}

		private void PrintOtions_Load(object sender, EventArgs e)
		{
			rb_AllRows.Checked = true;
			cb_FitToPageWidth.Checked = true;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			Close();
		}

		public List<string> GetSelectedColumns()
		{
			List<string> list = new List<string>();
			foreach (object checkedItem in clb_PrintCol.CheckedItems)
			{
				list.Add(checkedItem.ToString());
			}
			return list;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TDSView.PrintOptions));
			this.rb_SelectedRows = new System.Windows.Forms.RadioButton();
			this.rb_AllRows = new System.Windows.Forms.RadioButton();
			this.cb_FitToPageWidth = new System.Windows.Forms.CheckBox();
			this.lbl_Title = new System.Windows.Forms.Label();
			this.tb_txtTitle = new System.Windows.Forms.TextBox();
			this.gbox_RowsToPrint = new System.Windows.Forms.GroupBox();
			this.lbl_ColumnsToPrint = new System.Windows.Forms.Label();
			this.btn_OK = new System.Windows.Forms.Button();
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.clb_PrintCol = new System.Windows.Forms.CheckedListBox();
			this.gbox_RowsToPrint.SuspendLayout();
			base.SuspendLayout();
			this.rb_SelectedRows.AccessibleDescription = null;
			this.rb_SelectedRows.AccessibleName = null;
			resources.ApplyResources(this.rb_SelectedRows, "rdoSelectedRows");
			this.rb_SelectedRows.BackgroundImage = null;
			this.rb_SelectedRows.Name = "rdoSelectedRows";
			this.rb_SelectedRows.TabStop = true;
			this.rb_SelectedRows.UseVisualStyleBackColor = true;
			this.rb_AllRows.AccessibleDescription = null;
			this.rb_AllRows.AccessibleName = null;
			resources.ApplyResources(this.rb_AllRows, "rdoAllRows");
			this.rb_AllRows.BackgroundImage = null;
			this.rb_AllRows.Name = "rdoAllRows";
			this.rb_AllRows.TabStop = true;
			this.rb_AllRows.UseVisualStyleBackColor = true;
			this.cb_FitToPageWidth.AccessibleDescription = null;
			this.cb_FitToPageWidth.AccessibleName = null;
			resources.ApplyResources(this.cb_FitToPageWidth, "chkFitToPageWidth");
			this.cb_FitToPageWidth.BackgroundImage = null;
			this.cb_FitToPageWidth.Name = "chkFitToPageWidth";
			this.cb_FitToPageWidth.UseVisualStyleBackColor = true;
			this.lbl_Title.AccessibleDescription = null;
			this.lbl_Title.AccessibleName = null;
			resources.ApplyResources(this.lbl_Title, "lblTitle");
			this.lbl_Title.Name = "lblTitle";
			this.tb_txtTitle.AcceptsReturn = true;
			this.tb_txtTitle.AccessibleDescription = null;
			this.tb_txtTitle.AccessibleName = null;
			resources.ApplyResources(this.tb_txtTitle, "txtTitle");
			this.tb_txtTitle.BackgroundImage = null;
			this.tb_txtTitle.Font = null;
			this.tb_txtTitle.Name = "txtTitle";
			this.gbox_RowsToPrint.AccessibleDescription = null;
			this.gbox_RowsToPrint.AccessibleName = null;
			resources.ApplyResources(this.gbox_RowsToPrint, "gboxRowsToPrint");
			this.gbox_RowsToPrint.BackgroundImage = null;
			this.gbox_RowsToPrint.Controls.Add(this.rb_SelectedRows);
			this.gbox_RowsToPrint.Controls.Add(this.rb_AllRows);
			this.gbox_RowsToPrint.Name = "gboxRowsToPrint";
			this.gbox_RowsToPrint.TabStop = false;
			this.lbl_ColumnsToPrint.AccessibleDescription = null;
			this.lbl_ColumnsToPrint.AccessibleName = null;
			resources.ApplyResources(this.lbl_ColumnsToPrint, "lblColumnsToPrint");
			this.lbl_ColumnsToPrint.Name = "lblColumnsToPrint";
			this.btn_OK.AccessibleDescription = null;
			this.btn_OK.AccessibleName = null;
			resources.ApplyResources(this.btn_OK, "btnOK");
			this.btn_OK.BackColor = System.Drawing.SystemColors.Control;
			this.btn_OK.BackgroundImage = null;
			this.btn_OK.Cursor = System.Windows.Forms.Cursors.Default;
			this.btn_OK.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btn_OK.Name = "btnOK";
			this.btn_OK.UseVisualStyleBackColor = false;
			this.btn_OK.Click += new System.EventHandler(btnOK_Click);
			this.btn_Cancel.AccessibleDescription = null;
			this.btn_Cancel.AccessibleName = null;
			resources.ApplyResources(this.btn_Cancel, "btnCancel");
			this.btn_Cancel.BackColor = System.Drawing.SystemColors.Control;
			this.btn_Cancel.BackgroundImage = null;
			this.btn_Cancel.Cursor = System.Windows.Forms.Cursors.Default;
			this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_Cancel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btn_Cancel.Name = "btnCancel";
			this.btn_Cancel.UseVisualStyleBackColor = false;
			this.btn_Cancel.Click += new System.EventHandler(btnCancel_Click);
			this.clb_PrintCol.AccessibleDescription = null;
			this.clb_PrintCol.AccessibleName = null;
			resources.ApplyResources(this.clb_PrintCol, "chklst");
			this.clb_PrintCol.BackgroundImage = null;
			this.clb_PrintCol.CheckOnClick = true;
			this.clb_PrintCol.FormattingEnabled = true;
			this.clb_PrintCol.Name = "chklst";
			base.AccessibleDescription = null;
			base.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = null;
			base.Controls.Add(this.cb_FitToPageWidth);
			base.Controls.Add(this.lbl_Title);
			base.Controls.Add(this.tb_txtTitle);
			base.Controls.Add(this.gbox_RowsToPrint);
			base.Controls.Add(this.lbl_ColumnsToPrint);
			base.Controls.Add(this.btn_OK);
			base.Controls.Add(this.btn_Cancel);
			base.Controls.Add(this.clb_PrintCol);
			this.Font = null;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "PrintOptions";
			base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			base.Load += new System.EventHandler(PrintOtions_Load);
			this.gbox_RowsToPrint.ResumeLayout(false);
			this.gbox_RowsToPrint.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
