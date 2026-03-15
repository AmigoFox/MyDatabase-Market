using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossApp.Services;

public class ThemeService
{
    private const string ThemeKey = "app_theme";

    public AppTheme GetSavedTheme()
    {
        var value = Preferences.Get(ThemeKey, "system");

        return value switch
        {
            "light" => AppTheme.Light,
            "dark" => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };
    }

    public void ApplySavedTheme()
    {
        var theme = GetSavedTheme();
        Application.Current!.UserAppTheme = theme;
    }

    public void SetTheme(AppTheme theme)
    {
        Application.Current!.UserAppTheme = theme;

        var value = theme switch
        {
            AppTheme.Light => "light",
            AppTheme.Dark => "dark",
            _ => "system"
        };

        Preferences.Set(ThemeKey, value);
    }
}