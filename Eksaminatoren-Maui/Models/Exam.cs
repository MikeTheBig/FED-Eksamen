namespace Eksaminatoren_Maui.Models
{
public class Exam
{
    public int Id { get; set; }
    public string Term { get; set; }  
    public string CourseName { get; set; }     
    public DateTime Date { get; set; }         
    public int QuestionCount { get; set; }      
    public int DurationMinutes { get; set; }     
    public TimeSpan StartTime { get; set; }     
}

    
}