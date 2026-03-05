using System.ComponentModel;
using System.Runtime.CompilerServices;
using CrossApp.Models;
using CrossApp.Services.Api;
using CrossApp.Services;

namespace CrossApp.ViewModels;

public class OrderDetailsViewModel : INotifyPropertyChanged
{
    private readonly ApiClient _api;

    private OrderDto? _order;
    public OrderDto? Order
    {
        get => _order;
        set
        {
            _order = value;
            OnPropertyChanged();
        }
    }

    public OrderDetailsViewModel(ApiClient api)
    {
        _api = api;
    }

    public async Task LoadOrder(string id)
    {
        Order = await _api.GetAsync<OrderDto>($"orders/{id}");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}