using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Flurl.Http;
using Kantan.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartChan.Lib.Archives;
using Spectre.Console;
using Spectre.Console.Cli;
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

	public static async Task Main(string[] args)
	{
#if DEBUG
		if (args is not { Length: >= 1 }) {
			args = Console.ReadLine().Split(' ');
		}
#endif
		AnsiConsole.WriteLine(args.QuickJoin());

		var app = new CommandApp<SearchCommand>();
		var res = await app.RunAsync(args);

	}

}