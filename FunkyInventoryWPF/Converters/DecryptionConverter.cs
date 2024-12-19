using System.Globalization;
using System.Windows.Data;

namespace FunkyInventoryWPF.Converters;

public class DecryptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value.ToString().Decrypt();

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
