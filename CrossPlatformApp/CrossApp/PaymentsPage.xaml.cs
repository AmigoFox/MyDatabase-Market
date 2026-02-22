namespace CrossApp;

public partial class PaymentsPage : ContentPage
{
    public PaymentsPage()
    {
        InitializeComponent();
    }

    private async void OnOrdersTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(OrdersPage));
    }

    private async void OnPaymentsTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PaymentsPage));
    }

    private async void OnProfileDetailsTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ProfilePage));
    }

}
