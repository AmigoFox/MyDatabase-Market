using Microsoft.Maui.Controls;

namespace CrossApp;

public partial class DatabaseCalculator : ContentPage
{

    public DatabaseCalculator(CrossApp.ViewModels.DatabaseCalculatorViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void OnPickerChanged(object sender, EventArgs e)
    {

        if (BindingContext is ViewModels.DatabaseCalculatorViewModel vm)
            vm.OnSelectionChanged();

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