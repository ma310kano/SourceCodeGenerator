using SourceCodeGeneration.Application.ValueObject.Command;

namespace SourceCodeGeneration.Application.ValueObject;

/// <summary>
/// 値オブジェクト(最大長)の生成サービス
/// </summary>
public interface IMaximumLengthGenerationService
{
	#region Methods

	/// <summary>
	/// 値オブジェクトを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	Stream Generate(MaximumLengthGenerationCommand command);

	/// <summary>
	/// 値オブジェクトを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	Task<Stream> GenerateAsync(MaximumLengthGenerationCommand command);

	#endregion
}
