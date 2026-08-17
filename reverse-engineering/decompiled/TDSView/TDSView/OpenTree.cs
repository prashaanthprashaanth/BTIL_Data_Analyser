using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TDSView.Properties;

namespace TDSView
{
	public class OpenTree : Form
	{
		public int nbrOfFiles;

		public string rootDirectory;

		private string previousRootDir;

		private IContainer components;

		private Label lbl_nbrOfFoundFiles;

		private DataGridView dgv_EDVFiles;

		private TreeView tv_edvFiles;

		private DataGridViewTextBoxColumn Col_Project;

		private DataGridViewTextBoxColumn Col_Id;

		private DataGridViewTextBoxColumn Col_Date;

		private DataGridViewTextBoxColumn Col_ED_V_Path;

		private CheckBox cb_treeView;

		private TextBox tb_rootDir;

		private Button btn_browseDir;

		private FolderBrowserDialog rootFolderBrowserDialog;

		private Label lbl_RootDirectory;

		private Button btn_openSel;

		private Button btn_addSel;

		private Button btn_CloseForm;

		private Button btn_Reload;

		private Button btn_UncheckAll;

		public event EventHandler<SelectedEDVFileEventArgs> edvSelectionUpdated;

		public event EventHandler<OptionsEventArgs> OptionsUpdated;

		public OpenTree(string rootDirectory, bool isTreeView)
		{
			InitializeComponent();
			nbrOfFiles = 0;
			dgv_EDVFiles.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(226, 254, 222);
			tb_rootDir.Text = rootDirectory;
			previousRootDir = rootDirectory;
			cb_treeView.Checked = isTreeView;
			btn_addSel.Text = TVStr.AddSelection;
			btn_openSel.Text = TVStr.OpenSelection;
			lbl_RootDirectory.Text = TVStr.ED_V_Root_Directory;
			cb_treeView.Text = TVStr.TreeView;
			lbl_nbrOfFoundFiles.Text = TVStr.NbrFoundEDVFiles;
			btn_CloseForm.Text = TVStr.Close;
			Text = TVStr.Open_ED_V_Overview;
			dgv_EDVFiles.Columns[Col_Project.Index].HeaderText = TVStr.Project;
			dgv_EDVFiles.Columns[Col_Id.Index].HeaderText = TVStr.Vehicle;
			dgv_EDVFiles.Columns[Col_Date.Index].HeaderText = TVStr.Read_Out_Time;
			dgv_EDVFiles.Columns[Col_ED_V_Path.Index].HeaderText = TVStr.ED_V_Path;
			ToolTip toolTip = new ToolTip();
			toolTip.IsBalloon = true;
			toolTip.SetToolTip(btn_CloseForm, TVStr.OT_TT_Close);
			toolTip.SetToolTip(btn_UncheckAll, TVStr.OT_TT_DeselectAllCb);
			toolTip.SetToolTip(btn_openSel, TVStr.OT_TT_OpenSelEDV);
			toolTip.SetToolTip(btn_addSel, TVStr.OT_TT_AddSelEDV);
			toolTip.SetToolTip(btn_browseDir, TVStr.OT_TT_OpenDirDialog);
			toolTip.SetToolTip(btn_Reload, TVStr.OT_TT_ReloadDir);
			toolTip.SetToolTip(cb_treeView, TVStr.OT_TT_SwitchTreeList);
		}

		protected virtual void OnEdvSelectionUpdated(SelectedEDVFileEventArgs e)
		{
			if (this.edvSelectionUpdated != null)
			{
				this.edvSelectionUpdated(this, e);
			}
		}

		protected virtual void OnOptionsUpdated(OptionsEventArgs e)
		{
			if (this.OptionsUpdated != null)
			{
				this.OptionsUpdated(this, e);
			}
		}

		public void ShowTree()
		{
			try
			{
				if (!Directory.Exists(tb_rootDir.Text))
				{
					MessageBox.Show(TVStr.DirNotExist, TVStr.Warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				Cursor.Current = Cursors.WaitCursor;
				nbrOfFiles = 0;
				dgv_EDVFiles.Rows.Clear();
				string[] files = Directory.GetFiles(tb_rootDir.Text, "*.*", SearchOption.AllDirectories);
				string[] array = files;
				foreach (string text in array)
				{
					ED_V eD_V = new ED_V(useLocalTime: false, 0.0);
					try
					{
						if (eD_V.Read_ED_V_FileHeader(text) == 0 && eD_V.header.prefix == "ED_V")
						{
							string[] array2 = new string[4]
							{
								eD_V.header.prj_name,
								eD_V.header.vehicle_name,
								null,
								null
							};
							string text2 = eD_V.header.date_read_out.Substring(0, 4) + "-" + eD_V.header.date_read_out.Substring(4, 2) + "-" + eD_V.header.date_read_out.Substring(6, 2) + " " + eD_V.header.time_read_out.Substring(0, 2) + ":" + eD_V.header.time_read_out.Substring(2, 2) + ":" + eD_V.header.time_read_out.Substring(4, 2);
							array2[2] = text2;
							array2[3] = text;
							dgv_EDVFiles.Rows.Add(array2);
							nbrOfFiles++;
						}
					}
					catch (Exception)
					{
					}
				}
			}
			catch (Exception ex2)
			{
				MessageBox.Show(ex2.Message + "\n" + TVStr.CheckPath, TVStr.InvalidPath, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			dgv_EDVFiles.Sort(dgv_EDVFiles.Columns[0], ListSortDirection.Ascending);
			tv_edvFiles.Nodes.Clear();
			int num = 0;
			string value = "";
			TreeNode treeNode = null;
			TreeNode treeNode2 = null;
			string value2 = "";
			for (num = 0; num < dgv_EDVFiles.Rows.Count; num++)
			{
				DataGridViewTextBoxCell dataGridViewTextBoxCell = (DataGridViewTextBoxCell)dgv_EDVFiles.Rows[num].Cells["Col_Project"];
				string text3 = Convert.ToString(dataGridViewTextBoxCell.Value);
				if (!text3.Equals(value))
				{
					treeNode = tv_edvFiles.Nodes.Add(text3);
					value2 = "";
				}
				DataGridViewTextBoxCell dataGridViewTextBoxCell2 = (DataGridViewTextBoxCell)dgv_EDVFiles.Rows[num].Cells["Col_Id"];
				string text4 = Convert.ToString(dataGridViewTextBoxCell2.Value);
				if (!text4.Equals(value2))
				{
					treeNode2 = treeNode.Nodes.Add(text4);
				}
				DataGridViewTextBoxCell dataGridViewTextBoxCell3 = (DataGridViewTextBoxCell)dgv_EDVFiles.Rows[num].Cells["Col_Date"];
				string text5 = Convert.ToString(dataGridViewTextBoxCell3.Value);
				DataGridViewTextBoxCell dataGridViewTextBoxCell4 = (DataGridViewTextBoxCell)dgv_EDVFiles.Rows[num].Cells["Col_ED_V_Path"];
				string text6 = Convert.ToString(dataGridViewTextBoxCell4.Value);
				string text7 = text6.Replace(tb_rootDir.Text, "");
				treeNode2.Nodes.Add(text5 + " @ " + text7);
				value = text3;
				value2 = text4;
			}
			ShowTreeOrList(cb_treeView.Checked);
			lbl_nbrOfFoundFiles.Text = TVStr.NbrFoundEDVFiles + nbrOfFiles;
			Cursor.Current = Cursors.Default;
		}

		private void dgv_EDVFiles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0)
			{
				string[] array = new string[1];
				DataGridViewTextBoxCell dataGridViewTextBoxCell = (DataGridViewTextBoxCell)dgv_EDVFiles.Rows[e.RowIndex].Cells["Col_ED_V_Path"];
				array[0] = Convert.ToString(dataGridViewTextBoxCell.Value);
				SendSelectEvent(array, addIt: false);
			}
		}

		private void SendSelectEvent(string[] selPath, bool addIt)
		{
			SelectedEDVFileEventArgs e = new SelectedEDVFileEventArgs();
			int num = selPath.Length;
			e.Name = new string[num];
			for (int i = 0; i < selPath.Length; i++)
			{
				e.Name[i] = selPath[i];
			}
			e.addIt = addIt;
			OnEdvSelectionUpdated(e);
		}

		private void dgv_EDVFiles_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
			e.SortResult = string.Compare(e.CellValue1.ToString(), e.CellValue2.ToString());
			if (e.SortResult == 0 && e.Column.Name != "Col_Id")
			{
				e.SortResult = string.Compare(dgv_EDVFiles.Rows[e.RowIndex1].Cells["Col_Id"].Value.ToString(), dgv_EDVFiles.Rows[e.RowIndex2].Cells["Col_Id"].Value.ToString());
				if (e.SortResult == 0 && e.Column.Name != "Col_Date")
				{
					e.SortResult = string.Compare(dgv_EDVFiles.Rows[e.RowIndex1].Cells["Col_Date"].Value.ToString(), dgv_EDVFiles.Rows[e.RowIndex2].Cells["Col_Date"].Value.ToString());
				}
			}
			e.Handled = true;
		}

		private void tv_edvFiles_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (tv_edvFiles.SelectedNode != null && tv_edvFiles.SelectedNode.FirstNode == null)
			{
				string[] array = new string[1];
				string[] array2 = tv_edvFiles.SelectedNode.Text.Split('@');
				array[0] = tb_rootDir.Text + array2[1].TrimStart(' ');
				SendSelectEvent(array, addIt: false);
			}
		}

		private void cb_treeView_CheckedChanged(object sender, EventArgs e)
		{
			ShowTreeOrList(cb_treeView.Checked);
			OptionsEventArgs e2 = new OptionsEventArgs();
			e2.rootDir = tb_rootDir.Text;
			e2.treeSel = cb_treeView.Checked;
			OnOptionsUpdated(e2);
		}

		private void ShowTreeOrList(bool treeView)
		{
			dgv_EDVFiles.Enabled = !treeView;
			dgv_EDVFiles.Visible = !treeView;
			tv_edvFiles.Enabled = treeView;
			tv_edvFiles.Visible = treeView;
			btn_UncheckAll.Visible = treeView;
		}

		private void btn_browseDir_Click(object sender, EventArgs e)
		{
			rootFolderBrowserDialog.SelectedPath = tb_rootDir.Text;
			if (rootFolderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				tb_rootDir.Text = rootFolderBrowserDialog.SelectedPath;
				ShowTreeAndUpdateOptions();
			}
		}

		private void btn_openSel_Click(object sender, EventArgs e)
		{
			MultiSelect(addIt: false);
		}

		private void btn_addSel_Click(object sender, EventArgs e)
		{
			MultiSelect(addIt: true);
		}

		private void MultiSelect(bool addIt)
		{
			string[] array = null;
			int num = 0;
			if (cb_treeView.Checked)
			{
				int num2 = 0;
				string[] array2 = new string[dgv_EDVFiles.Rows.Count];
				int count = tv_edvFiles.Nodes.Count;
				for (int i = 0; i < count; i++)
				{
					bool flag = tv_edvFiles.Nodes[i].Checked;
					int count2 = tv_edvFiles.Nodes[i].Nodes.Count;
					for (int j = 0; j < count2; j++)
					{
						bool flag2 = tv_edvFiles.Nodes[i].Nodes[j].Checked || flag;
						int count3 = tv_edvFiles.Nodes[i].Nodes[j].Nodes.Count;
						for (int k = 0; k < count3; k++)
						{
							if (tv_edvFiles.Nodes[i].Nodes[j].Nodes[k].Checked || flag2)
							{
								string[] array3 = tv_edvFiles.Nodes[i].Nodes[j].Nodes[k].Text.Split('@');
								array2[num2] = tb_rootDir.Text + array3[1].TrimStart(' ');
								num2++;
							}
						}
					}
				}
				array = new string[num2];
				num = num2;
				for (int l = 0; l < num2; l++)
				{
					array[l] = array2[l];
				}
			}
			else
			{
				int rowCount = dgv_EDVFiles.Rows.GetRowCount(DataGridViewElementStates.Selected);
				array = new string[rowCount];
				num = rowCount;
				if (rowCount > 0)
				{
					for (int m = 0; m < rowCount; m++)
					{
						DataGridViewTextBoxCell dataGridViewTextBoxCell = (DataGridViewTextBoxCell)dgv_EDVFiles.SelectedRows[m].Cells["Col_ED_V_Path"];
						array[m] = dataGridViewTextBoxCell.Value.ToString();
					}
				}
			}
			if (array != null && num > 0)
			{
				SendSelectEvent(array, addIt);
			}
		}

		private void btn_CloseForm_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void tb_rootDir_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return && !tb_rootDir.Text.Equals(previousRootDir))
			{
				ShowTreeAndUpdateOptions();
				previousRootDir = tb_rootDir.Text;
			}
		}

		private void ShowTreeAndUpdateOptions()
		{
			ShowTree();
			OptionsEventArgs e = new OptionsEventArgs();
			e.rootDir = tb_rootDir.Text;
			e.treeSel = cb_treeView.Checked;
			OnOptionsUpdated(e);
		}

		private void btn_Reload_Click(object sender, EventArgs e)
		{
			ShowTreeAndUpdateOptions();
		}

		private void btn_UncheckAll_Click(object sender, EventArgs e)
		{
			foreach (TreeNode node in tv_edvFiles.Nodes)
			{
				walkThroughTree(node);
			}
		}

		private void walkThroughTree(TreeNode tNode)
		{
			foreach (TreeNode node in tNode.Nodes)
			{
				walkThroughTree(node);
			}
			tNode.Checked = false;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TDSView.OpenTree));
			this.lbl_nbrOfFoundFiles = new System.Windows.Forms.Label();
			this.dgv_EDVFiles = new System.Windows.Forms.DataGridView();
			this.Col_Project = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Col_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Col_Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Col_ED_V_Path = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tv_edvFiles = new System.Windows.Forms.TreeView();
			this.cb_treeView = new System.Windows.Forms.CheckBox();
			this.tb_rootDir = new System.Windows.Forms.TextBox();
			this.btn_browseDir = new System.Windows.Forms.Button();
			this.rootFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.lbl_RootDirectory = new System.Windows.Forms.Label();
			this.btn_openSel = new System.Windows.Forms.Button();
			this.btn_addSel = new System.Windows.Forms.Button();
			this.btn_CloseForm = new System.Windows.Forms.Button();
			this.btn_UncheckAll = new System.Windows.Forms.Button();
			this.btn_Reload = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)this.dgv_EDVFiles).BeginInit();
			base.SuspendLayout();
			this.lbl_nbrOfFoundFiles.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.lbl_nbrOfFoundFiles.AutoSize = true;
			this.lbl_nbrOfFoundFiles.Location = new System.Drawing.Point(12, 420);
			this.lbl_nbrOfFoundFiles.Name = "lbl_nbrOfFoundFiles";
			this.lbl_nbrOfFoundFiles.Size = new System.Drawing.Size(124, 13);
			this.lbl_nbrOfFoundFiles.TabIndex = 0;
			this.lbl_nbrOfFoundFiles.Text = "Nbr of found ED_V files: ";
			this.dgv_EDVFiles.AllowUserToAddRows = false;
			this.dgv_EDVFiles.AllowUserToDeleteRows = false;
			this.dgv_EDVFiles.AllowUserToResizeRows = false;
			this.dgv_EDVFiles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.dgv_EDVFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgv_EDVFiles.Columns.AddRange(this.Col_Project, this.Col_Id, this.Col_Date, this.Col_ED_V_Path);
			this.dgv_EDVFiles.Location = new System.Drawing.Point(12, 66);
			this.dgv_EDVFiles.Name = "dgv_EDVFiles";
			this.dgv_EDVFiles.ReadOnly = true;
			this.dgv_EDVFiles.RowHeadersVisible = false;
			this.dgv_EDVFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgv_EDVFiles.Size = new System.Drawing.Size(777, 341);
			this.dgv_EDVFiles.TabIndex = 1;
			this.dgv_EDVFiles.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(dgv_EDVFiles_SortCompare);
			this.dgv_EDVFiles.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgv_EDVFiles_CellDoubleClick);
			this.Col_Project.DataPropertyName = "Project";
			this.Col_Project.HeaderText = "Project";
			this.Col_Project.Name = "Col_Project";
			this.Col_Project.ReadOnly = true;
			this.Col_Project.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
			this.Col_Id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.Col_Id.DataPropertyName = "Vehicle";
			this.Col_Id.HeaderText = "Vehicle Id";
			this.Col_Id.Name = "Col_Id";
			this.Col_Id.ReadOnly = true;
			this.Col_Id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
			this.Col_Id.Width = 73;
			this.Col_Date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.Col_Date.DataPropertyName = "Readout Time";
			this.Col_Date.HeaderText = "Read Out Time";
			this.Col_Date.Name = "Col_Date";
			this.Col_Date.ReadOnly = true;
			this.Col_Date.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
			this.Col_Date.Width = 75;
			this.Col_ED_V_Path.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Col_ED_V_Path.DataPropertyName = "ED_V Path";
			dataGridViewCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
			this.Col_ED_V_Path.DefaultCellStyle = dataGridViewCellStyle;
			this.Col_ED_V_Path.HeaderText = "ED_V Path";
			this.Col_ED_V_Path.Name = "Col_ED_V_Path";
			this.Col_ED_V_Path.ReadOnly = true;
			this.Col_ED_V_Path.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
			this.tv_edvFiles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.tv_edvFiles.CheckBoxes = true;
			this.tv_edvFiles.Location = new System.Drawing.Point(12, 66);
			this.tv_edvFiles.Name = "tv_edvFiles";
			this.tv_edvFiles.Size = new System.Drawing.Size(769, 341);
			this.tv_edvFiles.TabIndex = 2;
			this.tv_edvFiles.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(tv_edvFiles_NodeMouseDoubleClick);
			this.cb_treeView.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.cb_treeView.AutoSize = true;
			this.cb_treeView.Location = new System.Drawing.Point(190, 420);
			this.cb_treeView.Name = "cb_treeView";
			this.cb_treeView.Size = new System.Drawing.Size(74, 17);
			this.cb_treeView.TabIndex = 3;
			this.cb_treeView.Text = "Tree View";
			this.cb_treeView.UseVisualStyleBackColor = true;
			this.cb_treeView.CheckedChanged += new System.EventHandler(cb_treeView_CheckedChanged);
			this.tb_rootDir.Location = new System.Drawing.Point(14, 32);
			this.tb_rootDir.Name = "tb_rootDir";
			this.tb_rootDir.Size = new System.Drawing.Size(675, 20);
			this.tb_rootDir.TabIndex = 4;
			this.tb_rootDir.KeyUp += new System.Windows.Forms.KeyEventHandler(tb_rootDir_KeyUp);
			this.btn_browseDir.Location = new System.Drawing.Point(695, 30);
			this.btn_browseDir.Name = "btn_browseDir";
			this.btn_browseDir.Size = new System.Drawing.Size(35, 23);
			this.btn_browseDir.TabIndex = 5;
			this.btn_browseDir.Text = ">>";
			this.btn_browseDir.UseVisualStyleBackColor = true;
			this.btn_browseDir.Click += new System.EventHandler(btn_browseDir_Click);
			this.lbl_RootDirectory.AutoSize = true;
			this.lbl_RootDirectory.Location = new System.Drawing.Point(15, 13);
			this.lbl_RootDirectory.Name = "lbl_RootDirectory";
			this.lbl_RootDirectory.Size = new System.Drawing.Size(75, 13);
			this.lbl_RootDirectory.TabIndex = 6;
			this.lbl_RootDirectory.Text = "Root Directory";
			this.btn_openSel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.btn_openSel.Location = new System.Drawing.Point(408, 418);
			this.btn_openSel.Name = "btn_openSel";
			this.btn_openSel.Size = new System.Drawing.Size(96, 23);
			this.btn_openSel.TabIndex = 7;
			this.btn_openSel.Text = "Open Selected";
			this.btn_openSel.UseVisualStyleBackColor = true;
			this.btn_openSel.Click += new System.EventHandler(btn_openSel_Click);
			this.btn_addSel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.btn_addSel.Location = new System.Drawing.Point(510, 418);
			this.btn_addSel.Name = "btn_addSel";
			this.btn_addSel.Size = new System.Drawing.Size(96, 23);
			this.btn_addSel.TabIndex = 8;
			this.btn_addSel.Text = "Add Selected";
			this.btn_addSel.UseVisualStyleBackColor = true;
			this.btn_addSel.Click += new System.EventHandler(btn_addSel_Click);
			this.btn_CloseForm.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.btn_CloseForm.Location = new System.Drawing.Point(676, 418);
			this.btn_CloseForm.Name = "btn_CloseForm";
			this.btn_CloseForm.Size = new System.Drawing.Size(75, 23);
			this.btn_CloseForm.TabIndex = 9;
			this.btn_CloseForm.Text = "Close";
			this.btn_CloseForm.UseVisualStyleBackColor = true;
			this.btn_CloseForm.Click += new System.EventHandler(btn_CloseForm_Click);
			this.btn_UncheckAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.btn_UncheckAll.Image = TDSView.Properties.Resources.UncheckAll;
			this.btn_UncheckAll.Location = new System.Drawing.Point(269, 418);
			this.btn_UncheckAll.Name = "btn_UncheckAll";
			this.btn_UncheckAll.Size = new System.Drawing.Size(43, 23);
			this.btn_UncheckAll.TabIndex = 11;
			this.btn_UncheckAll.UseVisualStyleBackColor = true;
			this.btn_UncheckAll.Click += new System.EventHandler(btn_UncheckAll_Click);
			this.btn_Reload.Image = TDSView.Properties.Resources.RepeatHS;
			this.btn_Reload.Location = new System.Drawing.Point(743, 30);
			this.btn_Reload.Name = "btn_Reload";
			this.btn_Reload.Size = new System.Drawing.Size(38, 23);
			this.btn_Reload.TabIndex = 10;
			this.btn_Reload.UseVisualStyleBackColor = true;
			this.btn_Reload.Click += new System.EventHandler(btn_Reload_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(801, 451);
			base.Controls.Add(this.btn_UncheckAll);
			base.Controls.Add(this.btn_Reload);
			base.Controls.Add(this.btn_CloseForm);
			base.Controls.Add(this.btn_addSel);
			base.Controls.Add(this.btn_openSel);
			base.Controls.Add(this.lbl_RootDirectory);
			base.Controls.Add(this.btn_browseDir);
			base.Controls.Add(this.tb_rootDir);
			base.Controls.Add(this.cb_treeView);
			base.Controls.Add(this.tv_edvFiles);
			base.Controls.Add(this.dgv_EDVFiles);
			base.Controls.Add(this.lbl_nbrOfFoundFiles);
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "OpenTree";
			this.Text = "OpenTree";
			((System.ComponentModel.ISupportInitialize)this.dgv_EDVFiles).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
