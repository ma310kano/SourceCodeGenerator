namespace SourceCodeGeneration.Application.ValueObject.Command;

/// <summary>
/// 値オブジェクト(最大長)の生成コマンド
/// </summary>
/// <param name="NamespaceSequence">名前空間シーケンス</param>
/// <param name="ClassName">クラス名</param>
/// <param name="ClassDescription">クラスの説明</param>
/// <param name="MaximumLength">最大長</param>
public record class MaximumLengthGenerationCommand(string NamespaceSequence, string ClassName, string ClassDescription, int MaximumLength);
