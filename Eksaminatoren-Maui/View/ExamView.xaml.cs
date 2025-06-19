using Eksaminatoren_Maui.ViewModels;

namespace Eksaminatoren_Maui.Views
{
    public partial class ExamView : ContentPage
    {
        private ExamViewModel _viewModel;

        public ExamView(ExamViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadExamsAsync();
        }
    }
}
