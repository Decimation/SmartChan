using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartChan.Lib.Archives.Base;

namespace SmartChan.Lib;

public class SearchClient
{
	public delegate void OnEngine(object s, ChanPost[] p);

	public event OnEngine OnEngineRes;

	public async Task<ChanPost[]> Search(BaseArchiveEngine[] eng, SearchQuery q)
	{
		var tasks = eng.Select(e =>
		{
			return e.SearchAsync(q);
		}).ToList();

		var posts = new List<ChanPost>();

		while (tasks.Any()) {

			var r  = await Task.WhenAny(tasks);
			var rr = await r;

			OnEngineRes?.Invoke(this, rr);
			posts.AddRange(rr);
			tasks.Remove(r);
		}

		return posts.ToArray();
	}
}