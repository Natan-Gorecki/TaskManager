using Microsoft.Extensions.Logging;
using Microsoft.Xaml.Behaviors;
using System.Windows.Input;
using System.Windows;
using TaskManager.Client.View.Backlog;
using TaskManager.Core.Models;
using TaskManager.Client.View.Modal;
using TaskManager.Client.Utils;

namespace TaskManager.Client.Behaviors;

internal class BacklogItemEditTaskBehavior : Behavior<BacklogItem>
{
    private readonly ILogger<BacklogItemEditTaskBehavior> _logger = App.IoC.GetRequiredService<ILogger<BacklogItemEditTaskBehavior>>();
    private readonly IModalPageManager _modalPageManager = App.IoC.GetRequiredService<IModalPageManager>();

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
        if (AssociatedObject.DataContext is not Task coreTask)
        {
            _logger.LogCritical($"BacklogItem data context is null or has different type than {typeof(Task)}");
            return;
        }

        _modalPageManager.EditTask(coreTask);
    }
}
