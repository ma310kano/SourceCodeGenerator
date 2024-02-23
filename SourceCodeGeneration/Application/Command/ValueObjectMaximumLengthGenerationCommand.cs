namespace SourceCodeGeneration.Application.Command;

/// <summary>
/// 値オブジェクトの生成コマンド
/// </summary>
public class ValueObjectMaximumLengthGenerationCommand
{
    #region Constructors

    /// <summary>
    /// 値オブジェクトの生成コマンドを初期化します。
    /// </summary>
    /// <param name="className">クラス名</param>
    /// <param name="classDescription">クラスの説明</param>
    /// <param name="namespaceSequence">名前空間シーケンス</param>
    /// <param name="maximumLength">最大長</param>
    public ValueObjectMaximumLengthGenerationCommand(string className, string classDescription, string namespaceSequence, int maximumLength)
    {
        ClassName = className;
        ClassDescription = classDescription;
        NamespaceSequence = namespaceSequence;
        MaximumLength = maximumLength;
    }

    #endregion

    #region Properties

    /// <summary>
    /// クラス名を取得します。
    /// </summary>
    public string ClassName { get; }

    /// <summary>
    /// クラスの説明を取得します。
    /// </summary>
    public string ClassDescription { get; }

    /// <summary>
    /// 名前空間シーケンスを取得します。
    /// </summary>
    public string NamespaceSequence { get; }

    /// <summary>
    /// 最大長を取得します。
    /// </summary>
    public int MaximumLength { get; }

    #endregion

    #region Methods

    /// <summary>
    /// 現在のオブジェクトを表す文字列を返します。
    /// </summary>
    /// <returns>現在のオブジェクトを表す文字列。</returns>
    public override string ToString()
    {
        string str = $"{nameof(ValueObjectMaximumLengthGenerationCommand)} {{ {nameof(ClassName)} = {ClassName}, {nameof(ClassDescription)} = {ClassDescription}, {nameof(NamespaceSequence)} = {NamespaceSequence}, {nameof(MaximumLength)} = {MaximumLength} }}";

        return str;
    }

    #endregion
}
