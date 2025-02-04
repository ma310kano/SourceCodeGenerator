namespace SourceCodeGeneration.Application.Command;

/// <summary>
/// プロパティのコマンド
/// </summary>
/// <param name="PropertyName">プロパティ名</param>
/// <param name="PropertyDescription">プロパティの説明</param>
/// <param name="TypeName">型名</param>
/// <param name="IsPrimaryKey">主キーかどうか</param>
public record class EntityPropertyCommand(string PropertyName, string PropertyDescription, string TypeName, bool IsPrimaryKey);
