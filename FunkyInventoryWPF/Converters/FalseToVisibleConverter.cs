﻿using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FunkyInventoryWPF.Converters;

public class FalseToVisibleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => bool.TryParse(value.ToString(), out bool isNotVisible) && isNotVisible ? Visibility.Collapsed : Visibility.Visible;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
