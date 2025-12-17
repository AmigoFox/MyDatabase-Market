using Microsoft.Maui.Controls;

namespace app;

public partial class DatabaseCalculator : ContentPage
{
    public DatabaseCalculator()
    {
        InitializeComponent();
    }

    private async void OnPickerChanged(object sender, EventArgs e)
    {

        if (BindingContext is ViewModels.DatabaseCalculatorViewModel vm)
        {
            byte count = (byte)vm.SelectedCountriesCount;
            vm.OnSelectionChanged();
            if (count > 3)
            {
               await DisplayAlert("Уведомление", "Чтобы выбрать больше 3-х стран обратитесь к менеджеру", "OK");
            }
        }

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
}