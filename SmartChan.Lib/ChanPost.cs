// Read S SmartChan.Lib ChanPost.cs
// 2023-06-04 @ 6:14 PM

namespace SmartChan.Lib;

public record ChanPost
{
	public string   Title    { get; init; }
	public string   Author   { get; init; }
	public string   Tripcode { get; init; }
	public string   Filename { get; init; }
	public string   Text     { get; init; }
	public string[] Url      { get; init; }
	public DateTime Time     { get; init; }
	
	public override string ToString()
	{
		return $"{Title}";
	}
}