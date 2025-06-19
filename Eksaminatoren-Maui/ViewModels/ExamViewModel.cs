using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eksaminatoren_Maui.Data;
using Eksaminatoren_Maui.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Eksaminatoren_Maui.ViewModels;

public partial class ExamViewModel : ObservableObject
{
    private readonly DatabaseService _database;

    [ObservableProperty]
    private ObservableCollection<Exam> exams = new();

    [ObservableProperty]
    private string courseName = string.Empty;

    [ObservableProperty]
    private string numberOfQuestions = string.Empty;

    [ObservableProperty]
    private string examDurationMinutes = string.Empty;

    [ObservableProperty]
    private string startTime = string.Empty; // Gemt som string, parse senere

    [ObservableProperty]
    private DateTime date = DateTime.Today;

    [ObservableProperty]
    private Exam selectedExam;

    public ExamViewModel(DatabaseService database)
    {
        _database = database;
    }

    [RelayCommand]
    public async Task LoadExamsAsync()
    {
        Exams.Clear();
        var examsFromDb = await _database.GetExamsAsync();
        foreach (var exam in examsFromDb)
        {
            Exams.Add(exam);
        }
    }

    [RelayCommand]
    public async Task AddExamAsync()
    {
        if (string.IsNullOrWhiteSpace(CourseName) || string.IsNullOrWhiteSpace(StartTime))
            return;

        if (!int.TryParse(NumberOfQuestions, out int parsedNumberOfQuestions) ||
            !int.TryParse(ExamDurationMinutes, out int parsedExamDurationMinutes))
        {
            // Her kan du evt. vise fejl til bruger, fx med DisplayAlert (kræver eventuelt reference til side)
            return;
        }

        if (!TimeSpan.TryParse(StartTime, out TimeSpan parsedStartTime))
        {
            // Fejl i tidspunkt
            return;
        }

        var exam = new Exam
        {
            CourseName = CourseName,
            Date = Date,
            ExamTermin = Date.Month <= 6 ? "Forår" : "Efterår",
            NumberOfQuestions = parsedNumberOfQuestions,
            ExamDurationMinutes = parsedExamDurationMinutes,
            StartTime = parsedStartTime
        };

        await _database.AddExamAsync(exam);

        await LoadExamsAsync();

        // Ryd inputfelter
        CourseName = string.Empty;
        NumberOfQuestions = string.Empty;
        ExamDurationMinutes = string.Empty;
        StartTime = string.Empty;
        Date = DateTime.Today;
    }

    public bool IsExamSelected => SelectedExam != null;

    partial void OnSelectedExamChanged(Exam value)
    {
        OnPropertyChanged(nameof(IsExamSelected));
    }
}
