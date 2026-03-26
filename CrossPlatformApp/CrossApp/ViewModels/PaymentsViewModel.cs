using CrossApp.Models;
using CrossApp.Services.Api;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrossApp.Services;
using CrossApp.ViewModels;
using System.Diagnostics;


using System.Collections.ObjectModel;
using System.Windows.Input;


namespace CrossApp.ViewModels
{
    public partial class PaymentsViewModel : BaseViewModel
    {
        private readonly PaymentsService _service;

        [ObservableProperty]
        private ValidationState _validationState = ValidationState.None;

        [ObservableProperty]
        private string _validationMessage;

        public ObservableCollection<PaymentDto> Payments { get; set; } = new();

        public ICommand LoadCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand PayCommand { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public PaymentsViewModel(PaymentsService service)
        {
            _service = service;

            PayCommand = new Command<PaymentDto>(async (payment) => await Pay(payment));
        }

        public ICommand OpenOrderCommand => new Command<int>(async (orderId) =>
        {
            await Shell.Current.GoToAsync($"{nameof(OrderDetailsPage)}?id={orderId}");
        });

        public async Task LoadPayments()
        {
            if (IsLoading) return;

            try
            {
                IsLoading = true;
                ValidationState = ValidationState.None;
                ValidationMessage = "Загрузка платежей...";

                var items = await _service.GetPayments();

                Payments.Clear();

                foreach (var item in items)
                    Payments.Add(item);

                ValidationState = ValidationState.None;
                ValidationMessage = "Чек отправлен на почту и доступен в PDF";
            }
            catch (Exception ex)
            {

                ValidationState = ValidationState.Error;
                ValidationMessage = "Ошибка загрузки платежей";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task Pay(PaymentDto payment)
        {
            if (payment == null)
                return;

            try
            {
                var request = new CreatePaymentRequest
                {
                    OrderId = payment.OrderId,
                    Amount = payment.Amount,
                    PaymentMethod = "card" // пока хардкод
                };

                await _service.CreatePayment(request);

                ValidationState = ValidationState.None;
                ValidationMessage = "Платеж выполнен";



            }
            catch (Exception ex)
            {
                ValidationState = ValidationState.Error;
                ValidationMessage = "Ошибка оплаты";
            }
        }
    }
}