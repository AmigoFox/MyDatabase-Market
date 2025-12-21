namespace app;

public partial class OrdersPage : ContentPage
{
    public OrdersPage()
    {
        InitializeComponent();

        // Заглушка — позже заменим на реальные данные
        OrdersList.ItemsSource = new List<OrderModel>
        {
            new OrderModel { Id = 1, Name = "MySQL 10GB", Price = "1200 ₽", Status = "Активен" },
            new OrderModel { Id = 2, Name = "PostgreSQL 20GB", Price = "2400 ₽", Status = "Ожидает оплаты" }
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

public class OrderModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Price { get; set; }
    public string Status { get; set; }
}
