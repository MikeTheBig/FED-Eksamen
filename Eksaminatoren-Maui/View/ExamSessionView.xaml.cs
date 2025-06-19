using Eksaminatoren_Maui.ViewModels;
using Eksaminatoren_Maui.Data;

namespace Eksaminatoren_Maui.Views;

public partial class ExamSessionView : ContentPage
{
    private readonly ExamSessionViewModel _viewModel;

    public ExamSessionView(DatabaseService database)
    {
        InitializeComponent();
        _viewModel = new ExamSessionViewModel(database);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadExamsAsync();
    }

    public async Task StartSessionAsync(int examId)
    {
        await _viewModel.LoadExamSessionAsync(examId);
    }
}
