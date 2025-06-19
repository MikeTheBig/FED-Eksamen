using Eksaminatoren_Maui.Data;
using Eksaminatoren_Maui.ViewModels;
using Microsoft.Maui.Storage;

namespace Eksaminatoren_Maui.Views
{
    public partial class ExamView : ContentPage
{
    public ExamView(ExamViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

}
