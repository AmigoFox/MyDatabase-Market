using CrossApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CrossApp.Services.Api
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly AuthTokenStore _tokenStore;

        public UserService(HttpClient httpClient, AuthTokenStore tokenStore)
        {
            _httpClient = httpClient;
            _tokenStore = tokenStore;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<UserMeResponse?> GetMeAsync()
        {
            return await _httpClient.GetFromJsonAsync<UserMeResponse>("auth/me");
        }
    }
}
