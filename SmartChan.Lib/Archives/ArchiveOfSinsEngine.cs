// Read S SmartChan.Lib ArchiveOfSins.cs
// 2023-06-04 @ 8:10 PM

using Flurl;
using Flurl.Http;
using SmartChan.Lib.Archives.Base;
using SmartChan.Lib.Model;

namespace SmartChan.Lib.Archives;

public sealed class ArchiveOfSinsEngine : BaseFoolFuukaEngine
{

	public override Url BaseUrl => "https://archiveofsins.com/";

	public override string Name => "Archive of Sins";

	public override BoardIdentifier Boards => BoardIdentifier.cmb_ArchiveOfSins;

}