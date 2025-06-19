using SQLite;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Eksaminatoren_Maui.Models;

public partial class Student : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int ExamId { get; set; }
    public int Order { get; set; }

    [ObservableProperty]
    private string? studentNumber;

    [ObservableProperty]
    private string? name;
}