// Read S SmartChan.Lib SearchQuery.cs
// 2023-06-04 @ 5:28 PM

global using IKeyValue = Kantan.Model.IKeyValue<string, object>;
global using KeyValueList = System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<string, object>>;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kantan.Model;

namespace SmartChan.Lib;

public class SearchQuery : IKeyValue
{

	#region

	public string Text { get; init; }

	public string SubmitSearch { get; init; }

	public long Thread { get; init; }

	public string Subject { get; init; }

	public string Username { get; init; }

	public string Tripcode { get; init; }

	public string Email { get; init; }

	public string Uid { get; init; }

	public string Country { get; init; }

	public string Filename { get; init; }

	public string Image { get; init; }

	public string Start { get; init; }

	public string End { get; init; }

	public string[] Boards { get; init; }

	public string Capcode { get; init; }

	public string Filter { get; init; }

	public string Deleted { get; init; }

	public string Ghost { get; init; }

	public string Type { get; init; }

	public string Results { get; init; }

	public string Order { get; init; }

	#endregion

	public SearchQuery()
	{
		Boards = null;
	}

	public KeyValueList GetKeyValues()
	{
		var kv = new List<KeyValuePair<string, object>>()
		{
			new("text", Text),
			new("submit_search", SubmitSearch),
			new("tnum", Thread),
			new("subject", Subject),
			new("username", Username),
			new("tripcode", Tripcode),
			new("email", Email),
			new("uid", Uid),
			new("country", Country),
			new("filename", Filename),
			new("image", Image),
			new("start", Start),
			new("end", End),
			//boards[]
			new("capcode", Capcode),
			new("filter", Filter),
			new("deleted", Deleted),
			new("ghost", Ghost),
			new("type", Type),
			new("results", Results),
			new("order", Order),

		};

		var b = Boards.Select(k => new KeyValuePair<string, object>("boards[]", k));
		kv.AddRange(b);

		return kv;

	}

}