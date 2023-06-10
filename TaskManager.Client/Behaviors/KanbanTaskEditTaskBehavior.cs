﻿using Microsoft.Extensions.Logging;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;
using TaskManager.Client.View.Kanban;
using TaskManager.Core.Models;

namespace TaskManager.Client.Behaviors;

internal class KanbanTaskEditTaskBehavior : Behavior<KanbanTask>
{
    ILogger<KanbanTaskEditTaskBehavior> _logger = App.IoC.GetRequiredService<ILogger< KanbanTaskEditTaskBehavior>>();

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
