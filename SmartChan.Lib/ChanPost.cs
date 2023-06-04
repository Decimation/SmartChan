// Read S SmartChan.Lib ChanPost.cs
// 2023-06-04 @ 6:14 PM

namespace SmartChan.Lib;

public class ChanPost
{
	public string Title { get; init; }

	public override string ToString()
	{
		return $"{Title}";
	}
}