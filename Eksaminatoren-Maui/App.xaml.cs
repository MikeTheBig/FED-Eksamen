using Eksaminatoren_Maui.Data;

namespace Eksaminatoren_Maui;

public partial class App : Application
{
    private readonly DatabaseService _databaseService;

    public App(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        Task.Run(async () => await _databaseService.SeedTestDataAsync()).Wait();
        MainPage = new AppShell();
    }
}