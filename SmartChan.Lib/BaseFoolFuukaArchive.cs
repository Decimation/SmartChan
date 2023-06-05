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
		var data = new FormUrlEncodedContent(query.KeyValues.OfType<KeyValuePair<string, string>>());

		var r = await Url.Combine(BaseUrl, "_", "search")
			        .WithClient(Client)
			        .AllowAnyHttpStatus()
			        .WithAutoRedirect(true)
			        .PostAsync(data);

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
		// var uri  = r.ResponseMessage.RequestMessage.RequestUri;
		// var res2 = await uri.WithClient(Client).GetAsync(ct);

		var parser = new HtmlParser();

		var    l2          = new List<(IElement, IElement)>();
		
		// var s = await res2.GetStringAsync();

		var s  = await r.ResponseMessage.Content.ReadAsStringAsync();
		var dp = await parser.ParseDocumentAsync(s);
		var e  = dp.QuerySelector(Resources.S_Post);
		NewFunction(l2, e);

		var pages = dp.QuerySelectorAll(Resources.S_Paginate);

		await Parallel.ForEachAsync(pages, ct, async (vElement, x) =>
		{
			var link = vElement.QuerySelector("a")?.GetAttribute("href");

			if (!Url.IsValid(link)) {
				return;
			}

			dp = await parser.ParseDocumentAsync(await link.WithClient(Client).GetStringAsync(ct));
			e  = dp.QuerySelector(Resources.S_Post);
			// ent.Add(elem);
			NewFunction(l2, e);
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
			var (ce, ce2) = ce3;

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
				Title  = title.TextContent,
				Author = author.TextContent,
				// Tripcode = trip.TextContent,
				Text = text.TextContent,

			};
			cl.Add(post);

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
}

public sealed class ArchiveOfSins : BaseFoolFuukaArchive
{
	public override Url BaseUrl => "https://archiveofsins.com/";
}

public sealed class ArchivedMoe : BaseFoolFuukaArchive
{
	public override Url BaseUrl => "https://archived.moe/";
}