namespace SourceCodeGeneration.Application.Command;

/// <summary>
/// エンティティの生成コマンド
/// </summary>
/// <param name="NamespaceSequence">名前空間シーケンス</param>
/// <param name="ClassName">クラス名</param>
/// <param name="ClassDescription">クラスの説明</param>
/// <param name="Properties">プロパティのコレクション</param>
public record class EntityGenerationCommand(string NamespaceSequence, string ClassName, string ClassDescription, IReadOnlyCollection<EntityPropertyCommand> Properties);
