namespace app;

public partial class PaymentsPage : ContentPage
{
    public PaymentsPage()
    {
        InitializeComponent();

        PaymentsList.ItemsSource = new List<PaymentModel>
        {
            new PaymentModel { Id = 1, OrderName = "MySQL 10GB", Amount = "1200 ₽", Status = "Оплачено" },
            new PaymentModel { Id = 2, OrderName = "PostgreSQL 20GB", Amount = "2400 ₽", Status = "Ожидает оплаты" }
        };
    }

    private async void OnProfileDetailsTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage());
    }

    private async void OnOrdersTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new OrdersPage());
    }

    private async void OnPaymentsTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new PaymentsPage());
    }
}

public class PaymentModel
{
    public int Id { get; set; }
    public string OrderName { get; set; }
    public string Amount { get; set; }
    public string Status { get; set; }
}
