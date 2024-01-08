// Read S SmartChan.Lib ArchivedMoe.cs
// 2023-06-04 @ 8:10 PM

using Flurl;
using SmartChan.Lib.Archives.Base;

namespace SmartChan.Lib.Archives;

public sealed class ArchivedMoeEngine : BaseFoolFuukaEngine
{
	public override Url BaseUrl => "https://archived.moe/";

	public override string Name => "Archived.Moe";
}