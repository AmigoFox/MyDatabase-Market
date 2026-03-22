using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Maui.ApplicationModel;
using CrossApp.Models;
using CrossApp.Services.Api;

namespace CrossApp.ViewModels;

public class OrderDetailsViewModel : INotifyPropertyChanged
{
    private readonly ApiClient _api;

    private OrderItemDto? _order;
    private readonly OrdersService _ordersService;
    public int OrderId { get; private set; }
    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly PaymentsService _paymentsService;

    public List<string> PaymentMethods { get; } = new()
        {
            "Card",
            "Crypto",
            "Bank"
        };

    private string _selectedPaymentMethod = "Card";

    public string SelectedPaymentMethod
    {
        get => _selectedPaymentMethod;
        set
        {
            _selectedPaymentMethod = value;
            OnPropertyChanged();
        }
    }

    public OrderItemDto? Order
    {
        get => _order;
        set
        {
            _order = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CountriesText));
        }
    }

    public OrderDetailsViewModel(ApiClient api, OrdersService ordersService, PaymentsService paymentsService)
    {
        _api = api;
        _ordersService = ordersService;
        _paymentsService = paymentsService;
    }

    public string CountriesText =>
        Order == null ? "" : string.Join(", ", Order.Countries);



    public async Task LoadOrder(int id)
    {
        OrderId = id;

        var order = await _api.GetAsync<OrderDto>($"orders/{id}");

        if (order == null)
            return;

        var item = order.Items?.FirstOrDefault();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Order = item;
        });
    }

    public async Task DeleteOrder(int id)
    {
        var success = await _ordersService.DeleteOrderAsync(id);

        if (success)
        {
            await Shell.Current.GoToAsync("//OrdersPage");
        }
    }

    public async Task PutOrder(int id, UpdateOrderItemRequest request)
    {
        var success = await _ordersService.UpdateAsync(id, request);

        if (success)
        {
            await Shell.Current.GoToAsync("//OrdersPage");
        }
    }


    protected void OnPropertyChanged([CallerMemberName] string name = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public async Task CreatePayment(CreatePaymentRequest request)
    {
        await _paymentsService.CreatePayment(request);
    }
}