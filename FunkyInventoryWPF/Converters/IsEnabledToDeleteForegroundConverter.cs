using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FunkyInventoryWPF.Converters;

public class IsEnabledToDeleteForegroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => bool.TryParse(value.ToString(), out bool isEnabled) && isEnabled == true
        ? (SolidColorBrush)new BrushConverter().ConvertFrom("#FF990000")
        : (SolidColorBrush)new BrushConverter().ConvertFrom("#FF0D0000");

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
