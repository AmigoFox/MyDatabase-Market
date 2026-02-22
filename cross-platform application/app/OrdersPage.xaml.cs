using app.ViewModels;
using app.Services.Api;

namespace app;

public partial class OrdersPage : ContentPage
{
    private readonly OrdersViewModel _vm;

    public OrdersPage()
    {
        InitializeComponent();

        var api = App.Services.GetService<ApiService>();
        _vm = new OrdersViewModel(api);
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadOrders();
    }

    private async void OnProfileDetailsTapped(object sender, TappedEventArgs e)
    {
        var page = App.Services.GetService<ProfilePage>();
        await Navigation.PushAsync(page);
    }

    private async void OnPaymentsTapped(object sender, TappedEventArgs e)
    {
        var page = App.Services.GetService<PaymentsPage>();
        await Navigation.PushAsync(page);
    }
}
