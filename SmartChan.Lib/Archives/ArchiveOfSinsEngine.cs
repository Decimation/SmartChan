// Read S SmartChan.Lib ArchiveOfSins.cs
// 2023-06-04 @ 8:10 PM

using Flurl;
using Flurl.Http;
using SmartChan.Lib.Archives.Base;

namespace SmartChan.Lib.Archives;

public sealed class ArchiveOfSinsEngine : BaseFoolFuukaEngine
{
	public override Url BaseUrl => "https://archiveofsins.com/";

	public override string Name => "Archive of Sins";

	protected override async Task<IFlurlResponse> GetInitialResponseAsync(SearchQuery query)
	{
		return await base.GetInitialResponseAsync(query);
	}

	public override async Task<ChanPost[]> RunSearchAsync(SearchQuery q)
	{
		return await base.RunSearchAsync(q);
	}

	protected override async Task<IEnumerable<ChanPost>> ParseResponseAsync(IFlurlResponse r, CancellationToken ct = default)
	{
		return await base.ParseResponseAsync(r, ct);
	}

}