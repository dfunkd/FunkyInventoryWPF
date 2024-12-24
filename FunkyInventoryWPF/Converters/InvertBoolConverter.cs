using System.Globalization;
using System.Windows.Data;

namespace FunkyInventoryWPF.Converters;

public class InvertBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => bool.TryParse(value.ToString(), out var boolValue) ? !boolValue : false;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
