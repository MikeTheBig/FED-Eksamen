using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eksaminatoren_Maui.Data;
using Eksaminatoren_Maui.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Eksaminatoren_Maui.ViewModels;

public partial class HistoryViewModel : ObservableObject
{
    private readonly DatabaseService _database;

    [ObservableProperty]
    private ObservableCollection<Exam> exams = new();

    [ObservableProperty]
    private Exam? selectedExam;

    [ObservableProperty]
    private ObservableCollection<ExamResultWithStudent> examResults = new();

    [ObservableProperty]
    private double averageGrade;

    [ObservableProperty]
    private bool isExamSelected;

    public HistoryViewModel(DatabaseService database)
    {
        _database = database;
    }

    partial void OnSelectedExamChanged(Exam? value)
    {
        if (value != null)
        {
            _ = LoadExamResultsAsync(value.Id);
            IsExamSelected = true;
        }
        else
        {
            ExamResults.Clear();
            AverageGrade = 0;
            IsExamSelected = false;
        }
    }

    [RelayCommand]
    public async Task LoadExamsAsync()
    {
        var examList = await _database.GetExamsAsync();
        Exams.Clear();
        foreach (var exam in examList)
        {
            Exams.Add(exam);
        }
    }

    private async Task LoadExamResultsAsync(int examId)
    {
        var results = await _database.GetResultsByExamAsync(examId);
        var resultWithStudents = new ObservableCollection<ExamResultWithStudent>();
        foreach (var result in results)
        {
            var student = (await _database.GetStudentsByExamAsync(examId))
                .FirstOrDefault(s => s.Id == result.StudentId);
            if (student != null)
            {
                resultWithStudents.Add(new ExamResultWithStudent
                {
                    StudentName = student.Name,
                    StudentNumber = student.StudentNumber,
                    Grade = result.Grade,
                    Notes = result.Notes ?? string.Empty,
                    QuestionNumber = result.QuestionNumber,
                    ActualDurationMinutes = result.ActualDurationMinutes
                });
            }
        }
        ExamResults = resultWithStudents;
        AverageGrade = resultWithStudents.Any() ? resultWithStudents.Average(r => r.Grade) : 0;
    }
}

public class ExamResultWithStudent
{
    public string? StudentName { get; set; }
    public string? StudentNumber { get; set; }
    public double Grade { get; set; }
    public string? Notes { get; set; }
    public int QuestionNumber { get; set; }
    public int ActualDurationMinutes { get; set; }
}