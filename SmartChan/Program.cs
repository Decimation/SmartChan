using System.Diagnostics;
using System.Runtime.CompilerServices;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Flurl.Http;
using Kantan.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Novus;
using SmartChan.Lib;
using SmartChan.Lib.Archives;
using SmartChan.Lib.Archives.Base;
using Spectre.Console;
using Url = Flurl.Url;

namespace SmartChan;

public static class Program
{
	static Program() { }

	public static async Task Main(string[] args)
	{
#if DEBUG

#endif
		var boards = args[1].Split(',');
		var subj   = args[0];

		AnsiConsole.WriteLine($"Boards: {boards.QuickJoin()} | Subject: {subj}");

		int n  = 0;
		var sc = new SearchClient();

		var query = new SearchQuery()
		{
			Boards       = boards,
			Subject      = subj,
			SubmitSearch = "Search"
		};

		var sw = Stopwatch.StartNew();

		sc.OnEngineResults += (o, chanPosts) =>
		{
			AnsiConsole.WriteLine($"{chanPosts.Length}");
		};

		var posts = await sc.Search(BaseArchiveEngine.All, query);

		sw.Stop();

		Console.WriteLine($"{sw.Elapsed.TotalSeconds:F3}");
		var p = posts[0];
		await File.WriteAllLinesAsync($"posts.txt",
		                              posts.Select(p => $"[{p.Url.QuickJoin(",")}]\n{p.Title}\n{p.Text}\n"));

	}
}