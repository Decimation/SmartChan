// Read S SmartChan.Lib ArchivedMoe.cs
// 2023-06-04 @ 8:10 PM

using Flurl;
using SmartChan.Lib.Archives.Base;
using SmartChan.Lib.Model;

namespace SmartChan.Lib.Archives;

public sealed class ArchivedMoeEngine : BaseFoolFuukaEngine
{

	public override Url BaseUrl => "https://archived.moe/";

	public override string Name => "Archived.Moe";

	public override ChanBoardId Boards => ChanBoardId.cmb_ArchivedMoe;

}