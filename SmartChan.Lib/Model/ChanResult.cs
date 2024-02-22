// Deci SmartChan.Lib ChanResult.cs
// $File.CreatedYear-$File.CreatedMonth-22 @ 16:12

using SmartChan.Lib.Archives.Base;

namespace SmartChan.Lib.Model;

public class ChanResult : IDisposable
{

	public BaseArchiveEngine Engine { get; }

	public IList<ChanPost> Results { get; internal init; }

	public ChanResult(BaseArchiveEngine engine)
	{
		Engine  = engine;
		Results = [];
	}

	public void Dispose() { }

}