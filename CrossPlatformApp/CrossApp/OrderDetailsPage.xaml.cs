using CrossApp.Models;
using CrossApp.Services.Api;
using CrossApp.ViewModels;
using System.Diagnostics;

namespace CrossApp;

// Removed [QueryProperty] to avoid duplicate parameter wiring; use IQueryAttributable only
public partial class OrderDetailsPage : ContentPage, IQueryAttributable
{
    private readonly OrderDetailsViewModel _vm;
    private int? _loadedId;
    private readonly PaymentsService _paymentsService;

    // Keep property for compatibility but do not trigger Load from its setter
    public int OrderId { get; set; }

    public OrderDetailsPage(OrderDetailsViewModel vm, PaymentsService paymentsService)
    {
        InitializeComponent();

        Debug.WriteLine("OrderDetailsPage.ctor");
        _vm = vm;
        BindingContext = _vm;
        _paymentsService = paymentsService;
        Debug.WriteLine("OrderDetailsPage: BindingContext set to vm");
    }

    private async Task Load(int id)
    {
        // guard against duplicate calls
        if (_loadedId.HasValue && _loadedId.Value == id)
            return;

        _loadedId = id;
        await _vm.LoadOrder(id);
    }

    // IDictionary<string, object> implementation receives query params from Shell
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Debug.WriteLine("ApplyQueryAttributes called: " + (query == null ? "null" : string.Join(",", query.Keys)));

        if (query == null)
            return;

        if (!query.TryGetValue("id", out var raw))
            return;

        int id;

        switch (raw)
        {
            case int i:
                id = i;
                break;
            case long l:
                id = (int)l;
                break;
            case string s when int.TryParse(s, out var parsed):
                id = parsed;
                break;
            default:
                return;
        }

        Debug.WriteLine($"ApplyQueryAttributes parsed id={id}");
        _ = Load(id);
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (_vm.Order == null)
            return;

        var orderItemId = _vm.Order.Id;
        await Shell.Current.GoToAsync($"{nameof(DatabaseCalculator)}?id={orderItemId}");
    }



    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var order = (sender as Button)?.BindingContext as OrderDto;
        if (order == null) return;
        if (!await DisplayAlert("Подтвердите", $"Удалить заказ #{order.Id}?", "Да", "Нет")) return;
        await _vm.DeleteOrder(order.Id);

    }


    private async void OnPayClicked(object sender, EventArgs e)
    {
        if (_vm.Order == null)
            return;

        Console.WriteLine($"PAY CLICK ORDER ID: {_vm.Order?.Id}");

        try
        {
            var request = new CreatePaymentRequest
            {
                OrderId = _vm.Order.Id,
                PaymentMethod = _vm.SelectedPaymentMethod ?? "Card"
            };

            await _vm.CreatePayment(request);

            await DisplayAlert("Успех", "Оплата прошла успешно", "OK");

            await Shell.Current.GoToAsync(nameof(PaymentsPage));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", ex.Message, "OK");
        }
    }


}