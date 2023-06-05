using System.Runtime.CompilerServices;
using Flurl;
using Flurl.Http;
using Novus;

[assembly: InternalsVisibleTo("SmartChan")]

namespace SmartChan.Lib;

public abstract class BaseChanArchive : IDisposable
{
	[ModuleInitializer]
	public static void Init()
	{
		Global.Setup();
	}

	public abstract Url BaseUrl { get; }

	protected abstract Task<IFlurlResponse> SearchInitialAsync(SearchQuery query);

	protected abstract Task<IEnumerable<ChanPost>> ParseAsync(IFlurlResponse res, CancellationToken ct = default);

	public abstract Task<ChanPost[]> SearchAsync(SearchQuery q);

	public void Dispose() { }

	public delegate void OnPost(object s,ChanPost p);

	public event OnPost Result;

	protected BaseChanArchive()
	{
	}

	protected static FlurlClient Client { get; }

	static BaseChanArchive()
	{
		FlurlHttp.Configure(settings =>
		{
			settings.Redirects.Enabled                    = true; // default true
			settings.Redirects.AllowSecureToInsecure      = true; // default false
			settings.Redirects.ForwardAuthorizationHeader = true; // default false
			settings.Redirects.MaxAutoRedirects           = 20;   // default 10 (consecutive)

		});

		Client = new FlurlClient()
			{ };
	}
}