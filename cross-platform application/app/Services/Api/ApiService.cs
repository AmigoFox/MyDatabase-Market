using app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;

namespace app.Services.Api
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;


        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7166/api/v1/")
            };
        }

        public void SetJwt(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
                return default;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(
            string endpoint,
            TRequest data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);

            var responseJson = await response.Content.ReadAsStringAsync();

            Debug.WriteLine("STATUS: " + response.StatusCode);
            Debug.WriteLine("RAW RESPONSE: " + responseJson);


            if (!response.IsSuccessStatusCode)
                return default;

            return JsonSerializer.Deserialize<TResponse>(responseJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }


        public async Task<LoginResponse?> LoginAsync(string login, string password)
        {
            return await PostAsync<LoginRequest, LoginResponse>(
                "auth/login",
                new LoginRequest
                {
                    Login = login,
                    Password = password
                });
        }
    }
}
