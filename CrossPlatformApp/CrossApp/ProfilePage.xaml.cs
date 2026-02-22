namespace CrossApp;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
    }

    private async void OnProfileDetailsTapped(object sender, TappedEventArgs e)
    {

        await DisplayAlert("Профиль", "Вы уже на странице профиля.", "OK");
    }

    private async void OnOrdersTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(OrdersPage));
    }

    private async void OnPaymentsTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PaymentsPage));
    }
}
