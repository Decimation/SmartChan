// Read S SmartChan.Lib SearchQuery.cs
// 2023-06-04 @ 5:28 PM

#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kantan.Model;

#endregion

namespace SmartChan.Lib;

public interface IKeyValue
{
	public KeyValuePair<string, string>[] ToKeyValues { get; }
}

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

	public string   Uid      { get; init; }
	public string   Country  { get; init; }
	public string   Filename { get; init; }
	public string   Image    { get; init; }
	public string   Start    { get; init; }
	public string   End      { get; init; }
	public string[] Boards   { get; init; }
	public string   Capcode  { get; init; }
	public string   Filter   { get; init; }
	public string   Deleted  { get; init; }
	public string   Ghost    { get; init; }
	public string   Type     { get; init; }
	public string   Results  { get; init; }
	public string   Order    { get; init; }

	#endregion

	public KeyValuePair<string, string>[] ToKeyValues
	{
		get
		{
			var kv = new List<KeyValuePair<string, string>>
			{
				new("text", Text),
				new("submit_search", SubmitSearch),
				new("tnum", Thread.ToString()),
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

			var b = Boards.Select(kv => new KeyValuePair<string, string>("boards[]", kv));
			kv.AddRange(b);

			return kv.ToArray();
		}
	}
}