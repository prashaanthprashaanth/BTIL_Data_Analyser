using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace TDSView
{
	public class Filter : Form
	{
		private IContainer components;

		private CheckedListBox clb_Elements;

		private Button btn_ApplyFilter;

		private Button btn_SelectAll;

		private Button btn_DeselectAll;

		private Button btn_Cancel;

		private Button btn_RemoveAllFilters;

		private DataGridView dgv_tmp;

		private string columnName;

		private bool lastColumn;

		public bool allItemsSelected;

		public bool removedAllFilters;

		public bool cancel;

		public bool newLastFilter;

		public string filterExpression;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TDSView.Filter));
			this.clb_Elements = new System.Windows.Forms.CheckedListBox();
			this.btn_ApplyFilter = new System.Windows.Forms.Button();
			this.btn_SelectAll = new System.Windows.Forms.Button();
			this.btn_DeselectAll = new System.Windows.Forms.Button();
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.btn_RemoveAllFilters = new System.Windows.Forms.Button();
			base.SuspendLayout();
			this.clb_Elements.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.clb_Elements.CheckOnClick = true;
			this.clb_Elements.FormattingEnabled = true;
			this.clb_Elements.Location = new System.Drawing.Point(13, 25);
			this.clb_Elements.Name = "clb_Elements";
			this.clb_Elements.Size = new System.Drawing.Size(341, 214);
			this.clb_Elements.TabIndex = 0;
			this.clb_Elements.SelectedValueChanged += new System.EventHandler(clb_Elements_SelectedValueChanged);
			this.btn_ApplyFilter.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			this.btn_ApplyFilter.Location = new System.Drawing.Point(403, 103);
			this.btn_ApplyFilter.Name = "btn_ApplyFilter";
			this.btn_ApplyFilter.Size = new System.Drawing.Size(75, 36);
			this.btn_ApplyFilter.TabIndex = 1;
			this.btn_ApplyFilter.Text = "Apply Filter";
			this.btn_ApplyFilter.UseVisualStyleBackColor = true;
			this.btn_ApplyFilter.Click += new System.EventHandler(btn_ApplyFilter_Click);
			this.btn_SelectAll.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.btn_SelectAll.Location = new System.Drawing.Point(403, 25);
			this.btn_SelectAll.Name = "btn_SelectAll";
			this.btn_SelectAll.Size = new System.Drawing.Size(75, 23);
			this.btn_SelectAll.TabIndex = 2;
			this.btn_SelectAll.Text = "Select all";
			this.btn_SelectAll.UseVisualStyleBackColor = true;
			this.btn_SelectAll.Click += new System.EventHandler(btn_SelectAll_Click);
			this.btn_DeselectAll.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.btn_DeselectAll.Location = new System.Drawing.Point(403, 54);
			this.btn_DeselectAll.Name = "btn_DeselectAll";
			this.btn_DeselectAll.Size = new System.Drawing.Size(75, 23);
			this.btn_DeselectAll.TabIndex = 3;
			this.btn_DeselectAll.Text = "Deselect all";
			this.btn_DeselectAll.UseVisualStyleBackColor = true;
			this.btn_DeselectAll.Click += new System.EventHandler(btn_DeselectAll_Click);
			this.btn_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			this.btn_Cancel.Location = new System.Drawing.Point(403, 216);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
			this.btn_Cancel.TabIndex = 4;
			this.btn_Cancel.Text = "Cancel";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new System.EventHandler(btn_Cancel_Click);
			this.btn_RemoveAllFilters.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			this.btn_RemoveAllFilters.Location = new System.Drawing.Point(403, 174);
			this.btn_RemoveAllFilters.Name = "btn_RemoveAllFilters";
			this.btn_RemoveAllFilters.Size = new System.Drawing.Size(75, 36);
			this.btn_RemoveAllFilters.TabIndex = 5;
			this.btn_RemoveAllFilters.Text = "Remove all Filters";
			this.btn_RemoveAllFilters.UseVisualStyleBackColor = true;
			this.btn_RemoveAllFilters.Click += new System.EventHandler(btn_RemoveAllFilters_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(506, 266);
			base.Controls.Add(this.btn_RemoveAllFilters);
			base.Controls.Add(this.btn_Cancel);
			base.Controls.Add(this.btn_DeselectAll);
			base.Controls.Add(this.btn_SelectAll);
			base.Controls.Add(this.btn_ApplyFilter);
			base.Controls.Add(this.clb_Elements);
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "Filter";
			this.Text = "Filter";
			base.ResumeLayout(false);
		}

		public Filter(ref DataGridView dgv, DataTable dt, string colName, bool isLastFilter)
		{
			InitializeComponent();
			GC.Collect();
			Text = TVStr.Filter + ": " + dgv.Columns[colName].HeaderText;
			btn_SelectAll.Text = TVStr.FilterSelectAll;
			btn_RemoveAllFilters.Text = TVStr.FilterRemoveAllFilters;
			btn_ApplyFilter.Text = TVStr.FilterApplyFilter;
			btn_Cancel.Text = TVStr.Cancel;
			btn_DeselectAll.Text = TVStr.FilterDeselectAll;
			columnName = colName;
			lastColumn = isLastFilter;
			newLastFilter = false;
			allItemsSelected = true;
			removedAllFilters = false;
			cancel = false;
			filterExpression = "";
			SortedList<string, bool> sortedList = new SortedList<string, bool>();
			dgv_tmp = dgv;
			bool value2;
			for (int i = 0; i < dgv.Rows.Count; i++)
			{
				DataGridViewTextBoxCell dataGridViewTextBoxCell = (DataGridViewTextBoxCell)dgv.Rows[i].Cells[columnName];
				bool value = true;
				string key = dataGridViewTextBoxCell.Value.ToString();
				if (!sortedList.TryGetValue(key, out value2))
				{
					sortedList.Add(key, value);
				}
			}
			if (lastColumn)
			{
				string rowFilter = ModifyFilterToCreateCheckBoxes(dt, restoreFilter: false);
				dt.DefaultView.RowFilter = rowFilter;
				for (int j = 0; j < dgv.Rows.Count; j++)
				{
					DataGridViewTextBoxCell dataGridViewTextBoxCell2 = (DataGridViewTextBoxCell)dgv.Rows[j].Cells[columnName];
					bool value3 = false;
					string key2 = dataGridViewTextBoxCell2.Value.ToString();
					if (!sortedList.TryGetValue(key2, out value2))
					{
						sortedList.Add(key2, value3);
					}
				}
			}
			foreach (KeyValuePair<string, bool> item in sortedList)
			{
				int index = clb_Elements.Items.Add(item.Key);
				clb_Elements.SetItemChecked(index, item.Value);
			}
			dt.DefaultView.RowFilter = ModifyFilterToCreateCheckBoxes(dt, restoreFilter: true);
		}

		private string ModifyFilterToCreateCheckBoxes(DataTable dt, bool restoreFilter)
		{
			bool flag = true;
			string text = "";
			foreach (DataColumn column in dt.Columns)
			{
				if ((!column.ColumnName.Equals(columnName) || restoreFilter) && column.ExtendedProperties.Count > 0 && !column.ExtendedProperties["Expr"].Equals(""))
				{
					if (flag)
					{
						object obj = text;
						text = string.Concat(obj, "(", column.ExtendedProperties["Expr"], ")");
						flag = false;
					}
					else
					{
						object obj2 = text;
						text = string.Concat(obj2, " AND (", column.ExtendedProperties["Expr"], ")");
					}
				}
			}
			return text;
		}

		private void btn_ApplyFilter_Click(object sender, EventArgs e)
		{
			allItemsSelected = AllSelected() && lastColumn;
			string text = "";
			bool flag = true;
			foreach (object checkedItem in clb_Elements.CheckedItems)
			{
				if (flag)
				{
					text = text + "'" + checkedItem.ToString() + "'";
					flag = false;
				}
				else
				{
					text = text + ", '" + checkedItem.ToString() + "'";
				}
			}
			if (text.Equals(""))
			{
				text = "''";
			}
			filterExpression = columnName + " IN (" + text + ")";
			newLastFilter = true;
			Close();
		}

		private bool isChecked(string content)
		{
			bool result = false;
			int num = clb_Elements.Items.IndexOf(content);
			if (num >= 0)
			{
				result = clb_Elements.GetItemChecked(num);
			}
			return result;
		}

		private bool AllSelected()
		{
			for (int i = 0; i < clb_Elements.Items.Count; i++)
			{
				if (!clb_Elements.GetItemChecked(i))
				{
					return false;
				}
			}
			return true;
		}

		private void btn_SelectAll_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < clb_Elements.Items.Count; i++)
			{
				clb_Elements.SetItemChecked(i, value: true);
			}
			btn_ApplyFilter.Enabled = true;
		}

		private void btn_DeselectAll_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < clb_Elements.Items.Count; i++)
			{
				clb_Elements.SetItemChecked(i, value: false);
			}
			btn_ApplyFilter.Enabled = false;
		}

		private void btn_Cancel_Click(object sender, EventArgs e)
		{
			cancel = true;
			Close();
		}

		private void btn_RemoveAllFilters_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(TVStr.FilterRemoveQuestion, TVStr.FilterRemoveAll, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
			{
				return;
			}
			for (int i = 0; i < dgv_tmp.Rows.Count; i++)
			{
				foreach (DataGridViewCell cell in dgv_tmp.Rows[i].Cells)
				{
					cell.Tag = true;
				}
				dgv_tmp.Rows[i].Visible = true;
			}
			removedAllFilters = true;
			Close();
		}

		private void clb_Elements_SelectedValueChanged(object sender, EventArgs e)
		{
			btn_ApplyFilter.Enabled = clb_Elements.CheckedItems.Count > 0;
		}
	}
}
