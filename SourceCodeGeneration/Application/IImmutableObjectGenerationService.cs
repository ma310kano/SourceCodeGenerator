using SourceCodeGeneration.Application.Command;

namespace SourceCodeGeneration.Application;

/// <summary>
/// 不変オブジェクト生成サービス
/// </summary>
public interface IImmutableObjectGenerationService
{
    #region Methods

    /// <summary>
    /// 不変オブジェクトを生成します。
    /// </summary>
    /// <param name="command">コマンド</param>
    /// <returns>生成したストリームを返します。</returns>
    Stream GenerateImmutableObject(ImmutableObjectGenerationCommand command);

    /// <summary>
    /// 不変オブジェクトを生成します。
    /// </summary>
    /// <param name="command">コマンド</param>
    /// <returns>生成したストリームを返します。</returns>
    Task<Stream> GenerateImmutableObjectAsync(ImmutableObjectGenerationCommand command);

    #endregion
}
