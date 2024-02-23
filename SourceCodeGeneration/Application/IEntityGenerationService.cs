using SourceCodeGeneration.Application.Command;

namespace SourceCodeGeneration.Application;

/// <summary>
/// エンティティ生成サービス
/// </summary>
public interface IEntityGenerationService
{
	#region Methods

	/// <summary>
	/// エンティティを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	Stream GenerateEntity(EntityGenerationCommand command);

	/// <summary>
	/// エンティティを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成したストリームを返します。</returns>
	Task<Stream> GenerateEntityAsync(EntityGenerationCommand command);

	#endregion
}
