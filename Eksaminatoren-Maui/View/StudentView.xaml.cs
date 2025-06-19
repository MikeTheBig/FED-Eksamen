using Eksaminatoren_Maui.Data;
using Eksaminatoren_Maui.ViewModels;
using Eksaminatoren_Maui.Models;
using Microsoft.Maui.Storage;

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

        private async void OnAddStudentClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_viewModel.StudentNumber) && !string.IsNullOrWhiteSpace(_viewModel.Name))
            {
                await _viewModel.AddStudentAsync(new Student
                {
                    StudentNumber = _viewModel.StudentNumber,
                    Name = _viewModel.Name
                });
                _viewModel.StudentNumber = string.Empty;
                _viewModel.Name = string.Empty;
            }
            else
            {
                await DisplayAlert("Fejl", "Indtast både studienummer og navn.", "OK");
            }
        }
    }
}