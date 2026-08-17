using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using CLI;
using Microsoft.Office.Interop.Excel;

namespace Tsv2Excel
{
	internal class Program
	{
		private static int Main(string[] args)
		{
			int result = 0;
			global::CLI.CLI cLI = new global::CLI.CLI(args, new string[0], 1, 2);
			string[] array = cLI.CheckCommmandLine();
			try
			{
				string text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
				Console.WriteLine("TSV to Excel converter " + text + " Bombardier Transportation TTAB, by Bos");
				Console.WriteLine();
				if (cLI.IsHelpOption())
				{
					WriteHelp();
				}
				else if (array != null)
				{
					string[] array2 = array;
					foreach (string value in array2)
					{
						Console.WriteLine(value);
					}
					Console.WriteLine("Check your commandline");
					Console.WriteLine();
					WriteHelp();
				}
				else if (cLI.numberOfArgs < 1)
				{
					Console.WriteLine("ERROR: No TSV-Filename given");
					result = 1;
				}
				else
				{
					string attr = "";
					bool flag = false;
					cLI.GetArgument(1, out var attr2);
					if (cLI.numberOfArgs > 1)
					{
						cLI.GetArgument(2, out attr);
						flag = !Directory.Exists(attr);
					}
					if (!flag)
					{
						if (File.Exists(attr2))
						{
							string text2 = ((!attr.Equals("")) ? Path.Combine(attr, Path.GetFileNameWithoutExtension(attr2) + ".xlsx") : Path.Combine(Path.GetDirectoryName(attr2), Path.GetFileNameWithoutExtension(attr2) + ".xlsx"));
							object value2 = Missing.Value;
							Workbook workbook = null;
							Worksheet worksheet = null;
							ApplicationClass applicationClass = new ApplicationClass();
							workbook = applicationClass.Application.Workbooks.Add(Type.Missing);
							worksheet = (Worksheet)workbook.Sheets[1];
							int[] array3 = new int[1000];
							for (int j = 0; j < 1000; j++)
							{
								array3[j] = 2;
							}
							ImportCSV(attr2, (Worksheet)workbook.Worksheets[1], ((_Worksheet)(Worksheet)workbook.Worksheets[1]).get_Range((object)"A1", (object)"B2"), array3, autoFitColumns: true);
							worksheet.Name = "Event List";
							applicationClass.Columns.AutoFit();
							applicationClass.ActiveWorkbook.SaveCopyAs(text2);
							applicationClass.ActiveWorkbook.Saved = true;
							applicationClass.ActiveWorkbook.Close(true, value2, value2);
							applicationClass.Quit();
							releaseObject(worksheet);
							releaseObject(workbook);
							releaseObject(applicationClass);
							Console.WriteLine("Excel file " + text2 + " created");
						}
						else
						{
							Console.WriteLine("ERROR: File " + attr2 + " doesn't exist");
							result = 2;
						}
					}
					else
					{
						Console.WriteLine("ERROR: Invalid directory path: " + attr);
						result = 3;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR: " + ex.ToString());
				result = 4;
			}
			return result;
		}

		public static void ImportCSV(string importFileName, Worksheet destinationSheet, Range destinationRange, int[] columnDataTypes, bool autoFitColumns)
		{
			destinationSheet.QueryTables.Add("TEXT;" + Path.GetFullPath(importFileName), destinationRange, Type.Missing);
			destinationSheet.QueryTables[1].TextFilePlatform = 2;
			destinationSheet.QueryTables[1].Name = Path.GetFileNameWithoutExtension(importFileName);
			destinationSheet.QueryTables[1].FieldNames = true;
			destinationSheet.QueryTables[1].RowNumbers = false;
			destinationSheet.QueryTables[1].FillAdjacentFormulas = false;
			destinationSheet.QueryTables[1].PreserveFormatting = true;
			destinationSheet.QueryTables[1].RefreshOnFileOpen = false;
			destinationSheet.QueryTables[1].RefreshStyle = XlCellInsertionMode.xlInsertDeleteCells;
			destinationSheet.QueryTables[1].SavePassword = false;
			destinationSheet.QueryTables[1].SaveData = true;
			destinationSheet.QueryTables[1].AdjustColumnWidth = true;
			destinationSheet.QueryTables[1].RefreshPeriod = 0;
			destinationSheet.QueryTables[1].TextFilePromptOnRefresh = false;
			destinationSheet.QueryTables[1].TextFileStartRow = 1;
			destinationSheet.QueryTables[1].TextFileParseType = XlTextParsingType.xlDelimited;
			destinationSheet.QueryTables[1].TextFileTextQualifier = XlTextQualifier.xlTextQualifierDoubleQuote;
			destinationSheet.QueryTables[1].TextFileConsecutiveDelimiter = false;
			destinationSheet.QueryTables[1].TextFileTabDelimiter = true;
			destinationSheet.QueryTables[1].TextFileSemicolonDelimiter = false;
			destinationSheet.QueryTables[1].TextFileCommaDelimiter = false;
			destinationSheet.QueryTables[1].TextFileSpaceDelimiter = false;
			destinationSheet.QueryTables[1].TextFileColumnDataTypes = columnDataTypes;
			Console.Write("Importing data...");
			destinationSheet.QueryTables[1].Refresh(false);
			if (autoFitColumns)
			{
				destinationSheet.QueryTables[1].Destination.EntireColumn.AutoFit();
			}
		}

		private static void WriteHelp()
		{
			Console.WriteLine("TSV2EXCEL pathToTsvFile [destinationFolderForExcelOutput]");
			Console.WriteLine();
			Console.WriteLine("Example: TSVEXCEL D:\\DIAGFILES\\ED_V_BR430____20131107_018_A._046");
			Console.WriteLine("         --> the output will be: D:\\DIAGFILES\\ED_V_BR430____20131107_018_A.xlsx");
			Console.WriteLine();
			Console.WriteLine("         TSVEXCEL D:\\DIAGFILES\\ED_V_BR430____20131107_018_A._046 D:\\DIAGOUTPUT");
			Console.WriteLine("         --> the output will be: D:\\DIAGOUTPUT\\ED_V_BR430____20131107_018_A.xlsx");
			Console.WriteLine();
			Console.WriteLine("Press any key...");
			Console.ReadKey();
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
				Console.WriteLine("Error: " + ex.ToString());
			}
			finally
			{
				GC.Collect();
			}
		}
	}
}
