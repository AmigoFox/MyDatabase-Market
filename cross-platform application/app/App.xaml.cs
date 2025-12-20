using app.Services;

namespace app
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }

        public App(IServiceProvider services)
        {
            InitializeComponent();
            Services = services;

            var cache = services.GetService<ExchangeRateCache>();
            var api = services.GetService<CbrExchangeRateService>();

            // Загружаем курсы в фоне
            Task.Run(async () =>
            {
                try
                {
                    cache.Rates = await api.GetRatesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка загрузки курсов: " + ex.Message);
                }
            });
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
