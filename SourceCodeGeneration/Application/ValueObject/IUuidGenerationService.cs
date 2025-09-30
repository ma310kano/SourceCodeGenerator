using SourceCodeGeneration.Application.ValueObject.Command;

namespace SourceCodeGeneration.Application.ValueObject;

/// <summary>
/// 値オブジェクト(UUID)の生成サービス
/// </summary>
public interface IUuidGenerationService
{
	#region Methods

	/// <summary>
	/// 値オブジェクトを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	Stream Generate(UuidGenerationCommand command);

	/// <summary>
	/// 値オブジェクトを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	Task<Stream> GenerateAsync(UuidGenerationCommand command);

	#endregion
}
