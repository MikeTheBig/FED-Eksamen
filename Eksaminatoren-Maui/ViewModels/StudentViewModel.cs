using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eksaminatoren_Maui.Data;
using Eksaminatoren_Maui.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Eksaminatoren_Maui.ViewModels;


public partial class StudentViewModel : ObservableObject
{
    private readonly DatabaseService _database;

    [ObservableProperty]
    private ObservableCollection<Student> students = new();

    [ObservableProperty]
    private ObservableCollection<Exam> exams = new();

    [ObservableProperty]
    private Exam selectedExam;

    [ObservableProperty]
    private string studentNumber;

    [ObservableProperty]
    private string name;

    public StudentViewModel(DatabaseService database)
    {
        _database = database;
    }

    [RelayCommand]
    public async Task LoadStudentsAsync()
    {
        if (SelectedExam == null)
        {
            // Hvis ingen eksamen valgt, kan man f.eks. hente alle studerende eller ingen
            Students.Clear();
            return;
        }

        var studentsFromDb = await _database.GetStudentsByExamAsync(SelectedExam.Id);
        Students.Clear();
        foreach (var student in studentsFromDb)
            Students.Add(student);
    }

    [RelayCommand]
    public async Task LoadExamsAsync()
    {
        var examsFromDb = await _database.GetExamsAsync();
        Exams.Clear();
        foreach (var exam in examsFromDb)
            Exams.Add(exam);

        // Sæt automatisk valgt eksamen til den første, hvis muligt
        if (Exams.Count > 0)
        {
            SelectedExam = Exams[0];
            await LoadStudentsAsync();
        }
    }

    [RelayCommand]
    public async Task AddStudentAsync()
    {
        if (SelectedExam == null)
        {
            await Application.Current.MainPage.DisplayAlert("Fejl", "Vælg en eksamen først.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(StudentNumber))
            return;

        var newStudent = new Student
        {
            Name = Name,
            StudentNumber = StudentNumber,
            ExamId = SelectedExam.Id,
            Order = Students.Count + 1
        };

        var result = await _database.AddStudentAsync(newStudent);
        if (result > 0)
        {
            Students.Add(newStudent);
            Name = string.Empty;
            StudentNumber = string.Empty;
        }
    }

    partial void OnSelectedExamChanged(Exam value)
    {
        // Når valgt eksamen ændres, load studerende for den eksamen
        _ = LoadStudentsAsync();
    }
}