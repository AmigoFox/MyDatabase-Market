using CrossApp.Models;
using CrossApp.Services.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossApp.ViewModels
{
    public class LoginViewModel
    {
        private readonly ApiService _api;

        public LoginViewModel(ApiService api)
        {
            _api = api;
        }

        public async Task Login(string login, string password)
        {
            Console.WriteLine("LOGIN BUTTON CLICKED");

            var response = await _api.LoginAsync(login, password);

            if (response == null)
                return;

            _api.SetJwt(response.Token);

            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}
