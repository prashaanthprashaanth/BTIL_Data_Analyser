using System;
using System.Collections;
using System.Globalization;

namespace TDSView
{
	public class CompEnvSignals : IComparer
	{
		int IComparer.Compare(object x, object y)
		{
			if (x is ED_D_EnvSignal && y is ED_D_EnvSignal)
			{
				ED_D_EnvSignal eD_D_EnvSignal = (ED_D_EnvSignal)x;
				ED_D_EnvSignal eD_D_EnvSignal2 = (ED_D_EnvSignal)y;
				int num = int.Parse(eD_D_EnvSignal.env_word, NumberStyles.AllowHexSpecifier);
				int num2 = 0;
				if (!eD_D_EnvSignal.env_bit.Equals(""))
				{
					num2 = int.Parse(eD_D_EnvSignal.env_bit, NumberStyles.AllowHexSpecifier);
				}
				int num3 = 0;
				if (eD_D_EnvSignal.env_byte == "H")
				{
					num3 = 8;
				}
				int num4 = num * 16 + num3 + num2;
				num = int.Parse(eD_D_EnvSignal2.env_word, NumberStyles.AllowHexSpecifier);
				num2 = 0;
				if (!eD_D_EnvSignal2.env_bit.Equals(""))
				{
					num2 = int.Parse(eD_D_EnvSignal2.env_bit, NumberStyles.AllowHexSpecifier);
				}
				num3 = 0;
				if (eD_D_EnvSignal2.env_byte == "H")
				{
					num3 = 8;
				}
				int value = num * 16 + num3 + num2;
				return num4.CompareTo(value);
			}
			throw new ArgumentException("object is not a ED_D_EnvSignal Data set");
		}
	}
}
