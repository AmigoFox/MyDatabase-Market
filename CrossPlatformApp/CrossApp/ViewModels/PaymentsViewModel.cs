using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossApp.Models;
using CrossApp.Services.Api;

namespace CrossApp.ViewModels
{
    public class PaymentsViewModel : BaseViewModel
    {
        private readonly PaymentsService _service;

        public ObservableCollection<PaymentDto> Payments { get; set; } = new();

        public PaymentsViewModel(PaymentsService service)
        {
            _service = service;
        }

        public async Task Load()
        {
            var items = await _service.GetPayments();

            Payments.Clear();
            foreach (var item in items)
                Payments.Add(item);
        }
    }
}
