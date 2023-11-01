using Microsoft.UI.Xaml.Controls;

using SourceCodeGenerator.ViewModels;

namespace SourceCodeGenerator.Views;

public sealed partial class ImmutableClassGenerationPage : Page
{
    public ImmutableClassGenerationViewModel ViewModel
    {
        get;
    }

    public ImmutableClassGenerationPage()
    {
        ViewModel = App.GetService<ImmutableClassGenerationViewModel>();
        InitializeComponent();
    }
}
