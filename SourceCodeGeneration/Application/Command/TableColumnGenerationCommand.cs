namespace SourceCodeGeneration.Application.Command;

/// <summary>
/// テーブル列の生成コマンド
/// </summary>
/// <param name="ColumnName">列名</param>
/// <param name="ColumnType">列の型</param>
/// <param name="Description">説明</param>
/// <param name="IsNotNull">NOT NULL かどうか</param>
/// <param name="IsPrimaryKey">キーかどうか</param>
/// <param name="ContainsSummary">概要に含めるかどうか</param>
public record class TableColumnGenerationCommand(string ColumnName, ColumnTypes ColumnType, string Description, bool IsNotNull, bool IsPrimaryKey, bool ContainsSummary);