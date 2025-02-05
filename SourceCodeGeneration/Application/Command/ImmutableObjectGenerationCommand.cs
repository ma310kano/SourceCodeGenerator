namespace SourceCodeGeneration.Application.Command;

/// <summary>
/// 不変オブジェクトの生成コマンド
/// </summary>
/// <param name="NamespaceSequence">名前空間シーケンス</param>
/// <param name="ClassName">クラス名</param>
/// <param name="ClassDescription">クラスの説明</param>
/// <param name="Properties">プロパティのコレクション</param>
public record class ImmutableObjectGenerationCommand(string NamespaceSequence, string ClassName, string ClassDescription, IReadOnlyList<ImmutableObjectPropertyCommand> Properties);
