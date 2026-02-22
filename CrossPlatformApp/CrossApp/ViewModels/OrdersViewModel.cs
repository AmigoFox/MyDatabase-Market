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
        private readonly ApiService _api;

        public ObservableCollection<OrderDto> Orders { get; set; } = new();

        public OrdersViewModel(ApiService api)
        {
            _api = api;
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

    }
}
