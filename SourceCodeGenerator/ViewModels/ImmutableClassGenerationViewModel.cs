using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SourceCodeGeneration.Application;
using SourceCodeGeneration.Application.Command;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace SourceCodeGenerator.ViewModels;

/// <summary>
/// 不変クラス生成(ViewModel)
/// </summary>
public partial class ImmutableClassGenerationViewModel : ObservableRecipient
{
    #region Fields

    /// <summary>
    /// ソースコード生成サービス
    /// </summary>
    private readonly ISourceCodeGenerationService _sourceCodeGenerationService;

    /// <summary>
    /// クラス名
    /// </summary>
    [ObservableProperty]
    private string _className = string.Empty;

    /// <summary>
    /// クラスの説明
    /// </summary>
    [ObservableProperty]
    private string _classDescription = string.Empty;

    /// <summary>
    /// 名前空間シーケンス
    /// </summary>
    [ObservableProperty]
    private string _namespaceSequence = string.Empty;

    /// <summary>
    /// プロパティ項目のコレクション
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<PropertyItemViewModel> _propertyItems = new();

    /// <summary>
    /// 選択されたプロパティ項目
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeletePropertyCommand))]
    private PropertyItemViewModel? _selectedPropertyItem;

    #endregion

    #region Constructors

    /// <summary>
    /// 不変クラス生成(ViewModel)を初期化します。
    /// </summary>
    /// <param name="sourceCodeGenerationService">ソースコード生成サービス</param>
    public ImmutableClassGenerationViewModel(ISourceCodeGenerationService sourceCodeGenerationService)
    {
        _sourceCodeGenerationService = sourceCodeGenerationService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// プロパティを追加します。
    /// </summary>
    [RelayCommand]
    private void AddProperty()
    {
        var propertyItem = new PropertyItemViewModel();

        PropertyItems.Add(propertyItem);    
    }

    /// <summary>
    /// プロパティを削除します。
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanDeleteProperty))]
    private void DeleteProperty()
    {
        if (SelectedPropertyItem is null)
        {
            return;
        }

        PropertyItems.Remove(SelectedPropertyItem);
        SelectedPropertyItem = null;
    }

    /// <summary>
    /// プロパティが削除できるか判定します。
    /// </summary>
    /// <returns>削除可能な場合は、 <c>true</c>。それ以外の場合は、 <c>false</c>。</returns>
    private bool CanDeleteProperty() => SelectedPropertyItem is not null;

    /// <summary>
    /// 不変クラスを生成します。
    /// </summary>
    [RelayCommand]
    private async Task GenerateImmutableClass()
    {
        var savePicker = new FileSavePicker();
        savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        savePicker.FileTypeChoices.Add("C# File", new List<string>() { ".cs" });
        savePicker.SuggestedFileName = ClassName;

        {
            var hWnd = WindowNative.GetWindowHandle(App.MainWindow);
            InitializeWithWindow.Initialize(savePicker, hWnd);
        }

        var file = await savePicker.PickSaveFileAsync();
        if (file is null)
        {
            return;
        }

        ImmutableClassGenerationCommand command;
        {
            var properties = PropertyItems.Select(item => new PropertyCommand(item.PropertyName, item.PropertyDescription, item.TypeName)).ToList();

            command = new ImmutableClassGenerationCommand(ClassName, ClassDescription, file.Path, NamespaceSequence, properties);
        }

        await _sourceCodeGenerationService.GenerateImmutableClassAsync(command);
    }

    #endregion
}
