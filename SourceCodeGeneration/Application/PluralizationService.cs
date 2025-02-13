using System.Text.RegularExpressions;

namespace SourceCodeGeneration.Application;

/// <summary>
/// 複数形のサービス
/// </summary>
public partial class PluralizationService : IPluralizationService
{
	#region Methods

	/// <summary>
	/// 値の複数形を返します。
	/// </summary>
	/// <param name="value">値</param>
	/// <returns>値の複数形を返します。</returns>
	public string Pluralize(string value)
	{
		string result;

		if (value.EndsWith('s') || value.EndsWith("sh") || value.EndsWith("ch") || value.EndsWith('x') || EndO().IsMatch(value))
		{
			result = value + "es";
		}
		else if (value.EndsWith('f') || value.EndsWith("fe"))
		{
			result = value + "ves";
		}
		else if (EndY().IsMatch(value))
		{
			result = value[..^1] + "ies";
		}
		else
		{
			result = value + "s";
		}

		return result;
	}

	/// <summary>
	/// 値の複数形を返します。
	/// </summary>
	/// <param name="value">値</param>
	/// <returns>値の複数形を返します。</returns>
	public async Task<string> PluralizeAsync(string value)
	{
		return await Task.Run(() => Pluralize(value));
	}

	/// <summary>
	/// 末尾 o
	/// </summary>
	/// <returns>正規表現を返します。</returns>
	[GeneratedRegex("[^aeiou]o$")]
	private static partial Regex EndO();

	/// <summary>
	/// 末尾 y
	/// </summary>
	/// <returns>正規表現を返します。</returns>
	[GeneratedRegex("[^aeiou]y$")]
	private static partial Regex EndY();

	#endregion
}
