namespace app;

public partial class Personal_account : ContentPage
{
    public Personal_account()
    {
        InitializeComponent();
    }

    private void OnProfileTapped(object sender, TappedEventArgs e)
    {
        // TODO: переход на страницу профиля
        DisplayAlert("Профиль", "Откроем страницу профиля.", "Ок");
    }

    private void OnOrdersTapped(object sender, TappedEventArgs e)
    {
        // TODO: переход на страницу заказов
        DisplayAlert("Заказы", "Откроем список заказов.", "Ок");
    }

    private void OnPaymentsTapped(object sender, TappedEventArgs e)
    {
        // TODO: переход на страницу платежей
        DisplayAlert("Платежи", "Откроем историю платежей.", "Ок");
    }
}
