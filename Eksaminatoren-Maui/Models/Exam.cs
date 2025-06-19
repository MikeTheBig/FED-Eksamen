using SQLite;

namespace Eksaminatoren_Maui.Models;

public class Exam
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; } 
    public string ExamTermin { get; set; } = string.Empty; // Initialiseret
    public string CourseName { get; set; } = string.Empty; // Initialiseret
    public DateTime Date { get; set; }
    public int NumberOfQuestions { get; set; }
    public int ExamDurationMinutes { get; set; }
    public int DurationMinutes { get; set; }
    public TimeSpan StartTime { get; set; }  // Bemærk TimeSpan til tid på dagen
}
