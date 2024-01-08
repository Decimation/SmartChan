using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartChan.Lib.Archives.Base;

namespace SmartChan.Lib;

public class SearchClient
{
	public delegate void EngineResultsCallback(object s, ChanPost[] p);

	public event EngineResultsCallback OnEngineResults;

	public async Task<ChanPost[]> Search(BaseArchiveEngine[] eng, SearchQuery q)
	{
		var tasks = eng.Select(e =>
		{
			return e.RunSearchAsync(q);
		}).ToList();

		var posts = new List<ChanPost>();

		while (tasks.Any()) {

			var r  = await Task.WhenAny(tasks);
			var rr = await r;

			OnEngineResults?.Invoke(this, rr);
			posts.AddRange(rr);
			tasks.Remove(r);
		}

		return posts.ToArray();
	}
}