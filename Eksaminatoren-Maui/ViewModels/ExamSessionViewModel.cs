using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eksaminatoren_Maui.Models;
using Eksaminatoren_Maui.Data;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Eksaminatoren_Maui.ViewModels;

public partial class ExamSessionViewModel : ObservableObject
{
    private readonly DatabaseService _database;
    private CancellationTokenSource _cts;
    private DateTime _examStartTime;

    public ExamSessionViewModel(DatabaseService database)
    {
        _database = database;
        GradeOptions = new ObservableCollection<double> { 0, 2, 4, 7, 10, 12 };
    }

    [ObservableProperty]
    private Student currentStudent;

    [ObservableProperty]
    private ObservableCollection<Student> students = new();

    [ObservableProperty]
    private Exam selectedExam;

    [ObservableProperty]
    private ObservableCollection<Exam> exams = new();

    [ObservableProperty]
    private int? drawnQuestionNumber;

    [ObservableProperty]
    private TimeSpan remainingTime;

    public string RemainingTimeDisplay => RemainingTime.ToString(@"mm\:ss");

    [ObservableProperty]
    private string notes;

    [ObservableProperty]
    private double? selectedGrade;

    public ObservableCollection<double> GradeOptions { get; }

    [ObservableProperty]
    private bool canStart = true;

    [ObservableProperty]
    private bool canStop = false;

    [RelayCommand]
    public async Task LoadExamsAsync()
    {
        var examList = await _database.GetExamsAsync();
        Exams = new ObservableCollection<Exam>(examList);
    }

    partial void OnSelectedExamChanged(Exam value)
    {
        if (value != null)
        {
            _ = LoadStudentsAsync(value.Id);
        }
        else
        {
            Students.Clear();
            CurrentStudent = null;
        }
    }

    private async Task LoadStudentsAsync(int examId)
    {
        var studentList = await _database.GetStudentsByExamAsync(examId);
        Students = new ObservableCollection<Student>(studentList);

        CurrentStudent = Students.Count > 0 ? Students[0] : null;
    }
    
    [RelayCommand]
    public async Task LoadExamSessionAsync(int examId)
    {
        SelectedExam = await _database.GetExamByIdAsync(examId);
        var studentsFromDb = await _database.GetStudentsByExamAsync(examId);
        Students.Clear();

        foreach (var s in studentsFromDb)
            Students.Add(s);

        if (Students.Count > 0)
            CurrentStudent = Students[0];
    }

    [RelayCommand]
    public void DrawQuestion()
    {
        if (SelectedExam == null || SelectedExam.NumberOfQuestions <= 0)
            return;

        var random = new Random();
        DrawnQuestionNumber = random.Next(1, SelectedExam.NumberOfQuestions + 1);
    }

    [RelayCommand]
    public async Task StartExamAsync()
    {
        if (SelectedExam == null)
            return;

        RemainingTime = TimeSpan.FromMinutes(SelectedExam.ExamDurationMinutes);
        OnPropertyChanged(nameof(RemainingTimeDisplay));

        CanStart = false;
        CanStop = true;
        _examStartTime = DateTime.Now;

        _cts = new CancellationTokenSource();

        while (RemainingTime.TotalSeconds > 0 && !_cts.IsCancellationRequested)
        {
            await Task.Delay(1000);
            RemainingTime -= TimeSpan.FromSeconds(1);
            OnPropertyChanged(nameof(RemainingTimeDisplay));
        }

        if (RemainingTime.TotalSeconds <= 0)
        {
        }
    }

    [RelayCommand]
    public void EndExam()
    {
        _cts?.Cancel();
        CanStart = true;
        CanStop = false;
    }

    [RelayCommand]
    public async Task SaveResultAsync()
    {
        if (SelectedExam == null || CurrentStudent == null || SelectedGrade == null)
            return;

        var actualDuration = (int)(DateTime.Now - _examStartTime).TotalMinutes;

        var result = new ExamResult
        {
            StudentId = CurrentStudent.Id,
            ExamId = SelectedExam.Id,
            QuestionNumber = DrawnQuestionNumber ?? 0,
            Notes = Notes,
            Grade = SelectedGrade.Value,
            ActualDurationMinutes = actualDuration
        };

        await _database.SaveExamResultAsync(result);

        Notes = string.Empty;
        SelectedGrade = null;
        DrawnQuestionNumber = null;
    }

    [RelayCommand]
    public void NextStudent()
    {
        if (Students.Count == 0 || CurrentStudent == null)
            return;

        int currentIndex = Students.IndexOf(CurrentStudent);
        if (currentIndex < Students.Count - 1)
        {
            CurrentStudent = Students[currentIndex + 1];
        }
        else
        {
            Application.Current.MainPage.DisplayAlert("Info", "Der er ikke flere studerende.", "OK");
        }

        Notes = string.Empty;
        SelectedGrade = null;
        DrawnQuestionNumber = null;
    }

    [RelayCommand]
    public void PreviousStudent()
    {
        if (Students.Count == 0 || CurrentStudent == null)
            return;

        int currentIndex = Students.IndexOf(CurrentStudent);
        if (currentIndex > 0)
            CurrentStudent = Students[currentIndex - 1];
    }
}
