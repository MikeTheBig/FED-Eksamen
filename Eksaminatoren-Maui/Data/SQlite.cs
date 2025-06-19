using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eksaminatoren_Maui.Models;

namespace Eksaminatoren_Maui.Data;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Exam>().Wait();
        _database.CreateTableAsync<Student>().Wait();
        _database.CreateTableAsync<ExamResult>().Wait();
    }

    public Task<List<Exam>> GetExamsAsync() => _database.Table<Exam>().ToListAsync();
    public Task<int> AddExamAsync(Exam exam) => _database.InsertAsync(exam);

    public Task<List<Student>> GetStudentsByExamAsync(int examId) =>
        _database.Table<Student>().Where(s => s.ExamId == examId).ToListAsync();

    public Task<int> AddStudentAsync(Student student) =>
            _database.InsertAsync(student);
    public Task<List<ExamResult>> GetResultsByStudentAsync(int studentId) =>
        _database.Table<ExamResult>().Where(r => r.StudentId == studentId).ToListAsync();

    public Task<List<ExamResult>> GetResultsByExamAsync(int examId) =>
        _database.Table<ExamResult>().Where(r => r.ExamId == examId).ToListAsync();

    public Task<int> AddExamResultAsync(ExamResult result) => _database.InsertAsync(result);

    public async Task SeedTestDataAsync()
{
    await AddExamAsync(new Exam
    {
        CourseName = "Matematik",
        Date = DateTime.Now,
        ExamTermin = "Forår 2025",
        NumberOfQuestions = 10,
        ExamDurationMinutes = 60,
        StartTime = TimeSpan.Parse("09:00")
    });
    await AddExamAsync(new Exam
    {
        CourseName = "Fysik",
        Date = DateTime.Now.AddDays(-10),
        ExamTermin = "Forår 2025",
        NumberOfQuestions = 8,
        ExamDurationMinutes = 45,
        StartTime = TimeSpan.Parse("10:00")
    });
    await AddStudentAsync(new Student
    {
        ExamId = 1,
        Name = "Anna Hansen",
        StudentNumber = "12345",
        Order = 1
    });
    await AddStudentAsync(new Student
    {
        ExamId = 1,
        Name = "Bob Jensen",
        StudentNumber = "67890",
        Order = 2
    });
    await AddStudentAsync(new Student
    {
        ExamId = 2,
        Name = "Clara Olsen",
        StudentNumber = "54321",
        Order = 1
    });
    await AddExamResultAsync(new ExamResult
    {
        ExamId = 1,
        StudentId = 1,
        Grade = 7.5,
        Notes = "God præstation",
        QuestionNumber = 1,
        ActualDurationMinutes = 30
    });
    await AddExamResultAsync(new ExamResult
    {
        ExamId = 1,
        StudentId = 2,
        Grade = 9.0,
        Notes = "Fremragende",
        QuestionNumber = 2,
        ActualDurationMinutes = 25
    });
    await AddExamResultAsync(new ExamResult
    {
        ExamId = 2,
        StudentId = 3,
        Grade = 6.5,
        Notes = "Acceptabel",
        QuestionNumber = 1,
        ActualDurationMinutes = 35
    });
 }
}