namespace app;

public partial class PaymentsPage : ContentPage
{
    public PaymentsPage()
    {
        InitializeComponent();
    }

    private async void OnOrdersTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//OrdersPage");
    }

    private async void OnPaymentsTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//PaymentsPage");
    }

    private async void OnProfileDetailsTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//ProfilePage");
    }

}
