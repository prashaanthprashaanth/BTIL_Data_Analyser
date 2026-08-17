using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TDSView
{
	public class RankingList : Form
	{
		private List<RankElement> rankingList;

		private IContainer components;

		private DataGridView dgv_Ranking;

		private DataGridViewTextBoxColumn Cnt;

		private DataGridViewTextBoxColumn ECode0;

		private DataGridViewTextBoxColumn SignalName;

		private DataGridViewTextBoxColumn Description;

		private DataGridViewTextBoxColumn EventId;

		private DataGridViewTextBoxColumn PrcId;

		public RankingList(ED ed, bool alternateRowColor)
		{
			rankingList = new List<RankElement>();
			InitializeComponent();
			if (alternateRowColor)
			{
				dgv_Ranking.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(226, 254, 222);
			}
			else
			{
				dgv_Ranking.AlternatingRowsDefaultCellStyle.BackColor = Color.Empty;
			}
			foreach (ED_V item in ed.edvl)
			{
				foreach (ED_V_Data datum in item.data)
				{
					bool flag = false;
					foreach (RankElement ranking in rankingList)
					{
						if (ranking.event_id == datum.event_id && ranking.prc_id == datum.process_id && ranking.eCode0.Equals(datum.errorcode_0.ToString("X4")))
						{
							flag = true;
							if (datum.event_cnt > ranking.event_cnt)
							{
								ranking.event_cnt = datum.event_cnt;
								break;
							}
						}
					}
					if (!flag)
					{
						RankElement rankElement = new RankElement
						{
							event_cnt = datum.event_cnt,
							event_id = datum.event_id,
							prc_id = datum.process_id,
							eCode0 = datum.errorcode_0.ToString("X4")
						};
						if (datum.eventDescr != null)
						{
							rankElement.signalName = datum.eventDescr.event_name;
							rankElement.eventDescr = datum.eventDescr.event_descr;
						}
						rankingList.Add(rankElement);
					}
				}
			}
			EventComparer comparer = new EventComparer();
			rankingList.Sort(comparer);
			string[] array = new string[dgv_Ranking.Columns.Count];
			foreach (RankElement ranking2 in rankingList)
			{
				array[dgv_Ranking.Columns.IndexOf(dgv_Ranking.Columns["Cnt"])] = ranking2.event_cnt.ToString();
				array[dgv_Ranking.Columns.IndexOf(dgv_Ranking.Columns["EventId"])] = ranking2.event_id.ToString();
				array[dgv_Ranking.Columns.IndexOf(dgv_Ranking.Columns["Description"])] = ranking2.eventDescr;
				array[dgv_Ranking.Columns.IndexOf(dgv_Ranking.Columns["ECode0"])] = ranking2.eCode0;
				array[dgv_Ranking.Columns.IndexOf(dgv_Ranking.Columns["SignalName"])] = ranking2.signalName;
				array[dgv_Ranking.Columns.IndexOf(dgv_Ranking.Columns["PrcId"])] = ranking2.prc_id.ToString();
				dgv_Ranking.Rows.Add(array);
			}
			SetElementText();
		}

		private void SetElementText()
		{
			Text = TVStr.RankingList;
			Cnt.HeaderText = TVStr.Cnt;
			Cnt.ToolTipText = TVStr.CntToolTip;
			Description.HeaderText = TVStr.Description;
			ECode0.HeaderText = TVStr.ECode0;
			EventId.HeaderText = TVStr.EventId;
			PrcId.HeaderText = TVStr.PrcId;
			SignalName.HeaderText = TVStr.SignalName;
		}

		private void dgv_Ranking_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TDSView.RankingList));
			this.dgv_Ranking = new System.Windows.Forms.DataGridView();
			this.Cnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ECode0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SignalName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EventId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PrcId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)this.dgv_Ranking).BeginInit();
			base.SuspendLayout();
			this.dgv_Ranking.AllowUserToAddRows = false;
			this.dgv_Ranking.AllowUserToDeleteRows = false;
			this.dgv_Ranking.AllowUserToResizeRows = false;
			dataGridViewCellStyle.BackColor = System.Drawing.Color.White;
			this.dgv_Ranking.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
			this.dgv_Ranking.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgv_Ranking.Columns.AddRange(this.Cnt, this.ECode0, this.SignalName, this.Description, this.EventId, this.PrcId);
			resources.ApplyResources(this.dgv_Ranking, "dgv_Ranking");
			this.dgv_Ranking.Name = "dgv_Ranking";
			this.dgv_Ranking.ReadOnly = true;
			this.dgv_Ranking.RowHeadersVisible = false;
			this.dgv_Ranking.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgv_Ranking.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgv_Ranking.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgv_Ranking_CellContentClick);
			this.Cnt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			resources.ApplyResources(this.Cnt, "Cnt");
			this.Cnt.Name = "Cnt";
			this.Cnt.ReadOnly = true;
			this.Cnt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.ECode0.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			resources.ApplyResources(this.ECode0, "ECode0");
			this.ECode0.Name = "ECode0";
			this.ECode0.ReadOnly = true;
			this.SignalName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			resources.ApplyResources(this.SignalName, "SignalName");
			this.SignalName.Name = "SignalName";
			this.SignalName.ReadOnly = true;
			this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.Description, "Description");
			this.Description.Name = "Description";
			this.Description.ReadOnly = true;
			this.Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.EventId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			resources.ApplyResources(this.EventId, "EventId");
			this.EventId.Name = "EventId";
			this.EventId.ReadOnly = true;
			this.PrcId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			resources.ApplyResources(this.PrcId, "PrcId");
			this.PrcId.Name = "PrcId";
			this.PrcId.ReadOnly = true;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.dgv_Ranking);
			base.Name = "RankingList";
			((System.ComponentModel.ISupportInitialize)this.dgv_Ranking).EndInit();
			base.ResumeLayout(false);
		}
	}
}
