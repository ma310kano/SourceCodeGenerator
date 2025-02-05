namespace SourceCodeGeneration.Application.Command;

/// <summary>
/// 値オブジェクト(UUID)の生成コマンド
/// </summary>
/// <param name="NamespaceSequence">名前空間シーケンス</param>
/// <param name="ClassName">クラス名</param>
/// <param name="ClassDescription">クラスの説明</param>
public record class ValueObjectUuidGenerationCommand(string NamespaceSequence, string ClassName, string ClassDescription);