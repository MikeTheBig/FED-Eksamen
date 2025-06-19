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

    public Task<Exam> GetExamByIdAsync(int examId) =>
        _database.Table<Exam>().Where(e => e.Id == examId).FirstOrDefaultAsync();

    public Task<int> SaveExamResultAsync(ExamResult result)
        {
            if (result.Id != 0)
            {
                return _database.UpdateAsync(result);
            }
            else
            {
                return _database.InsertAsync(result);
            }
        }

}