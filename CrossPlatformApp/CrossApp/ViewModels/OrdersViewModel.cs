using CrossApp.Models;
using CrossApp.Services.Api;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CrossApp.ViewModels
{
    public class OrdersViewModel
    {
        private readonly ApiClient _api;
        private bool _isLoading;
        private DateTime _lastLoad = DateTime.MinValue;

        public ObservableCollection<OrderDto> Orders { get; set; } = new();
        private readonly IServiceProvider _services;
        private readonly OrdersService _ordersService;

        public OrdersViewModel(ApiClient api, IServiceProvider services)
        {
            _api = api;
            _services = services;

        }


        public async Task LoadOrders()
        {
            // debounce: ignore repeated calls within short interval
            var now = DateTime.UtcNow;
            if (_isLoading)
            {
                Debug.WriteLine("LOAD ORDERS SKIPPED - already loading");
                return;
            }

            if ((now - _lastLoad).TotalMilliseconds < 800)
            {
                Debug.WriteLine("LOAD ORDERS SKIPPED - debounce");
                return;
            }

            _isLoading = true;
            try
            {
                Debug.WriteLine("LOAD ORDERS CALLED");

                var result = await _api.GetAsync<List<OrderDto>>("orders");

                Debug.WriteLine("RESULT COUNT: " + (result?.Count ?? 0));
                Debug.WriteLine("RESULT COUNT: " + (result));
                


                if (result == null)
                    return;

                Orders.Clear();

                foreach (var order in result)
                {
                    Orders.Add(order);
                }

                Console.WriteLine("ORDER OrdersViewModel:");
                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(Orders));
                _lastLoad = DateTime.UtcNow;
            }
            finally
            {
                _isLoading = false;
            }
        }

        public async Task LoadOrder(int id)
        {
            var item = await _api.GetAsync<OrderItemDto>($"OrderItems/{id}");

            if (item == null)
                return;
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            Debug.WriteLine($"Client: deleting order id={orderId}");
            var ok = await _ordersService.DeleteOrderAsync(orderId); // _ordersService: OrdersService injected
            if (ok) await LoadOrders();
            Debug.WriteLine($"Client: delete result={ok}");
            return ok;
        }

    }
}
