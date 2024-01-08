// Read S SmartChan.Lib ChanPost.cs
// 2023-06-04 @ 6:14 PM

using Kantan.Model;

namespace SmartChan.Lib;

public record ChanPost : IKeyValue
{

	public string Title { get; init; }

	public string Author { get; init; }

	public string Tripcode { get; init; }

	public string Filename { get; init; }

	public string Text { get; init; }

	public string[] Url { get; init; }

	public DateTime Time { get; init; }

	public dynamic Other { get; init; }

	public KeyValueList GetKeyValues()
	{
		return IMap.ToMap(this, m => m.Name != nameof(GetKeyValues))
			.ToList();
	}

	public override string ToString()
	{
		return $"{Title}";
	}

}