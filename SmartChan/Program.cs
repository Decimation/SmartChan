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
using SmartChan.Lib.Model;
using Spectre.Console;
using Url = Flurl.Url;

namespace SmartChan;

public static class Program
{

	static Program()
	{
		AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
		{
			// CloseOutput();
			Debugger.Break();
		};
	}

	public static FileStream OutputFileStream { get; private set; }

	public static StreamWriter OutputWriter { get; private set; }

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

		OpenOutput();

		/*sc.OnEngineResults += (o, chanResult) =>
		{

			// AnsiConsole.WriteLine($"{chanPosts.Length}");
			OutputWriter.WriteLine($"{chanResult.Engine.Name} {chanResult.Results.Count}");

			foreach (ChanPost post in chanResult.Results) {
				// AnsiConsole.WriteLine($"\t{post.Text}");
				OutputWriter.WriteLine($"[{post.Author} @ {post.Time}:\n{post.Text}");
			}

			OutputWriter.WriteLine();

			OutputWriter.Flush();
		};*/

		var posts = await sc.RunSearchAsync(query, BaseArchiveEngine.All);

		sw.Stop();

		Console.WriteLine($"{sw.Elapsed.TotalSeconds:F3}");

		foreach (ChanResult post in posts) {
			OutputWriter.WriteLine($"{post.Engine.Name} - {post.Results.Count}:");
			OutputWriter.WriteLine();

			foreach (ChanPost postResult in post.Results) {
				OutputWriter.WriteLine($"{postResult.Author} @ {postResult.Time}:");
				OutputWriter.WriteLine($"{postResult.Text}");
				OutputWriter.WriteLine();
			}

			OutputWriter.Flush();
		}

		CloseOutput();
	}

	private static void OpenOutput()
	{
		OutputFileStream = File.Open("./res.txt", FileMode.OpenOrCreate);
		
		OutputWriter     = new StreamWriter(OutputFileStream);
	}

	private static void CloseOutput()
	{

		OutputFileStream?.Dispose();
		// OutputWriter?.Dispose();
		OutputFileStream = null;
		OutputWriter     = null;
	}

}