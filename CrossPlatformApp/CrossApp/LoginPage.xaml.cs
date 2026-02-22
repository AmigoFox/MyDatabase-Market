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
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await _vm.Login(LoginEntry.Text, PasswordEntry.Text);
    }
}