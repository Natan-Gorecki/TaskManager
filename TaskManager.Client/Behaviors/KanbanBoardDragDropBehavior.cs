using Microsoft.Xaml.Behaviors;
using System.Windows.Input;
using TaskManager.Client.Behaviors.KanbanBoardDragDrop;
using TaskManager.Client.View.Kanban;

namespace TaskManager.Client.Behaviors;

public class KanbanBoardDragDropBehavior : Behavior<KanbanBoard>
{
    IDragDropHandler _dragDropHandler = App.IoC.GetRequiredService<IDragDropHandler>();
    IViewService _viewService = App.IoC.GetRequiredService<IViewService>();

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

        if (!_viewService.IsKanbanTaskDragged(e))
        {
            return;
        }

        _viewService.Setup(AssociatedObject, e);
        _viewService.CaptureMouse();
        _dragDropHandler.StartDragDrop(_viewService);
    }

    public void KanbanBoard_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (!_dragDropHandler.IsStarted())
        {
            return;
        }

        _viewService.UpdateMousePosition(e);
        _dragDropHandler.UpdateDragDrop();
    }

    public void KanbanBoard_PreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
        if (!_dragDropHandler.IsStarted())
        {
            return;
        }

        _viewService.ReleaseMouseCapture();
        _dragDropHandler.StopDragDrop();
    }
}
