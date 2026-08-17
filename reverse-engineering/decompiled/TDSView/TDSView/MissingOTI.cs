using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TDSView
{
	public class MissingOTI : Form
	{
		private IContainer components;

		private Button btn_Cancel;

		private Label lbl_missingOti;

		private Button btn_Search;

		private Label lbl_ManualOTISearch;

		public string availableOTI;

		public string selectedOTI;

		public bool browseOTI;

		private string[] avlLang;

		private int originalSizeOfForm;

		private Button[] btn_Language;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TDSView.MissingOTI));
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.lbl_missingOti = new System.Windows.Forms.Label();
			this.btn_Search = new System.Windows.Forms.Button();
			this.lbl_ManualOTISearch = new System.Windows.Forms.Label();
			base.SuspendLayout();
			this.btn_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			this.btn_Cancel.Location = new System.Drawing.Point(259, 158);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
			this.btn_Cancel.TabIndex = 0;
			this.btn_Cancel.Text = "Cancel";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new System.EventHandler(btn_Cancel_Click);
			this.lbl_missingOti.AutoSize = true;
			this.lbl_missingOti.Location = new System.Drawing.Point(13, 13);
			this.lbl_missingOti.Name = "lbl_missingOti";
			this.lbl_missingOti.Size = new System.Drawing.Size(35, 13);
			this.lbl_missingOti.TabIndex = 2;
			this.lbl_missingOti.Text = "label1";
			this.btn_Search.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.btn_Search.Location = new System.Drawing.Point(16, 158);
			this.btn_Search.Name = "btn_Search";
			this.btn_Search.Size = new System.Drawing.Size(75, 23);
			this.btn_Search.TabIndex = 3;
			this.btn_Search.Text = "Search";
			this.btn_Search.UseVisualStyleBackColor = true;
			this.btn_Search.Click += new System.EventHandler(btn_Search_Click);
			this.lbl_ManualOTISearch.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.lbl_ManualOTISearch.AutoSize = true;
			this.lbl_ManualOTISearch.Location = new System.Drawing.Point(12, 132);
			this.lbl_ManualOTISearch.Name = "lbl_ManualOTISearch";
			this.lbl_ManualOTISearch.Size = new System.Drawing.Size(35, 13);
			this.lbl_ManualOTISearch.TabIndex = 4;
			this.lbl_ManualOTISearch.Text = "label1";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(346, 193);
			base.Controls.Add(this.lbl_ManualOTISearch);
			base.Controls.Add(this.btn_Search);
			base.Controls.Add(this.lbl_missingOti);
			base.Controls.Add(this.btn_Cancel);
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "MissingOTI";
			this.Text = "Missing OTI";
			base.Load += new System.EventHandler(MissingOTI_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public MissingOTI()
		{
			InitializeComponent();
			availableOTI = "";
			browseOTI = false;
			lbl_missingOti.Text = TVStr.NotFoundOTI + "\n" + TVStr.MissingOTIAvlLanguages;
			lbl_ManualOTISearch.Text = TVStr.MissingOTISearchManual;
			originalSizeOfForm = base.ClientSize.Height;
			selectedOTI = "";
			Text = TVStr.MissingOTI;
			btn_Cancel.Text = TVStr.Cancel;
			btn_Search.Text = TVStr.Search;
		}

		private void MissingOTI_Load(object sender, EventArgs e)
		{
			int num = 50;
			avlLang = availableOTI.Split(',');
			int num2 = avlLang.Length;
			btn_Language = new Button[num2];
			for (int i = 0; i < num2; i++)
			{
				btn_Language[i] = new Button();
				btn_Language[i].Location = new Point(12, num + 30 * i);
				btn_Language[i].Name = i.ToString();
				btn_Language[i].Size = new Size(75, 23);
				btn_Language[i].TabIndex = 2;
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(avlLang[i]);
				btn_Language[i].Text = fileNameWithoutExtension.Substring(fileNameWithoutExtension.Length - 2);
				btn_Language[i].UseVisualStyleBackColor = true;
				base.Controls.Add(btn_Language[i]);
				btn_Language[i].Click += btn_Language_Click;
			}
			base.ClientSize = new Size(292, originalSizeOfForm + (num2 - 1) * 30);
		}

		private void btn_Cancel_Click(object sender, EventArgs e)
		{
			selectedOTI = "";
			Close();
		}

		private void btn_Language_Click(object sender, EventArgs e)
		{
			string name = ((Button)sender).Name;
			selectedOTI = avlLang[int.Parse(name)];
			Close();
		}

		private void btn_Search_Click(object sender, EventArgs e)
		{
			browseOTI = true;
			Close();
		}
	}
}
