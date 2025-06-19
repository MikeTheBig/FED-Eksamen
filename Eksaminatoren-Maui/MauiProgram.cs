using Eksaminatoren_Maui;
using Eksaminatoren_Maui.Views;
using Eksaminatoren_Maui.ViewModels;
using Eksaminatoren_Maui.Data;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
		string dbPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "eksaminatoren.db3");
        builder.Services.AddSingleton(new DatabaseService(dbPath));

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });


        
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<ExamView>();
        builder.Services.AddSingleton<StudentView>();
        builder.Services.AddSingleton<HistoryView>();

        builder.Services.AddSingleton<ExamViewModel>();
        builder.Services.AddSingleton<StudentViewModel>();
        builder.Services.AddSingleton<HistoryViewModel>();
		builder.Services.AddTransient<ExamSessionView>();

        return builder.Build();
    }
}
