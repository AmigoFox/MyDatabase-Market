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
            var page = App.Services.GetService<DatabaseCalculator>();
            await Navigation.PushAsync(page);
        }

    }
}