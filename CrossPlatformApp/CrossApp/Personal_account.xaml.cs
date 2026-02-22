namespace CrossApp;
public partial class Personal_account : ContentPage
{
    public Personal_account()
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

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
