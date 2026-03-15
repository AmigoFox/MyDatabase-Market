using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using CrossApp.Services.Api;

public class ProfileViewModel : INotifyPropertyChanged
{
    private readonly UserService _userService;

    public ProfileViewModel(UserService userService)
    {
        _userService = userService;
    }

    private string _login = "";
    public string Login
    {
        get => _login;
        set { _login = value; OnPropertyChanged(); }
    }

    private string _email = "";
    public string Email
    {
        get => _email;
        set { _email = value; OnPropertyChanged(); }
    }

    private string _fullName = "";
    public string FullName
    {
        get => _fullName;
        set { _fullName = value; OnPropertyChanged(); }
    }

    public async Task LoadUser()
    {
        var user = await _userService.GetMeAsync();

        if (user == null)
            return;

        Login = user.Login;
        Email = user.Email;
        FullName = user.FullName;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    void OnPropertyChanged([CallerMemberName] string name = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
