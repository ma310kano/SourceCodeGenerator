namespace SourceCodeGeneration.Application.Command;

/// <summary>
/// エンティティの生成コマンド
/// </summary>
public class EntityGenerationCommand
{
    #region Constructors

    /// <summary>
    /// エンティティの生成コマンドを初期化します。
    /// </summary>
    /// <param name="className">クラス名</param>
    /// <param name="classDescription">クラスの説明</param>
    /// <param name="namespaceSequence">名前空間シーケンス</param>
    /// <param name="properties">プロパティのコレクション</param>
    public EntityGenerationCommand(string className, string classDescription, string namespaceSequence, IReadOnlyList<PropertyCommand> properties)
    {
        ClassName = className;
        ClassDescription = classDescription;
        NamespaceSequence = namespaceSequence;
        Properties = properties;
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
    /// プロパティのコレクションを取得します。
    /// </summary>
    public IReadOnlyList<PropertyCommand> Properties { get; }

    #endregion

    #region Methods

    /// <summary>
    /// 現在のオブジェクトを表す文字列を返します。
    /// </summary>
    /// <returns>現在のオブジェクトを表す文字列。</returns>
    public override string ToString()
    {
        return $"{nameof(EntityGenerationCommand)} {{ {nameof(ClassName)} = {ClassName}, {nameof(ClassDescription)} = {ClassDescription}, {nameof(NamespaceSequence)} = {NamespaceSequence}, {nameof(Properties)} = {Properties} }}";
    }

    #endregion
}
