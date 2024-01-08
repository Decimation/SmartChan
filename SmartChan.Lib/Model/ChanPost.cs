// Read S SmartChan.Lib ChanPost.cs
// 2023-06-04 @ 6:14 PM

using Kantan.Model;
using SmartChan.Lib.Archives.Base;

namespace SmartChan.Lib.Model;

public class ChanResult:IDisposable
{
    public BaseArchiveEngine Engine { get;  }
	
    public IList<ChanPost> Results { get; internal init; }

    public ChanResult(BaseArchiveEngine engine)
    {
	    Engine  = engine;
	    Results = [];
    }

    public void Dispose()
    {
	    
    }

}

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