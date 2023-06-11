﻿using Microsoft.Extensions.Logging;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;
using TaskManager.Client.Utils;
using TaskManager.Client.View.Kanban;
using TaskManager.Client.View.Modal;
using TaskManager.Core.Models;

namespace TaskManager.Client.Behaviors;

internal class KanbanTaskEditTaskBehavior : Behavior<KanbanTask>
{
    ILogger<BacklogItemEditTaskBehavior> _logger = App.IoC.GetRequiredService<ILogger<BacklogItemEditTaskBehavior>>();
    IModalPageManager _modalPageManager = App.IoC.GetRequiredService<IModalPageManager>();

    protected override void OnAttached()
    {
        AssociatedObject.MouseDoubleClick += KanbanTask_MouseDoubleClick;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.MouseDoubleClick -= KanbanTask_MouseDoubleClick;
    }

    private void KanbanTask_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (AssociatedObject.DataContext is not Task coreTask)
        {
            _logger.LogCritical($"KanbanTask data context is null or has different type than {typeof(Task)}");
            return;
        }

        _modalPageManager.EditTask(coreTask);
    }
}
