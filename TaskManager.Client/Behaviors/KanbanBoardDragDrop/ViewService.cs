using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TaskManager.Client.Extensions;
using TaskManager.Client.Model;
using TaskManager.Client.View.Kanban;
using TaskManager.Core.Extensions;
using TaskManager.Core.Models;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public class ViewService : IViewService
{
    KanbanBoard? _kanbanBoard;
    KanbanTask? _kanbanTask;

    Point? _initialPosition;
    Point? _currentPosition;
    Point? _mouseInsideControl;

    KanbanTask? _draggedKanbanTask;

    #region KanbanBoard DragDrop Behavior
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
        
        _initialPosition = e.GetPosition(Application.Current.MainWindow);
        _currentPosition = _initialPosition;
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
    #endregion

    #region IDragDropHandler
    public Point InitialPosition 
    { 
        get
        {
            ArgumentNullException.ThrowIfNull(_initialPosition);
            return _initialPosition.Value;
        }
    }

    public Point CurrentPosition
    {
        get
        {
            ArgumentNullException.ThrowIfNull(_currentPosition);
            return _currentPosition.Value;
        }
    }

    public Point MouseInsideControl
    {
        get
        {
            ArgumentNullException.ThrowIfNull(_mouseInsideControl);
            return _mouseInsideControl.Value;
        }
    }

    public Task CoreTask
    {
        get
        {
            Task? task = _kanbanTask?.DataContext as Task;
            ArgumentNullException.ThrowIfNull(task);
            return task;
        }
    }

    public void ShowDraggedKanbanTask()
    {
        ArgumentNullException.ThrowIfNull(_kanbanBoard);
        ArgumentNullException.ThrowIfNull(_kanbanTask);

        _draggedKanbanTask = new KanbanTask();
        _draggedKanbanTask.CopyFrom(_kanbanTask);
        _draggedKanbanTask.Width = _kanbanTask.ActualWidth;
        _draggedKanbanTask.Height = _kanbanTask.ActualHeight;
        _draggedKanbanTask.DataContext = _kanbanTask.DataContext;

        _kanbanBoard.previewCanvas.Children.Add(_draggedKanbanTask);

        var topLeft = _kanbanTask.RelativeToMainWindow();
        Canvas.SetLeft(_draggedKanbanTask, topLeft.X);
        Canvas.SetTop(_draggedKanbanTask, topLeft.Y);
    }

    public void UpdateDraggedKanbanTask(double offsetX, double offsetY)
    {
        ArgumentNullException.ThrowIfNull(_draggedKanbanTask);

        _draggedKanbanTask.RenderTransform = new TranslateTransform(offsetX, offsetY);
    }

    public void HideDraggedKanbanTask()
    {
        ArgumentNullException.ThrowIfNull(_kanbanBoard);
        ArgumentNullException.ThrowIfNull(_draggedKanbanTask);

        _kanbanBoard.previewCanvas.Children.Remove(_draggedKanbanTask);
    }

    public bool IsDraggedKanbanTaskOutsideKanbanBoard()
    {
        ArgumentNullException.ThrowIfNull(_kanbanBoard);
        ArgumentNullException.ThrowIfNull(_draggedKanbanTask);

        ControlDimensions kanbanBoardCords = _kanbanBoard.GetControlDimensions();
        ControlDimensions draggedTaskCords = _draggedKanbanTask.GetControlDimensions();
             
        return draggedTaskCords.Center.X < kanbanBoardCords.TopLeft.X
            || draggedTaskCords.Center.X > kanbanBoardCords.RightBottom.X
            || draggedTaskCords.Center.Y < kanbanBoardCords.TopLeft.Y
            || draggedTaskCords.Center.Y > kanbanBoardCords.RightBottom.Y;

        #warning SUS
        //return draggedTaskCords.Center.X < eventArgs.KanbanBoard.TopLeft.X
        //        || draggedTaskCords.Center.X > eventArgs.KanbanBoard.RightBottom.X
        //        || draggedTaskCords.Center.Y < eventArgs.KanbanColumn.TopLeft.Y
        //        || draggedTaskCords.Center.Y > eventArgs.KanbanColumn.RightBottom.Y;
    }

    public bool IsDraggedKanbanTaskOverKandanColumn(out ETaskStatus taskStatus, out double itemTotalHeight, out double offsetInsideColumn)
    {
        ArgumentNullException.ThrowIfNull(_draggedKanbanTask);
        ControlDimensions draggedTaskCords = _draggedKanbanTask.GetControlDimensions();

        taskStatus = default;
        itemTotalHeight = default;
        offsetInsideColumn = default;

        var kanbanColumn = Application.Current.MainWindow.FindUnderlyingControl<KanbanColumn, KanbanTask>(draggedTaskCords.Center, _draggedKanbanTask);
        if (kanbanColumn is null)
        {
            return false;
        }

        ListView? columnListView = kanbanColumn.FindChild<ListView>();
        if (columnListView is null)
        {
            return false;
        }

        Point columnTopLeft = columnListView.RelativeToMainWindow();
        taskStatus = kanbanColumn.TaskStatus;
        itemTotalHeight = _draggedKanbanTask.ActualHeight + _draggedKanbanTask.Margin.Top + _draggedKanbanTask.Margin.Bottom;
        offsetInsideColumn = draggedTaskCords.Center.Y - columnTopLeft.Y;
        return true;
    }

    public void SetupTaskCollectionManager(ITaskCollectionManager taskCollectionManager)
    {
        ArgumentNullException.ThrowIfNull(_kanbanBoard);
        taskCollectionManager.Setup(_kanbanBoard.TaskCollection);
    }

    #endregion

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
