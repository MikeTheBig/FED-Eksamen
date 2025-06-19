using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eksaminatoren_Maui.Data;
using Eksaminatoren_Maui.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Eksaminatoren_Maui.ViewModels
{
    public partial class StudentViewModel : ObservableObject
    {
        private readonly DatabaseService _database;

        [ObservableProperty]
        private ObservableCollection<Student> students = new();

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
            var studentsFromDb = await _database.GetStudentsByExamAsync(1); // <-- evt. dynamisk examId senere
            Students.Clear();
            foreach (var student in studentsFromDb)
            {
                Students.Add(student);
            }
        }

        [RelayCommand]
        public async Task AddStudentAsync()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(StudentNumber))
                return;

            var newStudent = new Student
            {
                Name = Name,
                StudentNumber = StudentNumber,
                ExamId = 1, // <-- Gør dynamisk senere!
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
    }
}
