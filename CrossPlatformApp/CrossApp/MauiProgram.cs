using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using CrossApp.Services;
using CrossApp.Services.Api;
using CrossApp.ViewModels;

namespace CrossApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<ExchangeRateCache>();
            builder.Services.AddHttpClient<CbrExchangeRateService>();
            builder.Services.AddTransient<ViewModels.DatabaseCalculatorViewModel>();
            builder.Services.AddTransient<DatabaseCalculator>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<OrdersPage>();
            builder.Services.AddTransient<PaymentsPage>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<OrdersViewModel>();
            builder.Services.AddSingleton<AuthTokenStore>();
            builder.Services.AddTransient<AuthHandler>();
            builder.Services.AddTransient<OrderDetailsViewModel>();
            builder.Services.AddTransient<OrderDetailsPage>();
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddSingleton<ThemeService>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddSingleton<AppShell>();


            builder.Services
            .AddHttpClient<ApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7166/api/v1/");
            })
            .AddHttpMessageHandler<AuthHandler>();




            builder.Services.AddHttpClient<OrdersService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7166/api/v1/");
            })
            .AddHttpMessageHandler<AuthHandler>();



            builder.Services.AddHttpClient<UserService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7166/api/v1/");
            })
            .AddHttpMessageHandler<AuthHandler>();


            builder.Services.AddHttpClient<PaymentsService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7166/api/v1/");
            })
            .AddHttpMessageHandler<AuthHandler>();



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
