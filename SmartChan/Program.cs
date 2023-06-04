using System.CommandLine;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Flurl.Http;
using Kantan.Text;
using SmartChan.Lib;
using Url = Flurl.Url;

namespace SmartChan;

public static class Program
{
	public static async Task Main(string[] args)
	{
#if DEBUG
		
#endif
		var boards = args[1].Split(',');
		var subj   = args[0];

		Console.WriteLine($"Boards: {boards.QuickJoin()} | Subject: {subj}");

		var archive = new ArchivedMoe();

		var r = await archive.SearchAsync(new SearchQuery()
		{
			Boards = args[1].Split(','),
			Subject = args[0],
			SubmitSearch = "Search"
		});

		foreach (ChanPost c in r) {
			Console.WriteLine(c);
		}

		Console.WriteLine(r.Count());
	}

	public static KeyValuePair<T1, T2>[] Add<T1, T2>(this KeyValuePair<T1, T2>[] t, params T1[] t2)
	{
		var l = new List<KeyValuePair<T1, T2>>(t);
		l.AddRange(t2.Select(k => new KeyValuePair<T1, T2>(k, default(T2))));
		return l.ToArray();
	}
}