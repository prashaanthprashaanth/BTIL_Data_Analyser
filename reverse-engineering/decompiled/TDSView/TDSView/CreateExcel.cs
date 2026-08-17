using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace TDSView
{
	public class CreateExcel
	{
		public static int CreateExcelFile(string fileName, DataGridView dgV)
		{
			Workbook workbook = null;
			Worksheet worksheet = null;
			Range range = null;
			object value = Missing.Value;
			try
			{
				ApplicationClass applicationClass = new ApplicationClass();
				workbook = applicationClass.Application.Workbooks.Add(Type.Missing);
				worksheet = (Worksheet)workbook.Sheets[1];
				int count = workbook.Sheets.Count;
				for (int i = 1; i < count; i++)
				{
					((Worksheet)workbook.Sheets[2]).Delete();
				}
				applicationClass.Columns.ColumnWidth = 30;
				int num = 1;
				foreach (DataGridViewColumn column in dgV.Columns)
				{
					applicationClass.Cells[1, num++] = column.HeaderText;
				}
				char c = (char)(dgV.Columns.Count - 1);
				c = (char)(c + 65);
				int inx = dgV.Columns.Count - 1;
				string text = ColInxToColId(inx);
				DataGridViewSelectedRowCollection selectedRows = dgV.SelectedRows;
				bool flag = selectedRows.Count > 1;
				if (flag)
				{
					DialogResult dialogResult = MessageBox.Show(TVStr.SelectionExportToExcel, TVStr.MultipleLinesSelected, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (dialogResult == DialogResult.No)
					{
						flag = false;
					}
				}
				int count2 = dgV.Rows[0].Cells.Count;
				int num2 = ((!flag) ? (dgV.Rows.Count + 1) : (selectedRows.Count + 1));
				object[,] array = new object[num2, count2];
				int num3 = 0;
				for (int j = 0; j < dgV.Rows.Count; j++)
				{
					DataGridViewRow dataGridViewRow = dgV.Rows[j];
					if ((!flag || !dataGridViewRow.Selected) && flag)
					{
						continue;
					}
					range = ((_Worksheet)worksheet).get_Range((object)("A" + (num3 + 2)), (object)(text + (num3 + 2)));
					range.NumberFormat = "@";
					range.HorizontalAlignment = XlHAlign.xlHAlignLeft;
					for (int k = 0; k < dataGridViewRow.Cells.Count; k++)
					{
						if (dataGridViewRow.Cells[k].Value != null)
						{
							array[num3, k] = dataGridViewRow.Cells[k].Value.ToString();
						}
					}
					num3++;
				}
				Range cell = (Range)worksheet.Cells[2, 1];
				Range cell2 = (Range)worksheet.Cells[num2, count2];
				Range range2 = ((_Worksheet)worksheet).get_Range((object)cell, (object)cell2);
				range2.Value2 = array;
				range = ((_Worksheet)worksheet).get_Range((object)"A1", (object)(text + "1"));
				range.Interior.Color = Color.Yellow.ToArgb();
				range.Font.Bold = true;
				worksheet.Application.ActiveWindow.SplitRow = 1;
				worksheet.Application.ActiveWindow.FreezePanes = true;
				Range range3 = ((_Worksheet)worksheet).get_Range((object)"A1", (object)"AK1");
				range3.Activate();
				range3.Select();
				range3.AutoFilter(1, Type.Missing, XlAutoFilterOperator.xlAnd, Type.Missing, true);
				worksheet.Name = "Event List";
				applicationClass.Columns.AutoFit();
				applicationClass.ActiveWorkbook.SaveCopyAs(fileName);
				applicationClass.ActiveWorkbook.Saved = true;
				applicationClass.ActiveWorkbook.Close(true, value, value);
				applicationClass.Quit();
				releaseObject(worksheet);
				releaseObject(workbook);
				releaseObject(applicationClass);
			}
			catch (Exception ex)
			{
				MessageBox.Show(TVStr.CouldNotCreateExcel, TVStr.ExcelCreationError + "\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			return 0;
		}

		private static string ColInxToColId(int inx)
		{
			string text = "";
			int num = inx % 25 - 1;
			int num2 = inx / 25;
			char c = (char)(num + 65);
			if (num2 > 0)
			{
				text = ((char)(num2 - 1 + 65)).ToString();
			}
			return text + c;
		}

		private static void releaseObject(object obj)
		{
			try
			{
				Marshal.ReleaseComObject(obj);
				obj = null;
			}
			catch (Exception ex)
			{
				obj = null;
				MessageBox.Show(TVStr.ExceptionOccured + " " + ex.ToString());
			}
			finally
			{
				GC.Collect();
			}
		}
	}
}
