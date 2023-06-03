using System;
using System.Globalization;
using System.Windows.Data;

namespace TaskManager.Client.Converters;

public class BooleanToZIndexConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isBehind)
        {
            return isBehind ? -1 : 0;
        }

        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
