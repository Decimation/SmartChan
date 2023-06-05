// Read S SmartChan.Lib ArchiveOfSins.cs
// 2023-06-04 @ 8:10 PM

using Flurl;
using Flurl.Http;
using SmartChan.Lib.Archives.Base;

namespace SmartChan.Lib.Archives;

public sealed class ArchiveOfSinsEngine : BaseFoolFuukaArchiveEngine
{
	public override Url BaseUrl => "https://archiveofsins.com/";

	public override string Name => "Archive of Sins";

	protected override async Task<IFlurlResponse> SearchInitialAsync(SearchQuery query)
	{
		return await base.SearchInitialAsync(query);
	}

	public override async Task<ChanPost[]> SearchAsync(SearchQuery q)
	{
		return await base.SearchAsync(q);
	}

	protected override async Task<IEnumerable<ChanPost>> ParseAsync(IFlurlResponse r, CancellationToken ct = default)
	{
		return await base.ParseAsync(r, ct);
	}

}