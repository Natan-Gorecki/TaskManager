using System.Globalization;
using System.Windows.Data;
using System;
using Microsoft.Extensions.Logging;
using TaskManager.Core.Models;

namespace TaskManager.Client.Converters;

public class TaskStatusToStringConverter : IValueConverter
{
    ILogger<TaskStatusToStringConverter> _logger = App.IoC.GetRequiredService<ILogger<TaskStatusToStringConverter>>();
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not ETaskStatus taskStatus)
        {
            _logger.LogError($"Value is not {nameof(ETaskStatus)} - {value}.");
            return string.Empty;
        }

        return taskStatus switch
        {
            ETaskStatus.Waiting => Resources.Resources.ETaskStatus_Waiting,
            ETaskStatus.Blocked => Resources.Resources.ETaskStatus_Blocked,
            ETaskStatus.InProgress => Resources.Resources.ETaskStatus_InProgress,
            ETaskStatus.Completed => Resources.Resources.ETaskStatus_Completed,
            ETaskStatus.Uncompleted => Resources.Resources.ETaskStatus_Uncompleted,
            _ => () =>
            {
                _logger.LogWarning($"Unknown value of {nameof(ETaskStatus)} - {taskStatus}.");
                return string.Empty;
            }
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        _logger.LogCritical("ConvertBack is not implemented!");
        throw new NotSupportedException();
    }
}