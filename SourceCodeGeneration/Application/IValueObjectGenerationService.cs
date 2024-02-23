using SourceCodeGeneration.Application.Command;

namespace SourceCodeGeneration.Application;

/// <summary>
/// 値オブジェクト生成サービス
/// </summary>
public interface IValueObjectGenerationService
{
	#region Methods

	/// <summary>
	/// 値オブジェクトを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	Stream GenerateValueObject(ValueObjectMaximumLengthGenerationCommand command);

	/// <summary>
	/// 値オブジェクトを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	Task<Stream> GenerateValueObjectAsync(ValueObjectMaximumLengthGenerationCommand command);

	#endregion
}
