namespace app;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
    }

    private async void OnProfileDetailsTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new Personal_account());
    }

    private async void OnOrdersTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new OrdersPage());
    }

    private async void OnPaymentsTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new PaymentsPage());
    }
}
