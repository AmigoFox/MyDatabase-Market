using System;
using CrossApp.Services;
using CrossApp.ViewModels;

namespace CrossApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCalculatorClicked(object? sender, EventArgs e)
        {
            var page = Application.Current.Handler.MauiContext.Services
                  .GetService<DatabaseCalculator>();
            await Navigation.PushAsync(page);
        }

    }
}