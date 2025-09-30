using SourceCodeGeneration.Application.ValueObject.Command;

namespace SourceCodeGeneration.Application.ValueObject;

/// <summary>
/// 値オブジェクト(数値)の生成サービス
/// </summary>
public interface INumberGenerationService
{
	#region Methods

	/// <summary>
	/// 値オブジェクトを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	Stream Generate(NumberGenerationCommand command);

	/// <summary>
	/// 値オブジェクトを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	Task<Stream> GenerateAsync(NumberGenerationCommand command);

	#endregion
}
