using Eksaminatoren_Maui.ViewModels;

namespace Eksaminatoren_Maui.Views;

public partial class HistoryView : ContentPage
{
    private readonly HistoryViewModel _viewModel;

    public HistoryView(HistoryViewModel viewModel)
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