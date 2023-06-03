using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using TaskManager.Client.Extensions;
using TaskManager.Client.Model;
using TaskManager.Client.View.Kanban;
using System.Diagnostics;
using System.Windows.Media;
using System;
using TaskManager.Core.Extensions;
using TaskManager.Core.Models;
using System.Linq;
using System.Diagnostics.Tracing;
using System.Windows.Media.Animation;
using System.Xaml.Schema;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using Task = TaskManager.Core.Models.Task;
using TaskManager.Client.Behaviors.KanbanBoardDragDrop;

namespace TaskManager.Client.Behaviors;

public class KanbanBoardDragDropBehavior : Behavior<KanbanBoard>
{
    IDragDropHandler _dragDropHandler = App.IoC.GetRequiredService<IDragDropHandler>();

    protected override void OnAttached()
    {
        AssociatedObject.PreviewMouseDown += KanbanBoard_PreviewMouseDown;
        AssociatedObject.PreviewMouseMove += KanbanBoard_PreviewMouseMove;
        AssociatedObject.PreviewMouseUp += KanbanBoard_PreviewMouseUp;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.PreviewMouseDown -= KanbanBoard_PreviewMouseDown;
        AssociatedObject.PreviewMouseMove -= KanbanBoard_PreviewMouseMove;
        AssociatedObject.PreviewMouseUp -= KanbanBoard_PreviewMouseUp;
    }

    public void KanbanBoard_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (_dragDropHandler.IsStarted())
        {
            return;
        }

        KanbanTask? kanbanTask = (e.OriginalSource as DependencyObject)?.FindControlOrAncestor<KanbanTask>();
        if (kanbanTask is null)
        {
            return;
        }

        AssociatedObject.CaptureMouse();
        _dragDropHandler.StartDragDrop(AssociatedObject, kanbanTask, e.GetPosition(Application.Current.MainWindow), e.GetPosition(kanbanTask));
    }

    public void KanbanBoard_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (!_dragDropHandler.IsStarted())
        {
            return;
        }

        _dragDropHandler.UpdateDragDrop(e.GetPosition(Application.Current.MainWindow));
    }

    public void KanbanBoard_PreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
        if (!_dragDropHandler.IsStarted())
        {
            return;
        }

        AssociatedObject.ReleaseMouseCapture();
        _dragDropHandler.StopDragDrop();
    }
}
