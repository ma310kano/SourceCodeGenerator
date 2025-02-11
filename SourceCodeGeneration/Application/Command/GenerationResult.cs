namespace SourceCodeGeneration.Application.Command;

/// <summary>
/// 生成結果
/// </summary>
/// <param name="FileName">ファイル名</param>
/// <param name="Stream">ストリーム</param>
public record class GenerationResult(string FileName, Stream Stream);
