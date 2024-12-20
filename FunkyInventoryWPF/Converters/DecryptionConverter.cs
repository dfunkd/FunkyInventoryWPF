using System.Globalization;
using System.Windows.Data;

namespace FunkyInventoryWPF.Converters;

public class DecryptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is not null ?  value.ToString().Decrypt() : string.Empty;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
