using Eksaminatoren_Maui.ViewModels;

namespace Eksaminatoren_Maui.Views
{
    public partial class StudentView : ContentPage
    {
        private StudentViewModel _viewModel;

        public StudentView(StudentViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadStudentsAsync();
        }
    }
}
