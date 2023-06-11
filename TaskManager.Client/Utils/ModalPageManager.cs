using Microsoft.Extensions.Logging;
using System;
using System.Windows;
using TaskManager.Client.View.Modal;

namespace TaskManager.Client.Utils;

internal class ModalPageManager : IModalPageManager
{
    private readonly ILogger<ModalPageManager> _logger = App.IoC.GetRequiredService<ILogger<ModalPageManager>>();

    public void EditTask(Task task)
    {
        ArgumentNullException.ThrowIfNull(task);

        if (Application.Current.MainWindow is not MainWindow mainWindow)
        {
            _logger.LogCritical($"MainWindow is null or has different type than {typeof(MainWindow)}");
            return;
        }

        if (mainWindow.modalPage.ModalPageContent is not EditTaskControl editTaskControl)
        {
            _logger.LogCritical($"ModalPage content is null or has different type than {typeof(EditTaskControl)}");
            return;
        }

        editTaskControl.EditedTask = task;
        mainWindow.modalPage.Visibility = Visibility.Visible;
    }

    public void Close()
    {
        if (Application.Current.MainWindow is not MainWindow mainWindow)
        {
            _logger.LogCritical($"MainWindow is null or has different type than {typeof(MainWindow)}");
            return;
        }

        mainWindow.modalPage.Visibility = Visibility.Collapsed;
    }
}
