using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TDSView
{
	public class InformationForm : Form
	{
		private IContainer components;

		private Button btn_Exit;

		private DataGridView dgv_Information;

		private DataGridViewTextBoxColumn Item;

		private DataGridViewTextBoxColumn Content;

		private Label lbl_TotalNbrOfEvents;

		public ED ed;

		public ED_V edv;

		private bool rawData;

		private int totalNumberOfEvents;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TDSView.InformationForm));
			this.btn_Exit = new System.Windows.Forms.Button();
			this.dgv_Information = new System.Windows.Forms.DataGridView();
			this.Item = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Content = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.lbl_TotalNbrOfEvents = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)this.dgv_Information).BeginInit();
			base.SuspendLayout();
			resources.ApplyResources(this.btn_Exit, "btn_Exit");
			this.btn_Exit.Name = "btn_Exit";
			this.btn_Exit.UseVisualStyleBackColor = true;
			this.btn_Exit.Click += new System.EventHandler(buttonExit_Click);
			this.dgv_Information.AllowUserToAddRows = false;
			this.dgv_Information.AllowUserToDeleteRows = false;
			this.dgv_Information.AllowUserToResizeColumns = false;
			this.dgv_Information.AllowUserToResizeRows = false;
			this.dgv_Information.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.dgv_Information.BackgroundColor = System.Drawing.SystemColors.Control;
			this.dgv_Information.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dgv_Information.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgv_Information.Columns.AddRange(this.Item, this.Content);
			resources.ApplyResources(this.dgv_Information, "dgv_Information");
			this.dgv_Information.Name = "dgv_Information";
			this.dgv_Information.ReadOnly = true;
			this.dgv_Information.RowHeadersVisible = false;
			this.dgv_Information.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			resources.ApplyResources(this.Item, "Item");
			this.Item.Name = "Item";
			this.Item.ReadOnly = true;
			resources.ApplyResources(this.Content, "Content");
			this.Content.Name = "Content";
			this.Content.ReadOnly = true;
			resources.ApplyResources(this.lbl_TotalNbrOfEvents, "lbl_TotalNbrOfEvents");
			this.lbl_TotalNbrOfEvents.Name = "lbl_TotalNbrOfEvents";
			base.AcceptButton = this.btn_Exit;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.lbl_TotalNbrOfEvents);
			base.Controls.Add(this.dgv_Information);
			base.Controls.Add(this.btn_Exit);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.Name = "InformationForm";
			base.Load += new System.EventHandler(InformationForm_Load);
			((System.ComponentModel.ISupportInitialize)this.dgv_Information).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public InformationForm(bool raw)
		{
			rawData = raw;
			InitializeComponent();
			totalNumberOfEvents = 0;
		}

		private void buttonExit_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void InformationForm_Load(object sender, EventArgs e)
		{
			UpdateInfo(rawData);
			if (rawData)
			{
				lbl_TotalNbrOfEvents.Visible = false;
			}
			else
			{
				lbl_TotalNbrOfEvents.Text = TVStr.TotalNbrOfEvents + " " + totalNumberOfEvents;
			}
			Text = TVStr.GeneralFileInformation;
			btn_Exit.Text = TVStr.Close;
			Content.HeaderText = TVStr.Content;
			Item.HeaderText = TVStr.Item;
		}

		public void UpdateInfo(bool showRawData)
		{
			if (showRawData)
			{
				Text = "ED_V File Header Content";
				int count = dgv_Information.Columns.Count;
				string[] array = new string[count + 1];
				array[0] = "ED_V " + TVStr.File;
				array[1] = edv.fileName;
				dgv_Information.Rows.Add(array);
				array[0] = "FILE_ID";
				array[1] = edv.header.file_id;
				dgv_Information.Rows.Add(array);
				array[0] = "PREFIX";
				array[1] = edv.header.prefix;
				dgv_Information.Rows.Add(array);
				array[0] = "ED_VERSION";
				array[1] = edv.header.ed_version;
				dgv_Information.Rows.Add(array);
				array[0] = "PRJ_NAME";
				array[1] = edv.header.prj_name;
				dgv_Information.Rows.Add(array);
				array[0] = "PRJ_VERSION";
				array[1] = TDSViewUtil.StripVersion(edv.header.prj_version);
				dgv_Information.Rows.Add(array);
				array[0] = "DIAGNOSIS_SYSTEM";
				array[1] = edv.header.diagnosis_system;
				dgv_Information.Rows.Add(array);
				array[0] = "ODBS_ADR";
				array[1] = edv.header.odbs_adr;
				dgv_Information.Rows.Add(array);
				array[0] = "VEHICLE_NAME";
				array[1] = edv.header.vehicle_name;
				dgv_Information.Rows.Add(array);
				array[0] = "DATE/TIME_READ_OUT";
				char[] array2 = edv.header.date_read_out.ToCharArray();
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder = stringBuilder.Append(array2[0]).Append(array2[1]).Append(array2[2])
					.Append(array2[3])
					.Append(".");
				stringBuilder = stringBuilder.Append(array2[4]).Append(array2[5]).Append(".");
				stringBuilder = stringBuilder.Append(array2[6]).Append(array2[7]).Append(" ");
				array2 = edv.header.time_read_out.ToCharArray();
				stringBuilder = stringBuilder.Append(array2[0]).Append(array2[1]).Append(":");
				stringBuilder = stringBuilder.Append(array2[2]).Append(array2[3]).Append(":");
				stringBuilder = stringBuilder.Append(array2[4]).Append(array2[5]);
				array[1] = stringBuilder.ToString();
				dgv_Information.Rows.Add(array);
				array[0] = "EVENT_COPY";
				array[1] = edv.header.event_copy;
				dgv_Information.Rows.Add(array);
				if (edv.header.property_option.Length >= 8)
				{
					array[0] = "PROPERTY_OPTIONS1";
					array[1] = edv.header.property_option[1].ToString("X2") + edv.header.property_option[0].ToString("X2");
					dgv_Information.Rows.Add(array);
					array[0] = "PROPERTY_OPTIONS2";
					array[1] = edv.header.property_option[3].ToString("X2") + edv.header.property_option[2].ToString("X2");
					dgv_Information.Rows.Add(array);
					array[0] = "PROPERTY_OPTIONS3";
					array[1] = edv.header.property_option[5].ToString("X2") + edv.header.property_option[4].ToString("X2");
					dgv_Information.Rows.Add(array);
					array[0] = "PROPERTY_OPTIONS4";
					array[1] = edv.header.property_option[7].ToString("X2") + edv.header.property_option[6].ToString("X2");
					dgv_Information.Rows.Add(array);
				}
				array[0] = TVStr.NumberOfEvents;
				array[1] = edv.data.Count.ToString();
				dgv_Information.Rows.Add(array);
				totalNumberOfEvents += edv.data.Count;
				return;
			}
			int count2 = dgv_Information.Columns.Count;
			string[] array3 = new string[count2 + 1];
			foreach (ED_V item in ed.edvl)
			{
				array3[0] = TVStr.FileIndex;
				array3[1] = ed.edvl.IndexOf(item).ToString();
				int index = dgv_Information.Rows.Add(array3);
				dgv_Information.Rows[index].DefaultCellStyle.BackColor = Color.LightGray;
				array3[0] = TVStr.ProjectName;
				array3[1] = item.header.prj_name;
				dgv_Information.Rows.Add(array3);
				array3[0] = TVStr.VehicleName;
				array3[1] = item.header.vehicle_name;
				dgv_Information.Rows.Add(array3);
				array3[0] = TVStr.ODBSAddress;
				array3[1] = item.header.odbs_adr;
				dgv_Information.Rows.Add(array3);
				array3[0] = TVStr.ODBSVersion;
				array3[1] = TDSViewUtil.StripVersion(item.header.prj_version);
				dgv_Information.Rows.Add(array3);
				array3[0] = TVStr.ReadOutTime;
				char[] array4 = item.header.date_read_out.ToCharArray();
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2 = stringBuilder2.Append(array4[0]).Append(array4[1]).Append(array4[2])
					.Append(array4[3])
					.Append(".");
				stringBuilder2 = stringBuilder2.Append(array4[4]).Append(array4[5]).Append(".");
				stringBuilder2 = stringBuilder2.Append(array4[6]).Append(array4[7]).Append(" ");
				array4 = item.header.time_read_out.ToCharArray();
				stringBuilder2 = stringBuilder2.Append(array4[0]).Append(array4[1]).Append(":");
				stringBuilder2 = stringBuilder2.Append(array4[2]).Append(array4[3]).Append(":");
				stringBuilder2 = stringBuilder2.Append(array4[4]).Append(array4[5]);
				array3[1] = stringBuilder2.ToString();
				dgv_Information.Rows.Add(array3);
				array3[0] = TVStr.OTIVersion;
				array3[1] = item.header.ed_version;
				dgv_Information.Rows.Add(array3);
				array3[0] = "ED_V " + TVStr.File;
				array3[1] = item.fileName;
				dgv_Information.Rows.Add(array3);
				array3[0] = "ED_D " + TVStr.File;
				array3[1] = item.ed_d.fileName;
				dgv_Information.Rows.Add(array3);
				array3[0] = "ED_T " + TVStr.File;
				if (item.ed_t != null)
				{
					array3[1] = item.ed_t.fileName;
				}
				else
				{
					array3[1] = TVStr.NotAvailable;
				}
				dgv_Information.Rows.Add(array3);
				array3[0] = TVStr.NumberOfEvents;
				array3[1] = item.data.Count.ToString();
				dgv_Information.Rows.Add(array3);
				totalNumberOfEvents += item.data.Count;
			}
		}
	}
}
