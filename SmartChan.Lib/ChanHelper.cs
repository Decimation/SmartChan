using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartChan.Lib
{
	static public class ChanHelper
	{
		public static string Norm([CanBeNull] this string s) => string.IsNullOrWhiteSpace(s) ? null : s;
	}
}