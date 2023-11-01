using CommunityToolkit.Mvvm.ComponentModel;

namespace SourceCodeGenerator.ViewModels;

/// <summary>
/// プロパティ項目(ViewModel)
/// </summary>
public partial class PropertyItemViewModel : ObservableRecipient
{
    #region Fields

    /// <summary>
    /// プロパティ名
    /// </summary>
    [ObservableProperty]
    private string _propertyName = string.Empty;

    /// <summary>
    /// プロパティの説明
    /// </summary>
    [ObservableProperty]
    private string _propertyDescription = string.Empty;

    /// <summary>
    /// 型名
    /// </summary>
    [ObservableProperty]
    private string _typeName = string.Empty;

    #endregion

    #region Methods

    /// <summary>
    /// 現在のオブジェクトを表す文字列を返します。
    /// </summary>
    /// <returns>現在のオブジェクトを表す文字列。</returns>
    public override string ToString() => $"{nameof(PropertyItemViewModel)} {{ {nameof(PropertyName)} = {PropertyName}, {nameof(PropertyDescription)} = {PropertyDescription}, {nameof(TypeName)} = {TypeName} }}";

    #endregion
}
