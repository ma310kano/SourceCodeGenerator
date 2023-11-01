namespace SourceCodeGeneration.Application.Command;

/// <summary>
/// プロパティのコマンド
/// </summary>
public class PropertyCommand
{
    #region Constructors

    /// <summary>
    /// プロパティのコマンドを初期化します。
    /// </summary>
    /// <param name="propertyName">プロパティ名</param>
    /// <param name="propertyDescription">プロパティの説明</param>
    /// <param name="typeName">型名</param>
    public PropertyCommand(string propertyName, string propertyDescription, string typeName)
    {
        PropertyName = propertyName;
        PropertyDescription = propertyDescription;
        TypeName = typeName;
    }

    #endregion

    #region Properties

    /// <summary>
    /// プロパティ名を取得します。
    /// </summary>
    public string PropertyName
    {
        get;
    }

    /// <summary>
    /// プロパティの説明を取得します。
    /// </summary>
    public string PropertyDescription
    {
        get;
    }

    /// <summary>
    /// 型名を取得します。
    /// </summary>
    public string TypeName
    {
        get;
    }

    #endregion

    #region Methods

    /// <summary>
    /// 現在のオブジェクトを表す文字列を返します。
    /// </summary>
    /// <returns>現在のオブジェクトを表す文字列。</returns>
    public override string ToString() => $"{nameof(PropertyCommand)} {{ {nameof(PropertyName)} = {PropertyName}, {nameof(PropertyDescription)} = {PropertyDescription}, {nameof(TypeName)} = {TypeName} }}";

    #endregion
}
