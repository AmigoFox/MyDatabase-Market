using CrossApp.Models;
using CrossApp.Services.Api;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CrossApp.ViewModels;

public class OrdersViewModel : BaseViewModel
{
    private readonly ApiClient _api;
    private readonly OrdersService _ordersService;

    private bool _isLoading;
    private DateTime _lastLoad = DateTime.MinValue;

    public ObservableCollection<OrderDto> Orders { get; set; } = new();

    public OrdersViewModel(ApiClient api, OrdersService ordersService)
    {
        _api = api;
        _ordersService = ordersService;
    }

    private ValidationState _validationState;
    public ValidationState ValidationState
    {
        get => _validationState;
        set => SetProperty(ref _validationState, value);
    }

    private string _validationMessage;
    public string ValidationMessage
    {
        get => _validationMessage;
        set => SetProperty(ref _validationMessage, value);
    }

    public async Task LoadOrders()
    {
        if (_isLoading) return;

        _isLoading = true;

        try
        {
            ValidationState = ValidationState.Info;
            ValidationMessage = "Загрузка заказов...";

            var result = await _api.GetAsync<List<OrderDto>>("orders");

            Orders.Clear();

            if (result != null)
            {
                foreach (var order in result)
                    Orders.Add(order);
            }

            ValidationState = ValidationState.None;
            ValidationMessage = "";
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            ValidationState = ValidationState.Error;
            ValidationMessage = "Ошибка загрузки заказов";
        }
        finally
        {
            _isLoading = false;
            _lastLoad = DateTime.UtcNow;
        }
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        var ok = await _ordersService.DeleteOrderAsync(orderId);

        if (ok)
            await LoadOrders();

        return ok;
    }
}