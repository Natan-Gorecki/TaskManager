using System;
using System.Windows;
using System.Windows.Input;
using TaskManager.Client.Extensions;
using TaskManager.Client.View.Kanban;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public class ViewService : IViewService
{
    KanbanBoard? _kanbanBoard;
    KanbanTask? _kanbanTask;
    Point? _currentPosition;
    Point? _mouseInsideControl;

    public bool IsKanbanTaskDragged(MouseButtonEventArgs e)
    {
        KanbanTask? kanbanTask = (e.OriginalSource as DependencyObject)?.FindControlOrAncestor<KanbanTask>();
        return kanbanTask != null;
    }

    public void Setup(KanbanBoard kanbanBoard, MouseButtonEventArgs e)
    {
        ArgumentNullException.ThrowIfNull(kanbanBoard);
        ArgumentNullException.ThrowIfNull(e);

        _kanbanBoard = kanbanBoard;
        _kanbanTask = (e.OriginalSource as DependencyObject)?.FindControlOrAncestor<KanbanTask>();
        _currentPosition = e.GetPosition(Application.Current.MainWindow);
        _mouseInsideControl = e.GetPosition(_kanbanTask);
    }
    
    public void CaptureMouse()
    {
        ArgumentNullException.ThrowIfNull(_kanbanBoard);
        _kanbanBoard.CaptureMouse();
    }
    public void ReleaseMouseCapture()
    {
        ArgumentNullException.ThrowIfNull(_kanbanBoard);
        _kanbanBoard.ReleaseMouseCapture();
    }
    public void UpdateMousePosition(MouseEventArgs e)
    {
        _currentPosition = e.GetPosition(Application.Current.MainWindow);
    }

    public KanbanBoard GetKanbanBoard()
    {
        return _kanbanBoard;
    }
    public KanbanTask GetKanbanTask()
    {
        return _kanbanTask;
    }
    public Point GetCurrentPosition()
    {
        return _currentPosition.Value;
    }
    public Point GetMouseInsideControl()
    {
        return _mouseInsideControl.Value;
    }
}
