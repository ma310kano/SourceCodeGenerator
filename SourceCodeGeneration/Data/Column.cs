namespace SourceCodeGeneration.Data;

/// <summary>
/// 列
/// </summary>
/// <param name="ColumnName">列</param>
/// <param name="Type">型</param>
/// <param name="Description">説明</param>
/// <param name="IsNotNull">NOT NULL かどうか</param>
/// <param name="IsPrimaryKey">主キーかどうか</param>
/// <param name="ContainsSummary">概要に含むかどうか</param>
public record class Column(ColumnName ColumnName, ColumnTypes Type, string Description, bool IsNotNull, bool IsPrimaryKey, bool ContainsSummary);
