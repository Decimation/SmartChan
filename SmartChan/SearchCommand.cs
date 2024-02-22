// Deci SmartChan SearchCommand.cs
// $File.CreatedYear-$File.CreatedMonth-22 @ 16:27

using System.Diagnostics;
using JetBrains.Annotations;
using SmartChan.Lib;
using SmartChan.Lib.Archives.Base;
using SmartChan.Lib.Model;
using Spectre.Console;
using Spectre.Console.Cli;
using static SmartChan.SearchCommand;

namespace SmartChan;

internal sealed class SearchCommand : AsyncCommand<SearchCommand.Settings>
{

	public sealed class Settings : CommandSettings
	{

		[CanBeNull]
		[CommandArgument(0, "[searchPath]")]
		public string Subject { get; init; }

		[CanBeNull]
		[CommandOption("-b|--boards")]
		public string Boards { get; init; }

		[CommandOption("--output")]
		public string Output { get; init; }

	}

	public override ValidationResult Validate(CommandContext context, Settings settings)
	{
		return base.Validate(context, settings);
	}

	public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
	{

		AnsiConsole.WriteLine($"Boards: {string.Join(',', settings.Boards)} | Subject: {settings.Subject}");

		int n = 0;

		var sc = new SearchClient();

		var query = new SearchQuery()
		{
			Boards       = settings.Boards.Split(','),
			Subject      = settings.Subject,
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

		var all   = BaseArchiveEngine.All;
		var posts = await sc.RunSearchAsync(query, all);

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

		return 0;
	}

	public static FileStream OutputFileStream { get; private set; }

	public static StreamWriter OutputWriter { get; private set; }

	private static void OpenOutput()
	{
		// Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

		var fn = $"results.txt";

		OutputFileStream = File.Open(fn, FileMode.OpenOrCreate);

		OutputWriter = new StreamWriter(OutputFileStream);
	}

	private static void CloseOutput()
	{

		OutputFileStream?.Dispose();

		// OutputWriter?.Dispose();
		OutputFileStream = null;
		OutputWriter     = null;
	}

}