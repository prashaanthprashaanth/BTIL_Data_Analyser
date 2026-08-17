using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TDSView.Properties;

namespace TDSView
{
	public class IBrowser : Form
	{
		public bool isOpen;

		private IContainer components;

		private ToolStrip toolStrip;

		private ToolStripButton btn_Exit;

		private WebBrowser webBrowser;

		public IBrowser(string address, string caption, bool canBeMaximized)
		{
			InitializeComponent();
			Text = caption;
			base.MaximizeBox = canBeMaximized;
			webBrowser.Navigate(address);
			isOpen = false;
		}

		public void NavigateTo(string addr)
		{
			if (isOpen)
			{
				webBrowser.Navigate(addr);
			}
		}

		private void IBrowser_FormClosed(object sender, FormClosedEventArgs e)
		{
			isOpen = false;
		}

		private void IBrowser_Load(object sender, EventArgs e)
		{
			isOpen = true;
		}

		private void exitButton_Click(object sender, EventArgs e)
		{
			Close();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TDSView.IBrowser));
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.btn_Exit = new System.Windows.Forms.ToolStripButton();
			this.webBrowser = new System.Windows.Forms.WebBrowser();
			this.toolStrip.SuspendLayout();
			base.SuspendLayout();
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btn_Exit });
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(758, 25);
			this.toolStrip.TabIndex = 0;
			this.toolStrip.Text = "toolStrip1";
			this.toolStrip.Visible = false;
			this.btn_Exit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btn_Exit.Image = TDSView.Properties.Resources.ClosePreviewHH;
			this.btn_Exit.ImageTransparentColor = System.Drawing.Color.Black;
			this.btn_Exit.Name = "btn_Exit";
			this.btn_Exit.Size = new System.Drawing.Size(23, 22);
			this.btn_Exit.Text = "toolStripButton1";
			this.btn_Exit.ToolTipText = "Close Map Window";
			this.btn_Exit.Click += new System.EventHandler(exitButton_Click);
			this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webBrowser.Location = new System.Drawing.Point(0, 0);
			this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser.Name = "webBrowser";
			this.webBrowser.Size = new System.Drawing.Size(758, 454);
			this.webBrowser.TabIndex = 1;
			this.webBrowser.Url = new System.Uri("http://www.nzz.ch", System.UriKind.Absolute);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(758, 454);
			base.Controls.Add(this.webBrowser);
			base.Controls.Add(this.toolStrip);
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "IBrowser";
			this.Text = "TDSView Position Tracker";
			base.Load += new System.EventHandler(IBrowser_Load);
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(IBrowser_FormClosed);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
