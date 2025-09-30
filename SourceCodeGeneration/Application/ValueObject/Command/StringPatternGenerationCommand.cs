namespace SourceCodeGeneration.Application.ValueObject.Command;

/// <summary>
/// 値オブジェクト(文字列パターン)の生成コマンド
/// </summary>
/// <param name="NamespaceSequence">名前空間シーケンス</param>
/// <param name="ClassName">クラス名</param>
/// <param name="ClassDescription">クラスの説明</param>
/// <param name="Pattern">パターン</param>
/// <param name="PatternDescription">パターンの説明</param>
public record class StringPatternGenerationCommand(string NamespaceSequence, string ClassName, string ClassDescription, string Pattern, string PatternDescription);
