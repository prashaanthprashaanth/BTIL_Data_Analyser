using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using TDSView.Properties;

namespace TDSView
{
	internal class AboutBox : Form
	{
		private IContainer components;

		private TableLayoutPanel tableLayoutPanel;

		private PictureBox logoPictureBox;

		private Label labelProductName;

		private Label labelVersion;

		private Label labelCopyright;

		private Label labelCompanyName;

		private TextBox textBoxDescription;

		private Button okButton;

		private PictureBox pict_Credits;

		private ToolTip toolTip1;

		private string credit;

		public string AssemblyTitle
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), inherit: false);
				if (customAttributes.Length > 0)
				{
					AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute)customAttributes[0];
					if (assemblyTitleAttribute.Title != "")
					{
						return assemblyTitleAttribute.Title;
					}
				}
				return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

		public string AssemblyDescription
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), inherit: false);
				if (customAttributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyDescriptionAttribute)customAttributes[0]).Description;
			}
		}

		public string AssemblyProduct
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), inherit: false);
				if (customAttributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyProductAttribute)customAttributes[0]).Product;
			}
		}

		public string AssemblyCopyright
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), inherit: false);
				if (customAttributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCopyrightAttribute)customAttributes[0]).Copyright;
			}
		}

		public string AssemblyCompany
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), inherit: false);
				if (customAttributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCompanyAttribute)customAttributes[0]).Company;
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TDSView.AboutBox));
			this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.logoPictureBox = new System.Windows.Forms.PictureBox();
			this.labelProductName = new System.Windows.Forms.Label();
			this.labelVersion = new System.Windows.Forms.Label();
			this.labelCopyright = new System.Windows.Forms.Label();
			this.labelCompanyName = new System.Windows.Forms.Label();
			this.textBoxDescription = new System.Windows.Forms.TextBox();
			this.okButton = new System.Windows.Forms.Button();
			this.pict_Credits = new System.Windows.Forms.PictureBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.tableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.logoPictureBox).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.pict_Credits).BeginInit();
			base.SuspendLayout();
			resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
			this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.labelProductName, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.labelVersion, 1, 1);
			this.tableLayoutPanel.Controls.Add(this.labelCopyright, 1, 2);
			this.tableLayoutPanel.Controls.Add(this.labelCompanyName, 1, 3);
			this.tableLayoutPanel.Controls.Add(this.textBoxDescription, 1, 4);
			this.tableLayoutPanel.Controls.Add(this.okButton, 1, 5);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			resources.ApplyResources(this.logoPictureBox, "logoPictureBox");
			this.logoPictureBox.Name = "logoPictureBox";
			this.tableLayoutPanel.SetRowSpan(this.logoPictureBox, 6);
			this.logoPictureBox.TabStop = false;
			this.logoPictureBox.Click += new System.EventHandler(logoPictureBox_Click);
			resources.ApplyResources(this.labelProductName, "labelProductName");
			this.labelProductName.MaximumSize = new System.Drawing.Size(0, 17);
			this.labelProductName.Name = "labelProductName";
			resources.ApplyResources(this.labelVersion, "labelVersion");
			this.labelVersion.MaximumSize = new System.Drawing.Size(0, 17);
			this.labelVersion.Name = "labelVersion";
			resources.ApplyResources(this.labelCopyright, "labelCopyright");
			this.labelCopyright.MaximumSize = new System.Drawing.Size(0, 17);
			this.labelCopyright.Name = "labelCopyright";
			resources.ApplyResources(this.labelCompanyName, "labelCompanyName");
			this.labelCompanyName.MaximumSize = new System.Drawing.Size(0, 34);
			this.labelCompanyName.Name = "labelCompanyName";
			this.textBoxDescription.BackColor = System.Drawing.SystemColors.Control;
			resources.ApplyResources(this.textBoxDescription, "textBoxDescription");
			this.textBoxDescription.Name = "textBoxDescription";
			this.textBoxDescription.ReadOnly = true;
			this.textBoxDescription.TabStop = false;
			resources.ApplyResources(this.okButton, "okButton");
			this.okButton.BackColor = System.Drawing.SystemColors.Control;
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.okButton.Name = "okButton";
			this.okButton.UseVisualStyleBackColor = false;
			this.okButton.Click += new System.EventHandler(okButton_Click);
			this.pict_Credits.Image = TDSView.Properties.Resources.credits1;
			resources.ApplyResources(this.pict_Credits, "pict_Credits");
			this.pict_Credits.Name = "pict_Credits";
			this.pict_Credits.TabStop = false;
			this.pict_Credits.Click += new System.EventHandler(pict_Credits_Click);
			this.toolTip1.AutoPopDelay = 30000;
			this.toolTip1.InitialDelay = 500;
			this.toolTip1.IsBalloon = true;
			this.toolTip1.ReshowDelay = 100;
			this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			base.AcceptButton = this.okButton;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			base.Controls.Add(this.pict_Credits);
			base.Controls.Add(this.tableLayoutPanel);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AboutBox";
			base.ShowInTaskbar = false;
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.logoPictureBox).EndInit();
			((System.ComponentModel.ISupportInitialize)this.pict_Credits).EndInit();
			base.ResumeLayout(false);
		}

		public AboutBox()
		{
			InitializeComponent();
			Text = string.Format(TVStr.About + " {0}", AssemblyTitle);
			labelProductName.Text = AssemblyProduct;
			labelVersion.Text = string.Format(TVStr.Version + " {0}", AssemblyVersion);
			labelCopyright.Text = AssemblyCopyright;
			labelCompanyName.Text = AssemblyCompany;
			textBoxDescription.Text = TVStr.AboutDescription;
			credit = "Thanks to\n- Quingfeng Wu for the Chinese translation (中文翻译由BCP吴庆丰支持)\n- Ueli Nievergelt for a lot of useful hints (e.g. during a DBC workshop ;-)\n- Roland Bruegger for STSNT support\n- Wolfgang Mayer for feedback and suggestions from the 3rd floor and from Dalian\n- Hanspeter Hunziker for Ragusa and Baslerläckerli\n- Sven Künne. He pushed me to increase the performance in case of multiple event file usage";
			toolTip1.ToolTipTitle = "Credits";
			toolTip1.IsBalloon = true;
			toolTip1.SetToolTip(pict_Credits, credit);
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void logoPictureBox_Click(object sender, EventArgs e)
		{
			RevisionHistory revisionHistory = new RevisionHistory();
			revisionHistory.ShowDialog();
		}

		private void pict_Credits_Click(object sender, EventArgs e)
		{
			MessageBox.Show(credit, "Credits", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}
}
