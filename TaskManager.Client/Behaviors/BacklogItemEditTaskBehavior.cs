using Microsoft.Extensions.Logging;
using Microsoft.Xaml.Behaviors;
using System.Windows.Input;
using System.Windows;
using TaskManager.Client.View.Backlog;
using TaskManager.Core.Models;

namespace TaskManager.Client.Behaviors;

internal class BacklogItemEditTaskBehavior : Behavior<BacklogItem>
{
    ILogger<BacklogItemEditTaskBehavior> _logger = App.IoC.GetRequiredService<ILogger<BacklogItemEditTaskBehavior>>();
    
    protected override void OnAttached()
    {
        AssociatedObject.MouseDoubleClick += BacklogItem_MouseDoubleClick;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.MouseDoubleClick -= BacklogItem_MouseDoubleClick;
    }

    private void BacklogItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (Application.Current.MainWindow is not MainWindow mainWindow)
        {
            _logger.LogCritical($"MainWindow is null or has different type than {typeof(MainWindow)}");
            return;
        }

        if (AssociatedObject.DataContext is not Task coreTask)
        {
            _logger.LogCritical($"KanbanTask data context is null or has different type than {typeof(Task)}");
            return;
        }

        mainWindow.modalPage.ModalPageContent.DataContext = coreTask;
        mainWindow.modalPage.Visibility = Visibility.Visible;
    }
}
