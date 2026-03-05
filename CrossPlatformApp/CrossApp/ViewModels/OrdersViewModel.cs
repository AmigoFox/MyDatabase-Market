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
        public Command<OrderDto> OpenOrderCommand { get; }

        public ObservableCollection<OrderDto> Orders { get; set; } = new();

        public OrdersViewModel(ApiClient api)
        {
            _api = api;
            OpenOrderCommand = new Command<OrderDto>(OpenOrder);
        }

        public async Task LoadOrders()
        {
            Debug.WriteLine("LOAD ORDERS CALLED");

            var result = await _api.GetAsync<List<OrderDto>>("orders");

            Debug.WriteLine("RESULT COUNT: " + (result?.Count ?? 0));

            if (result == null)
                return;

            Orders.Clear();

            foreach (var order in result)
            {
                Orders.Add(order);
            }
        }

        private async void OpenOrder(OrderDto order)
        {
            if (order == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(OrderDetailsPage)}?id={order.Id}");
        }

    }
}
