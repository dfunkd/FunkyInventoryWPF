using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FunkyInventoryWPF.Converters;

public class TrueToVisibleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => bool.TryParse(value.ToString(), out bool boolValue) && boolValue ? Visibility.Visible : Visibility.Collapsed;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
