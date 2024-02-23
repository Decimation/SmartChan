// Read S SmartChan.Lib BaseFoolFuukaArchive.cs
// 2023-06-04 @ 5:28 PM

using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Flurl.Http;
using Kantan.Text;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using JetBrains.Annotations;
using Url = Flurl.Url;
using System.Runtime.ConstrainedExecution;
using SmartChan.Lib.Model;
using SmartChan.Lib.Utilities;

namespace SmartChan.Lib.Archives.Base;

public abstract class BaseFoolFuukaEngine : BaseArchiveEngine
{

	protected BaseFoolFuukaEngine() : base() { }

	public override Url SearchUrl => Url.Combine(BaseUrl, ChanHelper.BI_WLD_QUERY, "search");

	protected override async Task<IFlurlResponse> GetInitialResponseAsync(SearchQuery query)
	{
		var objects = query.ToIdValMap().ToList();
		var bi      = ParseBoards(query.Boards);

		var queryString = ChanHelper.GetBoardQueryString(bi);

		var boardsPost = queryString
			.Split(",", StringSplitOptions.TrimEntries)
			.Select(x => new KeyValuePair<string, object>("boards[]", x));

		// objects["boards[]"] = boardString;
		objects.AddRange(boardsPost);

		var tmp1 = objects
			.Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value?.ToString()));

		var data = new FormUrlEncodedContent(tmp1);

		var r = await BuildInitialRequest(query)
			        .AllowAnyHttpStatus()
			        .PostAsync(data);

		Logger.LogTrace("{Url} for {Name} #1", SearchUrl, Name);

		return r;
	}

	public override async Task<ChanResult> RunSearchAsync(SearchQuery q)
	{
		var r = await GetInitialResponseAsync(q);
		var c = await ParseResponseAsync(r);

		return c;
	}

	protected override async Task<ChanResult> ParseResponseAsync(IFlurlResponse r, CancellationToken ct = default)
	{
		string s = await GetDocumentAsync(r, ct);

		var parser = new HtmlParser();

		// var s  = await r.ResponseMessage.Content.ReadAsStringAsync();
		var dp = await parser.ParseDocumentAsync(s);
		var e  = dp.QuerySelectorAll(Resources.S_Post2);

		// var e = dp.GetElementsByTagName(Resources.S_Post3);

		// NewFunction(l2, e);
		// l2.AddRange(e);
		var pages = dp.QuerySelectorAll(Resources.S_Paginate);
		var l2    = new List<IElement>(pages.Length);

		Progress?.Report(new(0, pages.Length));

		await Parallel.ForEachAsync(pages, ct, async (vElement, x) =>
		{
			var link = vElement.QuerySelector("a")?.GetAttribute("href");

			if (!Url.IsValid(link)) {
				return;
			}

			var async = await Client.Request(link)
				            .WithCookies(Cookies)
				            .GetStringAsync(cancellationToken: ct);

			dp = await parser.ParseDocumentAsync(async);

			e = dp.QuerySelectorAll(Resources.S_Post2);

			// e = dp.GetElementsByTagName(Resources.S_Post3);
			// ent.Add(elem);
			// NewFunction(l2, e);
			l2.AddRange(e);

		});

		/*foreach (IElement vElement in pages) {
			var link = vElement.QuerySelector("a")?.GetAttribute("href");

			if (!Url.IsValid(link)) {
				continue;
			}

			dp   = await parser.ParseDocumentAsync(await link.GetStringAsync());
			elem = dp.QuerySelector(Resources.S_Paginate);
			ent.Add(elem);
		}*/

		int cn = 0;

		var cb = new ConcurrentBag<ChanPost>();

		await Parallel.ForEachAsync(l2, ct, async (element, token) =>
		{
			var pb = await ParseBodyAsync(element, token);

			if (pb is { }) {
				cb.Add(pb);
			}

			Progress?.Report(new(cb.Count, pages.Length));

		});

		var cr = new ChanResult(this)
		{
			Results = new List<ChanPost>(cb)
		};
		cb.Clear();

		return cr;

	}

	[ICBN]
	protected async Task<string> GetDocumentAsync(IFlurlResponse r, CancellationToken ct)
	{
		var uri = r.ResponseMessage.Headers.Location;
		uri ??= r.ResponseMessage.RequestMessage.RequestUri;

		Logger.LogTrace("{Url} for {Name} #2", uri, Name);

		var res2 = await Client.Request(uri)
			           .WithCookies(Cookies)
			           .OnError(er =>
			           {
				           er.ExceptionHandled = true;
				           Trace.WriteLine($"{Name}: {er.Exception.Message} when {nameof(GetDocumentAsync)}");
			           })
			           .GetAsync(cancellationToken: ct);

		var s = await res2.GetStringAsync();
		return s;
	}

}