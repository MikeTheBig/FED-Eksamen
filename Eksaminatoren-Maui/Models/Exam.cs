using SQLite;

namespace Eksaminatoren_Maui.Models;

public class Exam
{
    public int Id { get; set; }  // Hvis du bruger string id (evt. Guid)
    public string ExamTermin { get; set; } = string.Empty; // Initialiseret
    public string CourseName { get; set; } = string.Empty; // Initialiseret
    public DateTime Date { get; set; }
    public int NumberOfQuestions { get; set; }
    public int ExamDurationMinutes { get; set; }
    public int DurationMinutes { get; set; }
    public TimeSpan StartTime { get; set; }  // Bemærk TimeSpan til tid på dagen
}
