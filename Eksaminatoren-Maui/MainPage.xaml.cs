namespace Eksaminatoren_Maui
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}
		private async void OnGoToExamsClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync("//Eksamener"); // Skal matche route

			// defineret i AppShell.xaml
    }
    }
}