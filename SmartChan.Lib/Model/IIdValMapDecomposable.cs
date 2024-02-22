// Deci SmartChan.Lib IDecomposable.cs
// $File.CreatedYear-$File.CreatedMonth-22 @ 16:12

global using IdValMap = System.Collections.Generic.Dictionary<string, object>;

namespace SmartChan.Lib.Model;

public interface IIdValMapDecomposable
{

	public IdValMap ToIdValMap();

}