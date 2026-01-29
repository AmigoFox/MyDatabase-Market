using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;



namespace app.Converters;
public class ValidationBorderColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            ValidationState.Error => Colors.Red,
            ValidationState.Info => Colors.Orange,
            _ => Colors.Transparent
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}