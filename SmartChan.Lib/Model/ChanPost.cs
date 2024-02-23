// Read S SmartChan.Lib ChanPost.cs
// 2023-06-04 @ 6:14 PM

using Flurl;
using Kantan.Model;
using SmartChan.Lib.Utilities;

namespace SmartChan.Lib.Model;

public record ChanPost : IIdValMapDecomposable
{

	public string Title { get; init; }

	public string Author { get; init; }

	public string Tripcode { get; init; }

	public string Filename { get; init; }

	public string Text { get; init; }

	public string[] Urls { get; init; }

	public DateTime Time { get; init; }

	public dynamic Other { get; init; }

	public Url Url { get; init; }

	public IdValMap ToIdValMap()
	{
		return new IdValMap
		{
			["Title"]    = Title,
			["Author"]   = Author,
			["Tripcode"] = Tripcode,
			["Filename"] = Filename,
			["Text"]     = Text,
			["Urls"]      = Urls,
			["Time"]     = Time,
			["Url"]     = Url,
		};

	}

	public override string ToString()
	{
		return $"{Title}";
	}

}