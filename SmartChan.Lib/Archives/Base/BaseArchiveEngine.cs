using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Runtime.CompilerServices;
using AngleSharp.Dom;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using SmartChan.Lib.Model;
using SmartChan.Lib.Utilities;
using Url = Flurl.Url;

[assembly: InternalsVisibleTo("SmartChan")]

namespace SmartChan.Lib.Archives.Base;

public readonly struct ParseProgress
{

	public long Parsed { get; init; }

	public long Pages { get; init; }

	public ParseProgress(long parsed, long pages)
	{
		Parsed = parsed;
		Pages  = pages;
	}

	public override string ToString()
	{
		return $"{Parsed}/{Pages}";
	}

}

public abstract class BaseArchiveEngine : IDisposable
{

	protected static readonly FlurlClient Client;

	static BaseArchiveEngine()
	{
		Client = new()
		{
			Settings =
			{
				Redirects =
				{
					Enabled                    = true, // default true
					AllowSecureToInsecure      = true, // default false
					ForwardAuthorizationHeader = true, // default false
					MaxAutoRedirects           = 20,   // default 10 (consecutive)

				}
			}
		};
	}

	public abstract ChanBoardId Boards { get; }

	protected internal static readonly ILogger Logger = LogUtil.Factory.CreateLogger(nameof(BaseArchiveEngine));

	public abstract Url BaseUrl { get; }

	public abstract Url SearchUrl { get; }

	protected CookieJar Cookies { get; }

	public TimeSpan Timeout { get; protected set; } = TimeSpan.FromSeconds(20);

	public abstract string Name { get; }

	public IProgress<ParseProgress> Progress { get; set; }

	protected virtual ChanBoardId ParseBoards(string[] boards)
	{
		var b = ChanBoardId.s_None;

		foreach (string board in boards) {

			var boardLower = board.ToLower();

			if (boardLower == ChanHelper.BI_WLD_PARAM) {
				b = ChanBoardId.wld_Any;
				break;
			}

			var i = Enum.Parse<ChanBoardId>(boardLower);

			if (!Boards.HasFlag(i)) { }
			else {
				b |= i;
			}
		}

		return b;
	}

	protected virtual IFlurlRequest BuildInitialRequest(SearchQuery query)
	{

		var request = Client.Request(SearchUrl)
			.WithTimeout(Timeout)
			.WithCookies(Cookies)
			.OnError(er =>
			{
				er.ExceptionHandled = true;
				Trace.WriteLine($"{Name}: {er.Exception.Message}");
				Debugger.Break();
			});

		return request;
	}

	protected abstract Task<IFlurlResponse> GetInitialResponseAsync(SearchQuery query);

	protected abstract Task<ChanResult> ParseResponseAsync(IFlurlResponse res, CancellationToken ct = default);

	public abstract Task<ChanResult> RunSearchAsync(SearchQuery q);

	protected virtual async ValueTask<ChanPost> ParseBodyAsync(IElement elem, CancellationToken a)
	{
		// todo...

		// var (ce, ce2) = ce3;
		if (!elem.TagName.Contains("article", StringComparison.InvariantCultureIgnoreCase)) {
			return null;
		}

		IElement post_files  = null;
		var      post_files1 = elem.GetElementsByClassName("post_file");

		if (post_files1 is { Length: > 0 } && post_files1[0] is { ChildElementCount: > 0 }) {
			post_files = post_files1[0].Children[0];
		}

		// var      header           = post_wrapper.Children[2];
		var post_data = elem.GetElementsByClassName("post_data");

		// var post_data        = header.Children[0];
		// var post_poster_data = post_wrapper.Children[3];
		// var title        = elem.QuerySelector(".post_title");
		// var post_wrapper = ce2.Children[1];
		var title = post_data[0].GetElementsByClassName("post_title")[0];

		// var title = post_wrapper.Children[2].Children[0].Children[2];
		var pd    = elem.QuerySelector(".post_data");
		var allA  = pd.GetElementsByTagName("a");
		var links = new List<string>();
		Url ml    = null;

		foreach (IElement element in allA) {
			var o = element.GetAttribute("href");

			var dp = element.GetAttribute("data-post");
			var df = element.GetAttribute("data-function");

			if (ml == null && dp != null && df == "quote") {
				ml = o;
			}

			if (o != null) {
				links.Add(o);
			}
		}

		// var link = elem.QuerySelector("a")?.GetAttribute("href");

		// var author = post_poster_data.Children[0];
		var author = elem.QuerySelector(".post_author");

		// var authorTrip = post_wrapper.Children[2].Children[0].Children[3];
		// var author     = authorTrip.Children[0];
		// var trip       = post_poster_data.Children[1];
		var time = elem.QuerySelector(".time_wrap")?.Children[0];
		var fl   = post_files?.QuerySelector("a")?.GetAttribute("href");

		INode fname = default;

		if (post_files is { ChildNodes: { Length: >= 1 } cn }) {
			fname = cn[^1];

		}

		var text = elem.QuerySelector(".text");

		// var text   = post_wrapper.Children[4];
		var number = elem.QuerySelectorAll("header > div > a");
		var thread = elem.QuerySelector(".post_controls");

		var post = new ChanPost()
		{
			Title    = title?.TextContent,
			Author   = author?.TextContent,
			Filename = fname?.TextContent,
			Url     = ml,

			// Tripcode = trip?.TextContent,
			Text  = text?.TextContent,
			Urls  = [..links, fl],
			Other = new ExpandoObject(),
			Time  = DateTime.Parse(time.GetAttribute("datetime"))
		};
		post.Other.number = number;
		post.Other.thread = thread;

		return post;
	}

	public virtual void Dispose() { }

	protected BaseArchiveEngine()
	{
		Cookies = new CookieJar();

	}

	#region

	public delegate void OnResultCallback(object s, ChanPost p);

	public event OnResultCallback OnResult;

	#endregion

	public static readonly BaseArchiveEngine[] All = [new ArchiveOfSinsEngine(), new ArchivedMoeEngine()];

}