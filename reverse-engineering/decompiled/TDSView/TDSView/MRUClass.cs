using System;
using System.Collections.Specialized;

namespace TDSView
{
	public class MRUClass : StringCollection
	{
		private const int MRUnumber = 10;

		public void Push(string filePath)
		{
			if (Contains(filePath))
			{
				int index = IndexOf(filePath);
				RemoveAt(index);
			}
			Insert(0, filePath);
			int count = base.Count;
			if (count > 10)
			{
				RemoveAt(count - 1);
			}
		}

		public void TakeSettings(StringCollection col)
		{
			if (col == null)
			{
				return;
			}
			StringEnumerator enumerator = col.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					Add(current);
				}
			}
			finally
			{
				if (enumerator is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}
		}
	}
}
