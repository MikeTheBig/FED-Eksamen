using SQLite;
using CommunityToolkit.Mvvm.ComponentModel;
using Eksaminatoren_Maui.Models;
using System.Collections.ObjectModel;
using Eksaminatoren_Maui.Data;
using CommunityToolkit.Mvvm.Input;



namespace Eksaminatoren_Maui.ViewModels;
public partial class ExamViewModel : ObservableObject
{
    private readonly DatabaseService _database;

    [ObservableProperty]
    private ObservableCollection<Exam> exams = new();

    // Bindbare properties til inputfelter
    [ObservableProperty]
    private string courseName;

    [ObservableProperty]
    private DateTime date = DateTime.Now;

    [ObservableProperty]
    private int numberOfQuestions;

    [ObservableProperty]
    private int examDurationMinutes;

    [ObservableProperty]
    private string startTime;

    public ExamViewModel(DatabaseService database)
    {
        _database = database;
        _ = LoadExamsAsync();
    }

    [RelayCommand]
    private async Task AddExamAsync()
    {
        var newExam = new Exam
        {
            CourseName = CourseName,
            Date = Date,
            NumberOfQuestions = NumberOfQuestions,
            ExamDurationMinutes = ExamDurationMinutes,
            StartTime = TimeSpan.TryParse(StartTime, out var parsedTime) ? parsedTime : TimeSpan.Zero,
            ExamTermin = "Sommer 25"  // evt. også som input hvis du vil
        };

        await _database.AddExamAsync(newExam);
        Exams.Add(newExam);

        // Ryd inputfelter efter tilføjelse
        CourseName = string.Empty;
        NumberOfQuestions = 0;
        ExamDurationMinutes = 0;
        StartTime = string.Empty;
        Date = DateTime.Now;
    }

    public async Task LoadExamsAsync()
    {
        var list = await _database.GetExamsAsync();
        Exams = new ObservableCollection<Exam>(list);
    }
}

