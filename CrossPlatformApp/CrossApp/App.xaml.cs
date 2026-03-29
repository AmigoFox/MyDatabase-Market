using CrossApp.Services;

namespace CrossApp
{
        public partial class App : Application
        {
            public App(ThemeService themeService, IServiceProvider services)
            {
                InitializeComponent();

                themeService.ApplySavedTheme();

                var token = Preferences.Get("auth_token", null);

                if (string.IsNullOrEmpty(token))
                {
                    var loginPage = services.GetRequiredService<LoginPage>();
                    MainPage = new NavigationPage(loginPage);
                }
                else
                {
                    MainPage = services.GetRequiredService<AppShell>();
                }
            }
        }

}