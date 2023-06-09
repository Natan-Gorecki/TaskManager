using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Input;
using TaskManager.Client.View.Kanban;

namespace TaskManager.Client.Behaviors;

internal class KanbanTaskEditTaskBehavior : Behavior<KanbanTask>
{
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
            return;
        }

        mainWindow.modalPage.Visibility = Visibility.Visible;
    }
}
