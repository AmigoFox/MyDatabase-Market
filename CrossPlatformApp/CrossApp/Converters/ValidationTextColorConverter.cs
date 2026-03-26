using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossApp.ViewModels;

namespace CrossApp.Converters;
public class ValidationTextColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            ValidationState.Error => Colors.DarkGrey,
            ValidationState.Info => Colors.DarkGrey,
            ValidationState.None => Colors.DarkGrey,
            _ => Colors.DarkGrey

        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}