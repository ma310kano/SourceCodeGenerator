using SourceCodeGeneration.Application.ValueObject.Command;

namespace SourceCodeGeneration.Application.ValueObject;

/// <summary>
/// 値オブジェクト(文字列パターン)の生成サービス
/// </summary>
public interface IStringPatternGenerationService
{
	#region Methods

	/// <summary>
	/// 値オブジェクトを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	Stream Generate(StringPatternGenerationCommand command);

	/// <summary>
	/// 値オブジェクトを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	Task<Stream> GenerateAsync(StringPatternGenerationCommand command);

	#endregion
}
