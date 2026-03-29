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

    private OrderDto? _orderFull;
    public OrderDto? OrderFull
    {
        get => _orderFull;
        set
        {
            _orderFull = value;
            OnPropertyChanged(nameof(OrderName));
        }
    }

    public string OrderName
    {
        get => OrderFull?.OrderName ?? "";
        set
        {
            if (OrderFull != null)
            {
                OrderFull.OrderName = value;
                OnPropertyChanged(nameof(OrderName));
            }
        }
    }


    private string _selectedPaymentMethod = "Card";

    public string SelectedPaymentMethod
    {
        get => _selectedPaymentMethod;
        set
        {
            _selectedPaymentMethod = value;
            OnPropertyChanged(nameof(OrderName));
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
        OnPropertyChanged(nameof(OrderName));

        MainThread.BeginInvokeOnMainThread(() =>
        {
            OrderFull = order;
            Order = order.Items?.FirstOrDefault();
            OnPropertyChanged(nameof(OrderName));
        });
    }

    public async Task DeleteOrder(int id)
    {
        var success = await _ordersService.DeleteOrderAsync(id);
        

        if (success)
        {
            await Shell.Current.GoToAsync("//OrdersPage");
        }
        OnPropertyChanged(nameof(OrderName));
    }

    public async Task PutOrder(int id, UpdateOrderItemRequest request)
    {
        var success = await _ordersService.UpdateAsync(id, request);
        
        if (success)
        {
            await Shell.Current.GoToAsync("//OrdersPage");
        }
        OnPropertyChanged(nameof(OrderName));
    }

    public async Task SaveOrderNameAsync()
    {
        if (OrderId <= 0)
            return;

        var success = await _ordersService.UpdateOrderNameAsync(OrderId, OrderName);

        if (success)
        {
            await Application.Current.MainPage.DisplayAlert("Успех", "Имя обновлено", "OK");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось обновить имя", "OK");
        }

        OnPropertyChanged(nameof(OrderName));
    }


    protected void OnPropertyChanged([CallerMemberName] string name = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public async Task CreatePayment(CreatePaymentRequest request)
    {
        await _paymentsService.CreatePayment(request);
        OnPropertyChanged(nameof(OrderName));
    }


}