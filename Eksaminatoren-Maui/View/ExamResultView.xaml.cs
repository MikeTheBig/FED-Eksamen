using Eksaminatoren_Maui.Data;
using Eksaminatoren_Maui.ViewModels;
using Microsoft.Maui.Storage;

namespace Eksaminatoren_Maui.Views
{
    public partial class ExamResultView : ContentPage
    {
        public ExamResultView(ExamResultViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
