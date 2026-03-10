using CrossApp.ViewModels;
using CrossApp.Models;

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


    private async void OnMoreDetailsClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var order = button?.BindingContext as OrderDto;

        if (order == null)
            return;

        var parameters = new Dictionary<string, object>
        {
            ["id"] = order.Id
        };

        //await Shell.Current.GoToAsync(nameof(OrderDetailsPage), parameters);
        await Shell.Current.GoToAsync($"{nameof(OrderDetailsPage)}?id={order.Id}");
    }
}

