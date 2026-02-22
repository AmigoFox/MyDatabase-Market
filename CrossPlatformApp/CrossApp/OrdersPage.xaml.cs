using CrossApp.ViewModels;
using CrossApp.Services.Api;

namespace CrossApp;
public partial class OrdersPage : ContentPage
{
    private readonly OrdersViewModel _vm;

    public OrdersPage(OrdersViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadOrders();
    }

    private async void OnProfileDetailsTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ProfilePage));
    }

    private async void OnPaymentsTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PaymentsPage));
    }
}
