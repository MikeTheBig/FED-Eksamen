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
    private string examTermin = string.Empty;

    [ObservableProperty]
    private string courseName = string.Empty;

    [ObservableProperty]
    private string numberOfQuestions = string.Empty;

    [ObservableProperty]
    private string examDurationMinutes = string.Empty;

    [ObservableProperty]
    private TimeSpan startTime = TimeSpan.Zero;

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
        // Grundlæggende validering
        if (string.IsNullOrWhiteSpace(ExamTermin) || 
            string.IsNullOrWhiteSpace(CourseName) || 
            StartTime == TimeSpan.Zero)
        {
            await Application.Current.MainPage.DisplayAlert("Fejl", "Udfyld venligst alle felter.", "OK");
            return;
        }

        if (!int.TryParse(NumberOfQuestions, out int parsedNumberOfQuestions) || parsedNumberOfQuestions <= 0)
        {
            await Application.Current.MainPage.DisplayAlert("Fejl", "Indtast et gyldigt antal spørgsmål.", "OK");
            return;
        }

        if (!int.TryParse(ExamDurationMinutes, out int parsedExamDurationMinutes) || parsedExamDurationMinutes <= 0)
        {
            await Application.Current.MainPage.DisplayAlert("Fejl", "Indtast en gyldig eksamenstid.", "OK");
            return;
        }

        if (StartTime == TimeSpan.Zero)
        {
            await Application.Current.MainPage.DisplayAlert("Fejl", "Indtast et gyldigt starttidspunkt.", "OK");
            return;
        }

        var exam = new Exam
        {
            ExamTermin = ExamTermin,
            CourseName = CourseName,
            Date = Date,
            NumberOfQuestions = parsedNumberOfQuestions,
            ExamDurationMinutes = parsedExamDurationMinutes,
            StartTime = StartTime
        };

        try
        {
            await _database.AddExamAsync(exam);
            await LoadExamsAsync();

            // Nulstil inputfelter efter succesfuld gemning
            ExamTermin = string.Empty;
            CourseName = string.Empty;
            NumberOfQuestions = string.Empty;
            ExamDurationMinutes = string.Empty;
            StartTime = TimeSpan.Zero;
            Date = DateTime.Today;

            await Application.Current.MainPage.DisplayAlert("Succes", "Eksamen gemt!", "OK");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Fejl", $"Kunne ikke gemme eksamen: {ex.Message}", "OK");
        }
    }

    public bool IsExamSelected => SelectedExam != null;

    partial void OnSelectedExamChanged(Exam value)
    {
        OnPropertyChanged(nameof(IsExamSelected));
    }
}
