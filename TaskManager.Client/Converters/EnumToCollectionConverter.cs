using Microsoft.Extensions.Logging;
using System.Globalization;
using System;
using System.Windows.Data;
using TaskManager.Core.Models;

namespace TaskManager.Client.Converters;

public class EnumToCollectionConverter : IValueConverter
{
    ILogger<EnumToCollectionConverter> _logger = App.IoC.GetRequiredService<ILogger<EnumToCollectionConverter>>();

    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Enum)
        {
            _logger.LogCritical($"Value is null or not enum - {value}.");
            return null;
        }

        return Enum.GetValues(value.GetType());
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        _logger.LogCritical("ConvertBack is not implemented!");
        throw new NotSupportedException();
    }
}
