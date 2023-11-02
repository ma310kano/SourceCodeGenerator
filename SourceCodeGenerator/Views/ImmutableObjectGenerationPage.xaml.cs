using Microsoft.UI.Xaml.Controls;

using SourceCodeGenerator.ViewModels;

namespace SourceCodeGenerator.Views;

public sealed partial class ImmutableObjectGenerationPage : Page
{
    public ImmutableObjectGenerationViewModel ViewModel
    {
        get;
    }

    public ImmutableObjectGenerationPage()
    {
        ViewModel = App.GetService<ImmutableObjectGenerationViewModel>();
        InitializeComponent();
    }
}
