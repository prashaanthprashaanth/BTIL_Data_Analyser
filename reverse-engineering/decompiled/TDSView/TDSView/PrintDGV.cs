using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace TDSView
{
	internal class PrintDGV
	{
		private static StringFormat StrFormat;

		private static StringFormat StrFormatComboBox;

		private static Button CellButton;

		private static CheckBox CellCheckBox;

		private static ComboBox CellComboBox;

		private static int TotalWidth;

		private static int RowPos;

		private static bool NewPage;

		private static int PageNo;

		private static ArrayList ColumnLefts = new ArrayList();

		private static ArrayList ColumnWidths = new ArrayList();

		private static ArrayList ColumnTypes = new ArrayList();

		private static int CellHeight;

		private static int RowsPerPage;

		private static PrintDocument printDoc = new PrintDocument();

		private static string PrintTitle = "";

		private static string printFileName = "";

		private static DataGridView dgv;

		private static List<string> SelectedColumns = new List<string>();

		private static List<string> AvailableColumns = new List<string>();

		private static bool PrintAllRows = true;

		private static bool FitToPageWidth = true;

		private static int HeaderHeight = 0;

		public static void Print_DataGridView(DataGridView dgv1, PrintDialog pDlg, string printTitleStr, string fileName)
		{
			try
			{
				dgv = dgv1;
				AvailableColumns.Clear();
				foreach (DataGridViewColumn column in dgv.Columns)
				{
					if (column.Visible)
					{
						AvailableColumns.Add(column.HeaderText);
					}
				}
				PrintOptions printOptions = new PrintOptions(AvailableColumns);
				if (printOptions.ShowDialog() == DialogResult.OK)
				{
					printFileName = fileName;
					PrintTitle = printTitleStr + "   ";
					PrintTitle += printOptions.PrintTitle;
					PrintAllRows = printOptions.PrintAllRows;
					FitToPageWidth = printOptions.FitToPageWidth;
					SelectedColumns = printOptions.GetSelectedColumns();
					RowsPerPage = 0;
					printDoc.DefaultPageSettings = pDlg.PrinterSettings.DefaultPageSettings;
					printDoc.PrinterSettings = pDlg.PrinterSettings;
					PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
					printPreviewDialog.ShowIcon = false;
					printPreviewDialog.Document = printDoc;
					printDoc.BeginPrint += PrintDoc_BeginPrint;
					printDoc.PrintPage += PrintDoc_PrintPage;
					if (printPreviewDialog.ShowDialog() != DialogResult.OK)
					{
						printDoc.BeginPrint -= PrintDoc_BeginPrint;
						printDoc.PrintPage -= PrintDoc_PrintPage;
					}
					else
					{
						printDoc.Print();
						printDoc.BeginPrint -= PrintDoc_BeginPrint;
						printDoc.PrintPage -= PrintDoc_PrintPage;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private static void PrintDoc_BeginPrint(object sender, PrintEventArgs e)
		{
			try
			{
				StrFormat = new StringFormat();
				StrFormat.Alignment = StringAlignment.Near;
				StrFormat.LineAlignment = StringAlignment.Center;
				StrFormat.Trimming = StringTrimming.EllipsisCharacter;
				StrFormatComboBox = new StringFormat();
				StrFormatComboBox.LineAlignment = StringAlignment.Center;
				StrFormatComboBox.FormatFlags = StringFormatFlags.NoWrap;
				StrFormatComboBox.Trimming = StringTrimming.EllipsisCharacter;
				ColumnLefts.Clear();
				ColumnWidths.Clear();
				ColumnTypes.Clear();
				CellHeight = 0;
				RowsPerPage = 0;
				CellButton = new Button();
				CellCheckBox = new CheckBox();
				CellComboBox = new ComboBox();
				TotalWidth = 0;
				foreach (DataGridViewColumn column in dgv.Columns)
				{
					if (column.Visible && SelectedColumns.Contains(column.HeaderText))
					{
						TotalWidth += column.Width;
					}
				}
				PageNo = 1;
				NewPage = true;
				RowPos = 0;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private static void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
		{
			int num = e.MarginBounds.Top;
			int num2 = e.MarginBounds.Left;
			try
			{
				if (PageNo == 1)
				{
					foreach (DataGridViewColumn column in dgv.Columns)
					{
						if (column.Visible && SelectedColumns.Contains(column.HeaderText))
						{
							int num3 = ((!FitToPageWidth) ? column.Width : ((int)Math.Floor((double)column.Width / (double)TotalWidth * (double)TotalWidth * ((double)e.MarginBounds.Width / (double)TotalWidth))));
							HeaderHeight = (int)e.Graphics.MeasureString(column.HeaderText, column.InheritedStyle.Font, num3).Height + 11;
							ColumnLefts.Add(num2);
							ColumnWidths.Add(num3);
							ColumnTypes.Add(column.GetType());
							num2 += num3;
						}
					}
				}
				while (RowPos <= dgv.Rows.Count - 1)
				{
					DataGridViewRow dataGridViewRow = dgv.Rows[RowPos];
					if (dataGridViewRow.IsNewRow || (!PrintAllRows && !dataGridViewRow.Selected))
					{
						RowPos++;
						continue;
					}
					CellHeight = dataGridViewRow.Height;
					if (num + CellHeight >= e.MarginBounds.Height + e.MarginBounds.Top)
					{
						DrawFooter(e, RowsPerPage);
						NewPage = true;
						PageNo++;
						e.HasMorePages = true;
						return;
					}
					int num4;
					if (NewPage)
					{
						e.Graphics.DrawString(PrintTitle, new Font(dgv.Font, FontStyle.Bold), Brushes.Black, e.MarginBounds.Left, (float)e.MarginBounds.Top - e.Graphics.MeasureString(PrintTitle, new Font(dgv.Font, FontStyle.Bold), e.MarginBounds.Width).Height - 13f);
						string text = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();
						e.Graphics.DrawString(text, new Font(dgv.Font, FontStyle.Bold), Brushes.Black, (float)e.MarginBounds.Left + ((float)e.MarginBounds.Width - e.Graphics.MeasureString(text, new Font(dgv.Font, FontStyle.Bold), e.MarginBounds.Width).Width), (float)e.MarginBounds.Top - e.Graphics.MeasureString(PrintTitle, new Font(new Font(dgv.Font, FontStyle.Bold), FontStyle.Bold), e.MarginBounds.Width).Height - 13f);
						num = e.MarginBounds.Top;
						num4 = 0;
						foreach (DataGridViewColumn column2 in dgv.Columns)
						{
							if (column2.Visible && SelectedColumns.Contains(column2.HeaderText))
							{
								e.Graphics.FillRectangle(new SolidBrush(Color.LightGray), new Rectangle((int)ColumnLefts[num4], num, (int)ColumnWidths[num4], HeaderHeight));
								e.Graphics.DrawRectangle(Pens.Black, new Rectangle((int)ColumnLefts[num4], num, (int)ColumnWidths[num4], HeaderHeight));
								e.Graphics.DrawString(column2.HeaderText, column2.InheritedStyle.Font, new SolidBrush(column2.InheritedStyle.ForeColor), new RectangleF((int)ColumnLefts[num4], num, (int)ColumnWidths[num4], HeaderHeight), StrFormat);
								num4++;
							}
						}
						NewPage = false;
						num += HeaderHeight;
					}
					num4 = 0;
					foreach (DataGridViewCell cell in dataGridViewRow.Cells)
					{
						if (cell.OwningColumn.Visible && SelectedColumns.Contains(cell.OwningColumn.HeaderText))
						{
							if (((Type)ColumnTypes[num4]).Name == "DataGridViewTextBoxColumn" || ((Type)ColumnTypes[num4]).Name == "DataGridViewLinkColumn")
							{
								e.Graphics.DrawString(cell.Value.ToString(), cell.InheritedStyle.Font, new SolidBrush(cell.InheritedStyle.ForeColor), new RectangleF((int)ColumnLefts[num4], num, (int)ColumnWidths[num4], CellHeight), StrFormat);
							}
							else if (((Type)ColumnTypes[num4]).Name == "DataGridViewButtonColumn")
							{
								CellButton.Text = cell.Value.ToString();
								CellButton.Size = new Size((int)ColumnWidths[num4], CellHeight);
								Bitmap bitmap = new Bitmap(CellButton.Width, CellButton.Height);
								CellButton.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
								e.Graphics.DrawImage(bitmap, new Point((int)ColumnLefts[num4], num));
							}
							else if (((Type)ColumnTypes[num4]).Name == "DataGridViewCheckBoxColumn")
							{
								CellCheckBox.Size = new Size(14, 14);
								CellCheckBox.Checked = (bool)cell.Value;
								Bitmap bitmap2 = new Bitmap((int)ColumnWidths[num4], CellHeight);
								Graphics graphics = Graphics.FromImage(bitmap2);
								graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, bitmap2.Width, bitmap2.Height));
								CellCheckBox.DrawToBitmap(bitmap2, new Rectangle((bitmap2.Width - CellCheckBox.Width) / 2, (bitmap2.Height - CellCheckBox.Height) / 2, CellCheckBox.Width, CellCheckBox.Height));
								e.Graphics.DrawImage(bitmap2, new Point((int)ColumnLefts[num4], num));
							}
							else if (((Type)ColumnTypes[num4]).Name == "DataGridViewComboBoxColumn")
							{
								CellComboBox.Size = new Size((int)ColumnWidths[num4], CellHeight);
								Bitmap bitmap3 = new Bitmap(CellComboBox.Width, CellComboBox.Height);
								CellComboBox.DrawToBitmap(bitmap3, new Rectangle(0, 0, bitmap3.Width, bitmap3.Height));
								e.Graphics.DrawImage(bitmap3, new Point((int)ColumnLefts[num4], num));
								e.Graphics.DrawString(cell.Value.ToString(), cell.InheritedStyle.Font, new SolidBrush(cell.InheritedStyle.ForeColor), new RectangleF((int)ColumnLefts[num4] + 1, num, (int)ColumnWidths[num4] - 16, CellHeight), StrFormatComboBox);
							}
							else if (((Type)ColumnTypes[num4]).Name == "DataGridViewImageColumn")
							{
								Rectangle rectangle = new Rectangle((int)ColumnLefts[num4], num, (int)ColumnWidths[num4], CellHeight);
								Size size = ((Image)cell.FormattedValue).Size;
								e.Graphics.DrawImage((Image)cell.FormattedValue, new Rectangle((int)ColumnLefts[num4] + (rectangle.Width - size.Width) / 2, num + (rectangle.Height - size.Height) / 2, ((Image)cell.FormattedValue).Width, ((Image)cell.FormattedValue).Height));
							}
							e.Graphics.DrawRectangle(Pens.Black, new Rectangle((int)ColumnLefts[num4], num, (int)ColumnWidths[num4], CellHeight));
							num4++;
						}
					}
					num += CellHeight;
					RowPos++;
					if (PageNo == 1)
					{
						RowsPerPage++;
					}
				}
				if (RowsPerPage != 0)
				{
					DrawFooter(e, RowsPerPage);
					e.HasMorePages = false;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, TVStr.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private static void DrawFooter(PrintPageEventArgs e, int RowsPerPage)
		{
			double num = 0.0;
			string text = string.Concat(str2: Math.Ceiling(((!PrintAllRows) ? ((double)dgv.SelectedRows.Count) : ((!dgv.Rows[dgv.Rows.Count - 1].IsNewRow) ? ((double)(dgv.Rows.Count - 1)) : ((double)(dgv.Rows.Count - 2)))) / (double)RowsPerPage).ToString(), str0: PageNo.ToString(), str1: " of ");
			e.Graphics.DrawString(text, dgv.Font, Brushes.Black, (float)e.MarginBounds.Left + ((float)e.MarginBounds.Width - e.Graphics.MeasureString(text, dgv.Font, e.MarginBounds.Width).Width) / 2f, e.MarginBounds.Top + e.MarginBounds.Height + 31);
			e.Graphics.DrawString(printFileName, dgv.Font, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top + e.MarginBounds.Height + 31);
		}
	}
}
