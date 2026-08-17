using System.Collections.Generic;

namespace TDSView
{
	public class EventComparer : IComparer<RankElement>
	{
		public int Compare(RankElement y, RankElement x)
		{
			if (x == null)
			{
				if (y == null)
				{
					return 0;
				}
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			int num = x.event_cnt.CompareTo(y.event_cnt);
			if (num != 0)
			{
				return num;
			}
			return x.event_cnt.CompareTo(y.event_cnt);
		}
	}
}
