using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossApp.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;



namespace CrossApp.Converters;
public class ValidationBorderColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            ValidationState.Error => Colors.LightPink,
            ValidationState.Info => Colors.LightYellow,
            ValidationState.None => Colors.Transparent,
            _ => Colors.Transparent
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}