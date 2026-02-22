namespace app;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
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
        var page = App.Services.GetService<Personal_account>();
        await Navigation.PushAsync(page);
    }
}
