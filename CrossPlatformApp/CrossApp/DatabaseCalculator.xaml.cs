using CrossApp.ViewModels;
using CrossApp.Models;
using CrossApp.Services.Api;
using System.Diagnostics;

using Microsoft.Maui.Controls;

namespace CrossApp;

public partial class DatabaseCalculator : ContentPage, IQueryAttributable
{
    private readonly DatabaseCalculatorViewModel _vm;
    private readonly OrdersService _ordersService;
    public DatabaseCalculator(DatabaseCalculatorViewModel vm, OrdersService ordersService)
    {   
        InitializeComponent();
        _vm = vm;
        _ordersService = ordersService;
        BindingContext = _vm;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("orderId", out var idObj))
        {
            if (int.TryParse(idObj.ToString(), out var id))
            {
                _vm.OrderId = id;
            }
        }
    }

    private void OnPickerChanged(object sender, EventArgs e)
    {

        if (BindingContext is ViewModels.DatabaseCalculatorViewModel vm)
            vm.OnSelectionChanged();

    }

    private void OnEntryChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is ViewModels.DatabaseCalculatorViewModel vm)
            vm.OnSelectionChanged();
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        if (BindingContext is ViewModels.DatabaseCalculatorViewModel vm)
        {
            await DisplayAlert("Успех", "Конфигурация сохранена!", "OK");
        }
    }

    private async void OnSaveOrderNameClicked(object sender, EventArgs e)
    {
        if (_vm != null)
            await _vm.SaveOrderNameAsync();
    }

}