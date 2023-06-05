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

		int n       = 0;
		var archive = new ArchivedMoeEngine();

		archive.OnPost += (o, r) =>
		{
			Console.Title = $"\r{++n}";
			AnsiConsole.WriteLine($"{r.Title}::{r.Text}");
		};

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

		await File.WriteAllLinesAsync($"posts.txt", posts.Select(p => $"[{p.Url}]\n{p.Title}\n{p.Text}\n"));

	}
}