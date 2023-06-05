// Read S SmartChan.Lib BaseFoolFuukaArchive.cs
// 2023-06-04 @ 5:28 PM

using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Flurl.Http;
using Kantan.Text;
using Microsoft.Extensions.Logging;
using System;
using JetBrains.Annotations;
using Url = Flurl.Url;

namespace SmartChan.Lib.Archives.Base;

public abstract class BaseFoolFuukaArchiveEngine : BaseArchiveEngine
{
	protected BaseFoolFuukaArchiveEngine() : base() { }

	public override Url SearchUrl => Url.Combine(BaseUrl, "_", "search");

	protected override async Task<IFlurlResponse> SearchInitialAsync(SearchQuery query)
	{
		var data = new FormUrlEncodedContent(
			query.KeyValues.Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value?.ToString())));

		var r = await SearchUrl
			        .WithClient(Client)
			        .AllowAnyHttpStatus()
			        .WithAutoRedirect(true)
			        .WithCookies(Cookies)
			        .PostAsync(data);

		Logger.LogTrace("{Url} for {Name} #1", SearchUrl, Name);

		return r;
	}

	public override async Task<ChanPost[]> SearchAsync(SearchQuery q)
	{
		var r = await SearchInitialAsync(q);
		var c = await ParseAsync(r);

		return c.ToArray();
	}

	protected override async Task<IEnumerable<ChanPost>> ParseAsync(IFlurlResponse r, CancellationToken ct = default)
	{
		string s = await GetDocument(ct, r);

		var parser = new HtmlParser();

		var l2 = new List<IElement>();

		// var s  = await r.ResponseMessage.Content.ReadAsStringAsync();
		var dp = await parser.ParseDocumentAsync(s);
		var e  = dp.QuerySelectorAll(Resources.S_Post2);
		// NewFunction(l2, e);
		l2.AddRange(e);
		var pages = dp.QuerySelectorAll(Resources.S_Paginate);

		await Parallel.ForEachAsync(pages, ct, async (vElement, x) =>
		{
			var link = vElement.QuerySelector("a")?.GetAttribute("href");

			if (!Url.IsValid(link)) {
				return;
			}

			dp = await parser.ParseDocumentAsync(await link.WithClient(Client).WithCookies(Cookies).GetStringAsync(ct));
			e  = dp.QuerySelectorAll(Resources.S_Post2);
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
		var cl = new List<ChanPost>();

		await Parallel.ForEachAsync(l2, async (ce3, a) =>
		{
			// var (ce, ce2) = ce3;

			var ce2   = ce3;
			var title = ce2.QuerySelector(".post_title");
			// var post_wrapper = ce2.Children[1];

			// var title = post_wrapper.Children[2].Children[0].Children[2];

			var author = ce2.QuerySelector(".post_author");
			// var authorTrip = post_wrapper.Children[2].Children[0].Children[3];
			// var author     = authorTrip.Children[0];
			// var trip       = authorTrip.Children[1];

			var text = ce2.QuerySelector(".text");
			// var text   = post_wrapper.Children[4];
			var number = ce2.QuerySelectorAll("header > div > a");

			var post = new ChanPost()
			{
				Title  = title?.TextContent,
				Author = author?.TextContent,
				// Tripcode = trip.TextContent,
				Text = text?.TextContent,

			};

			cl.Add(post);
			OnPostInvocation(post);
		});

		return cl;

		static void NewFunction(ICollection<(IElement, IElement)> valueTuples, IElement element)
		{
			for (int i = 0; i < element.Children.Length - 1; i += 2) {
				IElement ce  = element.Children[i];
				IElement ce2 = element.Children[i + 1];
				valueTuples.Add((ce, ce2));

			}
		}
	}

	protected async Task<string> GetDocument(CancellationToken ct, IFlurlResponse r)
	{
		var uri = r.ResponseMessage.Headers.Location;
		uri ??= r.ResponseMessage.RequestMessage.RequestUri;

		Logger.LogTrace("{Url} for {Name} #2", uri, Name);
		var res2 = await uri.WithClient(Client).WithCookies(Cookies).GetAsync(ct);
		var s    = await res2.GetStringAsync();
		return s;
	}
}