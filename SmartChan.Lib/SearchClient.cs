global using CBN = JetBrains.Annotations.CanBeNullAttribute;
global using ICBN = JetBrains.Annotations.ItemCanBeNullAttribute;
global using MN = System.Diagnostics.CodeAnalysis.MaybeNullAttribute;
global using NN = JetBrains.Annotations.NotNullAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartChan.Lib.Archives.Base;
using SmartChan.Lib.Model;

namespace SmartChan.Lib;

public class SearchClient
{

	public delegate void EngineResultsCallback(object s, ChanResult p);

	public event EngineResultsCallback OnEngineResults;

	public async Task<IList<ChanResult>> RunSearchAsync(SearchQuery q, BaseArchiveEngine[] eng)
	{
		var tasks = eng.Select(e =>
		{
			var task = e.RunSearchAsync(q).ContinueWith(r =>
			{
				var result = r.Result;

				if (r.IsCompletedSuccessfully) {
					OnEngineResults?.Invoke(r, result);
				}

				return result;
			});

			return task;
		}).ToList();

		var crr = new List<ChanResult>(eng.Length);

		while (tasks.Count > 0) {

			var r  = await Task.WhenAny(tasks);
			var rr = await r;

			// OnEngineResults?.Invoke(this, rr);
			crr.Add(rr);
			tasks.Remove(r);
		}

		return crr;
	}

}