namespace SourceCodeGeneration.Application;

/// <summary>
/// 複数形のサービス
/// </summary>
public interface IPluralizationService
{
	#region Methods

	/// <summary>
	/// 値の複数形を返します。
	/// </summary>
	/// <param name="value">値</param>
	/// <returns>値の複数形を返します。</returns>
	string Pluralize(string value);

	/// <summary>
	/// 値の複数形を返します。
	/// </summary>
	/// <param name="value">値</param>
	/// <returns>値の複数形を返します。</returns>
	Task<string> PluralizeAsync(string value);

	#endregion
}