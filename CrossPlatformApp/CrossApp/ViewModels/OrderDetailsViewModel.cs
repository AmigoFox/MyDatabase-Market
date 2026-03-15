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

    public string CountriesText =>
        Order == null ? "" : string.Join(", ", Order.Countries);

    public OrderDetailsViewModel(ApiClient api)
    {
        _api = api;
    }

    public async Task LoadOrder(int id)
    {
        Debug.WriteLine($"LoadOrder called with id={id}");

        var order = await _api.GetAsync<OrderDto>($"orders/{id}");

        if (order == null)
            return;

        var item = order.Items?.FirstOrDefault();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Order = item;
        });
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}