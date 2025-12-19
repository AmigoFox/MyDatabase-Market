using Microsoft.Maui.Controls;

namespace app;

public partial class DatabaseCalculator : ContentPage
{

    public DatabaseCalculator(app.ViewModels.DatabaseCalculatorViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is app.ViewModels.DatabaseCalculatorViewModel viewModel)
            await viewModel.InitializeAsync();
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