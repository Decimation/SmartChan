using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartChan.Lib.Archives;

namespace SmartChan.Lib.Utilities;

public static class ChanHelper
{
	public static string Norm([CBN] this string s) => string.IsNullOrWhiteSpace(s) ? null : s;

	internal const string BI_R   = "r_";
	internal const string BI_WLD = "wld_";
	internal const string BI_CMB = "cmb_";
	internal const string BI_S   = "s_";

	public static string GetBoardString(BoardIdentifier b)
	{
		b &= ~BoardIdentifier.s_None;

		var name = b.ToString().
			Replace(ChanHelper.BI_R, String.Empty)
			.Replace(BI_CMB, String.Empty)
			.Replace(BoardIdentifier.wld_Any.ToString(), "_");

		return name;
	}

}