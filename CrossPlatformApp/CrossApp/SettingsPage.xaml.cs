using CrossApp.ViewModels;
using CrossApp.Services;

namespace CrossApp;
public partial class SettingsPage : ContentPage
{
    private readonly ProfileViewModel _viewModel;
    private readonly ThemeService _themeService;
    private const string PaymentMethodKey = "payment_method";

    public SettingsPage(ProfileViewModel viewModel, ThemeService themeService)
    {
        InitializeComponent();

        BindingContext = viewModel;
        _viewModel = viewModel;
        _themeService = themeService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadUser();

        int savedMethod = Preferences.Get(PaymentMethodKey, 0);

        PaymentMethodPicker.SelectedIndex = savedMethod;

        ThemePicker.SelectedIndex = 0;
    }

    private void OnPaymentMethodChanged(object sender, EventArgs e)
    {

        var picker = sender as Picker;

        if (picker == null)
            return;

        picker.BackgroundColor = Colors.DodgerBlue;

        Preferences.Set("payment_method", picker.SelectedIndex);
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


    private void OnThemeChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;

        if (picker == null)
            return;

        picker.BackgroundColor = Colors.DodgerBlue;

        switch (picker.SelectedIndex)
        {
            case 0:
                _themeService.SetTheme(AppTheme.Unspecified);
                break;

            case 1:
                _themeService.SetTheme(AppTheme.Light);
                break;

            case 2:
                _themeService.SetTheme(AppTheme.Dark);
                break;
        }
    }
}
