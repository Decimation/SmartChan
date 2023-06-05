using System.Diagnostics;
using System.Runtime.CompilerServices;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Flurl.Http;
using Kantan.Text;
using Novus;
using SmartChan.Lib;
using Spectre.Console;
using Url = Flurl.Url;

namespace SmartChan;

public static class Program
{

	static Program()
	{

	}

	public static async Task Main(string[] args)
	{
#if DEBUG

#endif
		var boards = args[1].Split(',');
		var subj   = args[0];

		Console.WriteLine($"Boards: {boards.QuickJoin()} | Subject: {subj}");

		var archive = new ArchivedMoe();

		var sw = Stopwatch.StartNew();

		var posts = await archive.SearchAsync(new SearchQuery()
		{
			Boards       = boards,
			Subject      = subj,
			SubmitSearch = "Search"
		});

		Console.WriteLine(posts.Length);

		sw.Stop();
		Console.WriteLine($"{sw.Elapsed.TotalSeconds:F3}");
	}
}