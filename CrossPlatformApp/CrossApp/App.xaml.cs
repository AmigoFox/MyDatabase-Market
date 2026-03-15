using CrossApp.Services;

namespace CrossApp
{
    public partial class App : Application
    {
        public App(ThemeService themeService)
        {
            InitializeComponent();

            themeService.ApplySavedTheme();

            MainPage = new AppShell();
        }
    }
}