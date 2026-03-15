using CrossApp.ViewModels;

namespace CrossApp;

public partial class ProfilePage : ContentPage
{
    private readonly ProfileViewModel _viewModel;
    public ProfilePage(ProfileViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
        _viewModel = viewModel;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadUser();
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

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
