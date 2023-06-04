using System.Runtime.CompilerServices;
using Flurl;
using Flurl.Http;

[assembly: InternalsVisibleTo("SmartChan")]

namespace SmartChan.Lib;

public abstract class BaseChanArchive : IDisposable
{
	public abstract Url BaseUrl { get; }

	protected abstract Task<IFlurlResponse> SearchInitialAsync(SearchQuery query);

	protected abstract Task<IEnumerable<ChanPost>> ParseAsync(IFlurlResponse res);

	public abstract Task<IEnumerable<ChanPost>> SearchAsync(SearchQuery q);

	public void Dispose() { }
}