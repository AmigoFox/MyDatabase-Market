using System;
using CrossApp.ViewModels;
using Microsoft.Maui.Controls;
using CrossApp.Services;
using CrossApp.Services.Api;

namespace CrossApp;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel _vm;

    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;

        _vm.LoginSucceeded += OnLoginSucceeded;
    }

    private void OnLoginSucceeded()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.GoToAsync("//MainPage");
        });
    }
}