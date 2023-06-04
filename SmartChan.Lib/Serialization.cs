// Read S SmartChan.Lib Serialization.cs
// 2023-06-04 @ 5:39 PM

using System.Reflection;
using Kantan.Model;

namespace SmartChan.Lib;

public class Serialization
{
	public static IEnumerable<KeyValuePair<string, string>> ToKV(object obj)
	{

		/*IMap.ToKeyValues(obj, fieldConv: (info, o) =>
		{
			if (info is PropertyInfo { PropertyType.IsArray: true } or FieldInfo { FieldType.IsArray: true }) {

			}
		})*/

		var kvp = IMap.ToKeyValues(obj);
		return kvp.OfType<KeyValuePair<string, string>>().ToArray();
	}
}