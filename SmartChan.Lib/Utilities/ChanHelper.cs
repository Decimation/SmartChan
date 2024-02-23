using Kantan.Text;
using SmartChan.Lib.Model;

namespace SmartChan.Lib.Utilities;

public static class ChanHelper
{

	public static string Normalize([CBN] this string s)
		=> Strings.NormalizeNull(s);

	internal const string BI_R   = "r_";
	internal const string BI_WLD = "wld_";
	internal const string BI_CMB = "cmb_";
	internal const string BI_S   = "s_";

	internal const string BI_WLD_PARAM = "*";
	internal const string BI_WLD_QUERY = "_";

	public static string GetBoardQueryString(ChanBoardId b)
	{
		b &= ~ChanBoardId.s_None;

		var name = b.ToString()
			.Replace(BI_R, String.Empty)
			.Replace(BI_CMB, String.Empty)
			.Replace(ChanBoardId.wld_Any.ToString(), BI_WLD_QUERY);

		return name;
	}

}