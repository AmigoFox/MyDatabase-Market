namespace app;

public partial class Personal_account : ContentPage
{
    public Personal_account()
    {
        InitializeComponent();
    }

    private async void OnProfileDetailsTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage());
    }

    private async void OnOrdersTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new OrdersPage());
    }

    private async void OnPaymentsTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new PaymentsPage());
    }

    
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Logout", "You have been logged out.", "OK");
    }

}
