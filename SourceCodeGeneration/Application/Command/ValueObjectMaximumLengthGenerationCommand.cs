namespace SourceCodeGeneration.Application.Command;

/// <summary>
/// 値オブジェクトの生成コマンド
/// </summary>
/// <param name="NamespaceSequence">名前空間シーケンス</param>
/// <param name="ClassName">クラス名</param>
/// <param name="ClassDescription">クラスの説明</param>
/// <param name="MaximumLength">最大長</param>
public record class ValueObjectMaximumLengthGenerationCommand(string NamespaceSequence, string ClassName, string ClassDescription, int MaximumLength);
