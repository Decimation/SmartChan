// Read S SmartChan.Lib SearchQuery.cs
// 2023-06-04 @ 5:28 PM

// global using IKeyValue = Kantan.Model.IKeyValue<string, object>;
// global using KeyValueList = System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<string, object>>;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kantan.Model;
using SmartChan.Lib.Model;

namespace SmartChan.Lib;

public class SearchQuery:IIdValMapDecomposable
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

	public IdValMap ToIdValMap()
	{
		var kv = new IdValMap()
		{
			["text"]          = Text,
			["submit_search"] = SubmitSearch,
			["tnum"]          = Thread,
			["subject"]       = Subject,
			["username"]      = Username,
			["tripcode"]      = Tripcode,
			["email"]         = Email,
			["uid"]           = Uid,
			["country"]       = Country,
			["filename"]      = Filename,
			["image"]         = Image,
			["start"]         = Start,
			["end"]           = End,
			["boards[]"]        = Array.Empty<string>(),
			["capcode"]       = Capcode,
			["filter"]        = Filter,
			["deleted"]       = Deleted,
			["ghost"]         = Ghost,
			["type"]          = Type,
			["results"]       = Results,
			["order"]         = Order,

		};

		/*var b  = Boards.Select(k => new KeyValuePair<string, object>("boards[]", k));
		kv.AddRange(b);*/

		return kv;

	}

}