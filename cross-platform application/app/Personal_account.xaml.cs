namespace app;

public partial class Personal_account : ContentPage
{
    public Personal_account()
    {
        InitializeComponent();
    }

    private async void OnOrdersTapped(object sender, TappedEventArgs e)
    {
        var page = App.Services.GetService<OrdersPage>();
        await Navigation.PushAsync(page);
    }

    private async void OnPaymentsTapped(object sender, TappedEventArgs e)
    {
        var page = App.Services.GetService<PaymentsPage>();
        await Navigation.PushAsync(page);
    }

    private async void OnProfileDetailsTapped(object sender, TappedEventArgs e)
    {
        var page = App.Services.GetService<ProfilePage>();
        await Navigation.PushAsync(page);
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Logout", "You have been logged out.", "OK");

        var page = App.Services.GetService<LoginPage>();
        await Navigation.PushAsync(page);
    }
}
