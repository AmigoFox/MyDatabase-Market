namespace app
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCalculatorClicked(object? sender, EventArgs e)
        {
            await DisplayAlert("Уведомление", "Уже скоро", "ок");
        }
    }
}
