using CrossApp.Models;
using CrossApp.Services;
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
        private readonly ApiClient _api;
        private readonly AuthTokenStore _tokenStore;

        public LoginViewModel(ApiClient api, AuthTokenStore tokenStore)
        {
            _api = api;
            _tokenStore = tokenStore;
        }

        public async Task Login(string login, string password)
        {
            Console.WriteLine("LOIN BUTTON CLICKED");
            var request = new LoginRequest
            {
                Login = login,
                Password = password
            };
            var result = await _api.PostAsync<LoginRequest, LoginResponse>("auth/login", request);

            if (result != null)
            {
                _tokenStore.SetToken(result.Token);

                await Shell.Current.GoToAsync("//MainPage");
            }
        }
    }
}
