using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eksaminatoren_Maui.Models; 
using Eksaminatoren_Maui.Data;
using System.Collections.ObjectModel;

namespace Eksaminatoren_Maui.ViewModels;

public partial class ExamResultViewModel : ObservableObject
{
    private readonly DatabaseService _database;

    [ObservableProperty]
    private ObservableCollection<Student> students = new();

    [ObservableProperty]
    private int currentStudentIndex = 0;

    [ObservableProperty]
    private Student currentStudent;

    [ObservableProperty]
    private int randomQuestionNumber;

    [ObservableProperty]
    private bool isTimerRunning;

    [ObservableProperty]
    private TimeSpan timeRemaining;

    [ObservableProperty]
    private string notes;

    [ObservableProperty]
    private int grade;

    private Exam _exam;

    private System.Timers.Timer _timer;

    public ExamResultViewModel(DatabaseService database, Exam exam)
    {
        _database = database;
        _exam = exam;
    }

    public async Task InitializeAsync()
    {
        var list = await _database.GetStudentsByExamAsync(_exam.Id);
        Students = new ObservableCollection<Student>(list);
        if (Students.Count > 0)
            CurrentStudent = Students[0];
    }

    [RelayCommand]
public void DrawQuestion()
{
    var rnd = new Random();
    RandomQuestionNumber = rnd.Next(1, _exam.NumberOfQuestions + 1);
}

    [RelayCommand]
    public void StartExamination()
    {
        TimeRemaining = TimeSpan.FromMinutes(_exam.ExamDurationMinutes);
        IsTimerRunning = true;

        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += TimerElapsed;
        _timer.Start();
    }

    private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        TimeRemaining = TimeRemaining.Subtract(TimeSpan.FromSeconds(1));
        if (TimeRemaining <= TimeSpan.Zero)
        {
            _timer.Stop();
            IsTimerRunning = false;
            // TODO: Play sound or notify user that time is over
        }
    }

    [RelayCommand]
    public async Task EndExaminationAsync()
    {
        if (_timer != null)
        {
            _timer.Stop();
            IsTimerRunning = false;
        }

        var result = new ExamResult
        {
            StudentId = CurrentStudent.Id,
            QuestionNumber = RandomQuestionNumber,
            ActualDurationMinutes = _exam.ExamDurationMinutes - (int)TimeRemaining.TotalMinutes,
            Notes = Notes,
            Grade = Grade
        };

        await _database.AddExamResultAsync(result);
    }

    [RelayCommand]
    public void NextStudent()
    {
        if (CurrentStudentIndex + 1 < Students.Count)
        {
            CurrentStudentIndex++;
            CurrentStudent = Students[CurrentStudentIndex];

            // Reset state for next student
            RandomQuestionNumber = 0;
            Notes = string.Empty;
            Grade = 0;
            TimeRemaining = TimeSpan.Zero;
        }
        else
        {
            // Eksamen er færdig - evt. vis besked til bruger
        }
    }
}
