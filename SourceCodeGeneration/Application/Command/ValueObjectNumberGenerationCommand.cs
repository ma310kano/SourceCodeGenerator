﻿namespace SourceCodeGeneration.Application.Command;

/// <summary>
/// 値オブジェクト(数値)の生成コマンド
/// </summary>
/// <param name="ClassName">クラス名</param>
/// <param name="ClassDescription">クラスの説明</param>
/// <param name="NamespaceSequence">名前空間シーケンス</param>
/// <param name="HasMinimumNumber">最小値を持つかどうか</param>
/// <param name="MinimumNumber">最小値</param>
/// <param name="HasMaximumNumber">最大値を持つかどうか</param>
/// <param name="MaximumNumber">最大値</param>
public record class ValueObjectNumberGenerationCommand(string ClassName, string ClassDescription, string NamespaceSequence, bool HasMinimumNumber, int MinimumNumber, bool HasMaximumNumber, int MaximumNumber);