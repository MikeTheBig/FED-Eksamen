using SQLite;

namespace Eksaminatoren_Maui.Models;

public class ExamResult
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int ExamId { get; set; }
    public int StudentId { get; set; }

    public double Grade { get; set; }
    public string? Notes { get; set; }

    public int QuestionNumber { get; set; }
    public int ActualDurationMinutes { get; set; }
}