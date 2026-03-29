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
        private readonly IServiceProvider _services;
        public event Action? LoginSucceeded;
        public string LoginApp { get; set; }
        public string Password { get; set; }

        public LoginViewModel(ApiClient api, AuthTokenStore tokenStore, IServiceProvider services)
        {
            _api = api;
            _tokenStore = tokenStore;
            _services = services;
        }

        public Command LoginCommand => new Command(async () =>
        {
            await Login(LoginApp, Password);
        });

        public async Task Login(string login, string password)
        {
            var request = new LoginRequest
            {
                Login = login,
                Password = password
            };

            var result = await _api.PostAsync<LoginRequest, LoginResponse>("auth/login", request);

            if (result != null)
            {
                _tokenStore.SetToken(result.Token);
                Preferences.Set("auth_token", result.Token);

                // 🔥 просто сигнал
                LoginSucceeded?.Invoke();
            }
        }

    }
}
