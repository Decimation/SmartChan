using System.Runtime.CompilerServices;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Novus;

[assembly: InternalsVisibleTo("SmartChan")]

namespace SmartChan.Lib.Archives.Base;

internal static class LogUtil
{
    internal static readonly ILoggerFactory Factory =
        LoggerFactory.Create(builder => builder.AddDebug().SetMinimumLevel(LogLevel.Trace));
}

public abstract class BaseArchiveEngine : IDisposable
{
    [ModuleInitializer]
    public static void Init()
    {
        Global.Setup();
    }

    protected internal static readonly ILogger Logger = LogUtil.Factory.CreateLogger(nameof(BaseArchiveEngine));

    public abstract Url BaseUrl { get; }
    public abstract Url SearchUrl { get; }

    protected       CookieJar Cookies { get; }
    public abstract string    Name    { get; }

    protected abstract Task<IFlurlResponse> SearchInitialAsync(SearchQuery query);

    protected abstract Task<IEnumerable<ChanPost>> ParseAsync(IFlurlResponse res, CancellationToken ct = default);

    public abstract Task<ChanPost[]> SearchAsync(SearchQuery q);

    public void Dispose() { }

    protected BaseArchiveEngine()
    {
	    Cookies = new CookieJar();
    }

    protected static FlurlClient Client { get; }

    static BaseArchiveEngine()
    {
        FlurlHttp.Configure(settings =>
        {
            settings.Redirects.Enabled = true; // default true
            settings.Redirects.AllowSecureToInsecure = true; // default false
            settings.Redirects.ForwardAuthorizationHeader = true; // default false
            settings.Redirects.MaxAutoRedirects = 20;   // default 10 (consecutive)

        });

        Client = new FlurlClient()
        { };

    }

    #region

    public delegate void OnPostCallback(object s, ChanPost p);

    public event OnPostCallback OnPost;

    protected virtual void OnPostInvocation(ChanPost p)
    {
        OnPost?.Invoke(this, p);
    }

    #endregion
}