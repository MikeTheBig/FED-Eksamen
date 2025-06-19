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
    private ObservableCollection<Student> students;

    [ObservableProperty]
    private string studentNumber;

    [ObservableProperty]
    private string name;

    public StudentViewModel(DatabaseService database)
    {
        _database = database;
        Students = new ObservableCollection<Student>();
    }

    [RelayCommand]
    public async Task AddStudentAsync(Student student)
    {
        await _database.AddStudentAsync(student);
        Students.Add(student);
        StudentNumber = string.Empty;
        Name = string.Empty;
    }

    [RelayCommand]
    public async Task LoadStudentsAsync()
    {
        var students = await _database.GetStudentsByExamAsync(1); // Adjust examId as needed
        Students.Clear();
        foreach (var student in students)
        {
            Students.Add(student);
        }
    }
}