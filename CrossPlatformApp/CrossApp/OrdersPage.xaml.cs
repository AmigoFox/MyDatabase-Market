using CrossApp.ViewModels;
using CrossApp.Models;
using CrossApp.Services.Api;
using CrossApp.Converters;


using System.Diagnostics;


namespace CrossApp;
public partial class OrdersPage : ContentPage

{
    private readonly OrdersViewModel _vm;
    private readonly OrdersService _ordersService;

    public OrdersPage(OrdersViewModel vm, OrdersService ordersService)
    {
        InitializeComponent();
        _vm = vm;
        _ordersService = ordersService;
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
        await Shell.Current.GoToAsync($"//{nameof(PaymentsPage)}");
    }


    private async void OnMoreDetailsClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var order = button?.BindingContext as OrderDto;

        if (order == null)
            return;

        await Shell.Current.GoToAsync($"{nameof(OrderDetailsPage)}?id={order.Id}");
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var order = button?.BindingContext as OrderDto;
        if (order == null) return;

        var confirmed = await DisplayAlert("Подтвердите", $"Удалить заказ #{order.Id}?", "Да", "Нет");
        if (!confirmed) return;

        Debug.WriteLine($"Attempting delete order id={order.Id}");

        try
        {
            var ok = await _ordersService.DeleteOrderAsync(order.Id);
            Debug.WriteLine($"Delete response ok={ok}");
            if (ok)
            {
                await _vm.LoadOrders();
            }
            else
            {
                await DisplayAlert("Ошибка", "Не удалось удалить заказ", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Delete failed: {ex}");
            await DisplayAlert("Ошибка", "Не удалось удалить заказ: " + ex.Message, "OK");
        }
    }
}

