// Deci SmartChan SearchSettings.cs
// $File.CreatedYear-$File.CreatedMonth-22 @ 17:55

using JetBrains.Annotations;
using Spectre.Console.Cli;

namespace SmartChan;

internal sealed class SearchCommandSettings : CommandSettings
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