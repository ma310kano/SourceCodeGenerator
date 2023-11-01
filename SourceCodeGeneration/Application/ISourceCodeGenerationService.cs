using SourceCodeGeneration.Application.Command;

namespace SourceCodeGeneration.Application;

/// <summary>
/// ソースコード生成サービス
/// </summary>
public interface ISourceCodeGenerationService
{
    #region Methods

    /// <summary>
    /// 不変クラスを生成します。
    /// </summary>
    /// <param name="command">コマンド</param>
    /// <returns>生成したファイルのパスを返します。</returns>
    string GenerateImmutableClass(ImmutableClassGenerationCommand command);

    /// <summary>
    /// 不変クラスを生成します。
    /// </summary>
    /// <param name="command">コマンド</param>
    /// <returns>生成したファイルのパスを返します。</returns>
    Task<string> GenerateImmutableClassAsync(ImmutableClassGenerationCommand command);

    #endregion
}
