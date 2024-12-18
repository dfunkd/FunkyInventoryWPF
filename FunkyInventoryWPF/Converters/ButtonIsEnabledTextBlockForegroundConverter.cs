using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FunkyInventoryWPF.Converters;

public class ButtonIsEnabledTextBlockForegroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => bool.TryParse(value.ToString(), out bool isEnabled) && isEnabled == true
        ? (SolidColorBrush)new BrushConverter().ConvertFrom("#FF999999")
        : (SolidColorBrush)new BrushConverter().ConvertFrom("#FF0D0D0D");

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
