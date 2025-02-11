using SourceCodeGeneration.Application.Command;

namespace SourceCodeGeneration.Application;

/// <summary>
/// テーブルの生成サービス
/// </summary>
public interface ITableGenerationService
{
	#region Methods

	/// <summary>
	/// CREATE TABLE 文を生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	GenerationResult GenerateCreateTableStatement(TableGenerationCommand command);

	/// <summary>
	/// CREATE TABLE 文を生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	Task<GenerationResult> GenerateCreateTableStatementAsync(TableGenerationCommand command);

	/// <summary>
	/// データクラスを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	GenerationResult GenerateDataClass(TableGenerationCommand command);

	/// <summary>
	/// データクラスを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	Task<GenerationResult> GenerateDataClassAsync(TableGenerationCommand command);

	/// <summary>
	/// クエリサービスのクラスを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	GenerationResult GenerateQueryServiceClass(TableGenerationCommand command);

	/// <summary>
	/// クエリサービスのクラスを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	Task<GenerationResult> GenerateQueryServiceClassAsync(TableGenerationCommand command);

	/// <summary>
	/// クエリサービスのインターフェースを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	GenerationResult GenerateQueryServiceInterface(TableGenerationCommand command);

	/// <summary>
	/// クエリサービスのインターフェースを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	Task<GenerationResult> GenerateQueryServiceInterfaceAsync(TableGenerationCommand command);

	/// <summary>
	/// 概要データクラスを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	GenerationResult GenerateSummaryDataClass(TableGenerationCommand command);

	/// <summary>
	/// 概要データクラスを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	Task<GenerationResult> GenerateSummaryDataClassAsync(TableGenerationCommand command);

	/// <summary>
	/// 概要クエリサービスのクラスを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	GenerationResult GenerateSummaryQueryServiceClass(TableGenerationCommand command);

	/// <summary>
	/// 概要クエリサービスのクラスを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	Task<GenerationResult> GenerateSummaryQueryServiceClassAsync(TableGenerationCommand command);

	/// <summary>
	/// 概要クエリサービスのインターフェースを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	GenerationResult GenerateSummaryQueryServiceInterface(TableGenerationCommand command);

	/// <summary>
	/// 概要クエリサービスのインターフェースを生成します。
	/// </summary>
	/// <param name="command">コマンド</param>
	/// <returns>生成した結果を返します。</returns>
	Task<GenerationResult> GenerateSummaryQueryServiceInterfaceAsync(TableGenerationCommand command);

	#endregion
}
