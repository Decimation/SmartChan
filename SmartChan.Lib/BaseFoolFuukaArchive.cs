// Read S SmartChan.Lib BaseFoolFuukaArchive.cs
// 2023-06-04 @ 5:28 PM

using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Flurl.Http;
using Url = Flurl.Url;

namespace SmartChan.Lib;

public abstract class BaseFoolFuukaArchive : BaseChanArchive
{
	protected override async Task<IFlurlResponse> SearchInitialAsync(SearchQuery query)
	{
		var data = new FormUrlEncodedContent(query.ToKeyValues);

		var r = await Url.Combine(BaseUrl, "_", "search")
			        .AllowAnyHttpStatus()
			        .WithAutoRedirect(true)
			        .PostAsync(data);

		return r;
	}

	public override async Task<IEnumerable<ChanPost>> SearchAsync(SearchQuery q)
	{
		var r = await SearchInitialAsync(q);
		var c = await ParseAsync(r);

		return c;
	}

	protected override async Task<IEnumerable<ChanPost>> ParseAsync(IFlurlResponse r)
	{
		var uri = r.ResponseMessage.RequestMessage.RequestUri;

		var res2 = await uri.GetAsync();

		var parser = new HtmlParser();

		var dp   = await parser.ParseDocumentAsync(await res2.GetStringAsync());
		var elem = dp.QuerySelector(Resources.S_Post);

		var ent = new List<IElement>
		{
			elem
		};

		var pages = dp.QuerySelectorAll(Resources.S_Paginate);

		await Parallel.ForEachAsync(pages, CancellationToken.None, async (vElement, x) =>
		{
			var link = vElement.QuerySelector("a")?.GetAttribute("href");

			if (!Url.IsValid(link)) {
				return;
			}

			dp   = await parser.ParseDocumentAsync(await link.GetStringAsync());
			elem = dp.QuerySelector(Resources.S_Post);
			ent.Add(elem);

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
		var l2 = new List<(IElement, IElement)>();
		var cl = new List<ChanPost>();

		foreach (var e in ent) {

			cn += e.Children.Length / 2;

			for (int i = 0; i < e.Children.Length - 1; i += 2) {
				IElement ce  = e.Children[i];
				IElement ce2 = e.Children[i + 1];
				l2.Add((ce, ce2));

				var title = ce2.QuerySelector(".post_title");

				cl.Add(new ChanPost()
				{
					Title = title.TextContent
				});
			}
		}

		return cl;
	}
}

public sealed class ArchiveOfSins : BaseFoolFuukaArchive
{
	public override Url BaseUrl => "https://archiveofsins.com/";
}

public sealed class ArchivedMoe : BaseFoolFuukaArchive
{
	public override Url BaseUrl => "https://archived.moe/";
}