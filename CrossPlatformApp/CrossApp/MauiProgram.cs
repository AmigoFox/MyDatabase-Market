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
            builder.Services.AddSingleton<ApiService>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<OrdersPage>();
            builder.Services.AddTransient<PaymentsPage>();
            builder.Services.AddTransient<Personal_account>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<OrdersViewModel>();





#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
