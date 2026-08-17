using System;
using System.Collections;
using System.IO;
using System.Text;

namespace TDSView
{
	public class ED
	{
		public ArrayList edvl;

		public ArrayList eddl;

		public ArrayList edtl;

		public ArrayList selectionList;

		public DateTime stsntStartTime;

		public DateTime stsntEndTime;

		public ArrayList packageList;

		public ED()
		{
			edvl = new ArrayList();
			eddl = new ArrayList();
			edtl = new ArrayList();
			selectionList = new ArrayList();
			packageList = new ArrayList();
		}

		public void Clear()
		{
			for (int count = edvl.Count; count > 0; count = edvl.Count)
			{
				edvl.RemoveAt(0);
			}
			for (int count = eddl.Count; count > 0; count = eddl.Count)
			{
				eddl.RemoveAt(0);
			}
			for (int count = edtl.Count; count > 0; count = edtl.Count)
			{
				edtl.RemoveAt(0);
			}
			selectionList.Clear();
		}

		public bool ED_V_FileIsAlreadyRead(string fileName)
		{
			bool result = false;
			foreach (ED_V item in edvl)
			{
				if (item.fileName.Equals(fileName))
				{
					result = true;
				}
			}
			return result;
		}

		public ED_D ED_D_FileIsAlreadyRead(string fileName)
		{
			ED_D result = null;
			string fileName2 = Path.GetFileName(fileName);
			foreach (ED_D item in eddl)
			{
				string fileName3 = Path.GetFileName(item.fileName);
				if (fileName3.Equals(fileName2))
				{
					result = item;
				}
			}
			return result;
		}

		public ED_T Get_ED_T_Data(string fileName)
		{
			ED_T result = null;
			string fileName2 = Path.GetFileName(fileName);
			foreach (ED_T item in edtl)
			{
				string fileName3 = Path.GetFileName(item.fileName);
				if (fileName3.Equals(fileName2))
				{
					result = item;
				}
			}
			return result;
		}

		public int SavePackage(string fileName)
		{
			Stream stream = File.Create(fileName);
			if (stream != null)
			{
				StreamWriter streamWriter = new StreamWriter(stream, Encoding.ASCII);
				streamWriter.WriteLine("$TDSVIEW_PACKAGE_FILE_1_0_0_0");
				foreach (ED_V item in edvl)
				{
					streamWriter.WriteLine("FILE_V=" + item.fileName);
					streamWriter.WriteLine("FILE_D=" + item.ed_d.fileName);
					if (item.ed_t == null)
					{
						streamWriter.WriteLine("FILE_T= ");
					}
					else
					{
						streamWriter.WriteLine("FILE_T=" + item.ed_t.fileName);
					}
				}
				streamWriter.Close();
			}
			return 0;
		}

		public TDSV_RetVal OpenPackage(string fileName)
		{
			TDSV_RetVal tDSV_RetVal = new TDSV_RetVal();
			tDSV_RetVal.rV = TDSV_RetVal.ReturnValue.OK;
			tDSV_RetVal.str = "";
			packageList.Clear();
			StreamReader streamReader = File.OpenText(fileName);
			string text = "";
			char[] separator = new char[1] { '=' };
			ed_package ed_package2 = new ed_package();
			if ((text = streamReader.ReadLine()) != null && text.Equals("$TDSVIEW_PACKAGE_FILE_1_0_0_0"))
			{
				while ((text = streamReader.ReadLine()) != null)
				{
					text = text.Trim();
					string[] array = text.Split(separator);
					if (array[0].Equals("FILE_V"))
					{
						ed_package2.ed_v_file = array[1];
						if (!File.Exists(ed_package2.ed_v_file))
						{
							packageList.Clear();
							tDSV_RetVal.rV = TDSV_RetVal.ReturnValue.FILE_NOT_FOUND;
							tDSV_RetVal.str = array[1];
						}
					}
					else if (array[0].Equals("FILE_D"))
					{
						ed_package2.ed_d_file = array[1];
					}
					else if (array[0].Equals("FILE_T"))
					{
						ed_package2.ed_t_file = array[1];
						packageList.Add(ed_package2);
						ed_package2 = new ed_package();
					}
				}
			}
			streamReader.Close();
			return tDSV_RetVal;
		}
	}
}
