using Eksaminatoren_Maui.Models;

namespace Eksaminatoren_Maui.Views;

public partial class ExamSessionView : ContentPage
{
    public Student? Student { get; private set; }

    public ExamSessionView()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public void SetStudent(Student student)
    {
        Student = student;
        OnPropertyChanged(nameof(Student));
    }
}