using System.Collections;
using System.IO;

namespace TDSView
{
	public class ED_T
	{
		private enum Ed_elem
		{
			nil_e,
			project_e,
			notebook_e,
			repair_e,
			meta_e,
			text_e
		}

		public string fileName;

		public ArrayList project;

		public ArrayList meta;

		public ArrayList text;

		public ArrayList notebook;

		public ArrayList repair;

		public string actualTextScope;

		public string actualTextType;

		public ED_T()
		{
			fileName = "";
			project = new ArrayList();
			meta = new ArrayList();
			text = new ArrayList();
			notebook = new ArrayList();
			repair = new ArrayList();
			actualTextScope = "";
			actualTextType = "";
		}

		public void Clear()
		{
			fileName = "";
			project.Clear();
			notebook.Clear();
			repair.Clear();
			text.Clear();
			meta.Clear();
		}

		public string GetRepairText(string signalName)
		{
			string text = "";
			foreach (ED_T_REPAIR item in repair)
			{
				if (signalName.Equals(item.name))
				{
					if (!item.rep_text.Equals(""))
					{
						text = TVStr.Repair + ": \n" + item.rep_text;
						text = text.Replace("\\n", "\n");
					}
					break;
				}
			}
			foreach (ED_T_TEXT item2 in this.text)
			{
				if (signalName.Equals(item2.name) && !item2.text.Equals(""))
				{
					if (!text.Equals(""))
					{
						text += "\n\n";
					}
					text = text + item2.type.ToUpper() + " :\n";
					text += item2.text;
					text = text.Replace("\\n", "\n");
				}
			}
			if (text.Equals(""))
			{
				text = TVStr.NoRepairTextAvailable;
			}
			return text;
		}

		public int GetTextParams(string inStr)
		{
			int result = 0;
			char[] separator = new char[1] { ' ' };
			char[] separator2 = new char[1] { '=' };
			string[] array = inStr.Split(separator);
			if (array.Length >= 3)
			{
				if (array[1].StartsWith("SCOPE"))
				{
					string[] array2 = array[1].Split(separator2);
					actualTextScope = array2[1];
					actualTextScope = actualTextScope.Replace("\"", "");
				}
				if (array[2].StartsWith("TYPE"))
				{
					string[] array3 = array[2].Split(separator2);
					actualTextType = array3[1];
					actualTextType = actualTextType.Replace("\"", "");
					actualTextType = actualTextType.Replace(">", "");
				}
			}
			return result;
		}

		public int Read_ED_T_File(string fileName)
		{
			int result = 1000;
			int num = 0;
			Ed_elem ed_elem = Ed_elem.nil_e;
			StreamReader streamReader = File.OpenText(fileName);
			this.fileName = fileName;
			string text = "";
			while ((text = streamReader.ReadLine()) != null)
			{
				text = text.Trim();
				if (text.Equals("<PROJECT>"))
				{
					ed_elem = Ed_elem.project_e;
					continue;
				}
				if (text.Equals("<META>"))
				{
					ed_elem = Ed_elem.meta_e;
					continue;
				}
				if (text.Equals("<NOTEBOOK>"))
				{
					ed_elem = Ed_elem.notebook_e;
					continue;
				}
				if (text.Equals("<REPAIR>"))
				{
					ed_elem = Ed_elem.repair_e;
					continue;
				}
				if (text.StartsWith("<TEXT") && text.EndsWith(">"))
				{
					ed_elem = Ed_elem.text_e;
					GetTextParams(text);
					continue;
				}
				if (text.StartsWith("<") && text.EndsWith(">"))
				{
					ed_elem = Ed_elem.nil_e;
					continue;
				}
				switch (ed_elem)
				{
				case Ed_elem.project_e:
				{
					ED_T_PROJECT eD_T_PROJECT = new ED_T_PROJECT();
					eD_T_PROJECT.GetRecord(text);
					project.Add(eD_T_PROJECT);
					num++;
					break;
				}
				case Ed_elem.meta_e:
				{
					ED_T_META eD_T_META = new ED_T_META();
					eD_T_META.GetRecord(text);
					meta.Add(eD_T_META);
					num++;
					break;
				}
				case Ed_elem.text_e:
				{
					ED_T_TEXT eD_T_TEXT = new ED_T_TEXT(actualTextScope, actualTextType);
					eD_T_TEXT.GetRecord(text);
					this.text.Add(eD_T_TEXT);
					num++;
					break;
				}
				case Ed_elem.notebook_e:
				{
					ED_T_NOTEBOOK eD_T_NOTEBOOK = new ED_T_NOTEBOOK();
					eD_T_NOTEBOOK.GetRecord(text);
					notebook.Add(eD_T_NOTEBOOK);
					num++;
					break;
				}
				case Ed_elem.repair_e:
				{
					ED_T_REPAIR eD_T_REPAIR = new ED_T_REPAIR();
					eD_T_REPAIR.GetRecord(text);
					repair.Add(eD_T_REPAIR);
					num++;
					break;
				}
				}
			}
			if (num > 10)
			{
				result = 0;
			}
			return result;
		}
	}
}
