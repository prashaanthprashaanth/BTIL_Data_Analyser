using System;
using System.Collections;

namespace TDSView
{
	public class CompStartTime : IComparer
	{
		int IComparer.Compare(object x, object y)
		{
			if (x is ED_V_TimeListEntry && y is ED_V_TimeListEntry)
			{
				ED_V_TimeListEntry eD_V_TimeListEntry = (ED_V_TimeListEntry)x;
				ED_V_TimeListEntry eD_V_TimeListEntry2 = (ED_V_TimeListEntry)y;
				return eD_V_TimeListEntry.time.CompareTo(eD_V_TimeListEntry2.time);
			}
			throw new ArgumentException("object is not a ED_V_TimeListEntry Data set");
		}
	}
}
