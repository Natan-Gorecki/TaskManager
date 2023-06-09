using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TaskManager.Client.Extensions;
using TaskManager.Client.Model;
using TaskManager.Client.View.Kanban;
using TaskManager.Core.Extensions;
using TaskManager.Core.Models;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public class ViewService : IViewService
{
    ILogger<ViewService> _logger = App.IoC.GetRequiredService<ILogger<ViewService>>();

    KanbanBoard? _kanbanBoard;
    KanbanTask? _kanbanTask;

    Point? _initialPosition;
    Point? _currentPosition;
    Point? _mouseInsideControl;

    KanbanTask? _draggedKanbanTask;

    #region KanbanBoard DragDrop Behavior
    public bool IsSingleClick(MouseButtonEventArgs e)
    {
        return e.ClickCount == 1;
    }

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

    public void ShowDraggedKanbanTask(double x, double y)
    {
        ArgumentNullException.ThrowIfNull(_kanbanBoard);
        ArgumentNullException.ThrowIfNull(_kanbanTask);

        _draggedKanbanTask = new KanbanTask();
        _draggedKanbanTask.CopyFrom(_kanbanTask);
        _draggedKanbanTask.Width = _kanbanTask.ActualWidth;
        _draggedKanbanTask.Height = _kanbanTask.ActualHeight;
        _draggedKanbanTask.DataContext = _kanbanTask.DataContext;

        var mainWindow = Application.Current.MainWindow as MainWindow;
        ArgumentNullException.ThrowIfNull(mainWindow);
        
        mainWindow.mainWindowCanvas.Children.Add(_draggedKanbanTask);
        Canvas.SetLeft(_draggedKanbanTask, x);
        Canvas.SetTop(_draggedKanbanTask, y);
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

        var mainWindow = Application.Current.MainWindow as MainWindow;
        ArgumentNullException.ThrowIfNull(mainWindow);

        mainWindow.mainWindowCanvas.Children.Remove(_draggedKanbanTask);
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
    }

    public bool IsDraggedKanbanTaskOverKandanColumn(out ETaskStatus columnStatus, out double offsetInsideColumn)
    {
        ArgumentNullException.ThrowIfNull(_draggedKanbanTask);
        ControlDimensions draggedTaskCords = _draggedKanbanTask.GetControlDimensions();

        columnStatus = default;
        offsetInsideColumn = default;

        var kanbanColumn = Application.Current.MainWindow.FindUnderlyingControl<KanbanColumn, KanbanTask>(draggedTaskCords.Center, _draggedKanbanTask);
        if (kanbanColumn is null)
        {
            _logger.LogTrace("Dragged KanbanTask is not over KanbanColumn.");
            return false;
        }

        ListView? columnListView = kanbanColumn.FindChild<ListView>();
        if (columnListView is null)
        {
            return false;
        }

        Point columnTopLeft = columnListView.RelativeToMainWindow();
        columnStatus = kanbanColumn.TaskStatus;
        offsetInsideColumn = draggedTaskCords.Center.Y - columnTopLeft.Y;
        return true;
    }

    public void SetupTaskCollectionManager(ITaskCollectionManager taskCollectionManager)
    {
        ArgumentNullException.ThrowIfNull(_kanbanBoard);
        taskCollectionManager.Setup(_kanbanBoard.TaskCollection);
    }
    #endregion

    #region IAnimationHandler
    public double KanbanTaskTotalHeight 
    {
        get
        {
            ArgumentNullException.ThrowIfNull(_kanbanTask);
            return _kanbanTask.ActualHeight + _kanbanTask.Margin.Top + _kanbanTask.Margin.Bottom;
        }
    }

    public void ForEachKanbanTask(ETaskStatus columnStatus, Action<KanbanTask, Task> action)
    {
        ArgumentNullException.ThrowIfNull(_kanbanBoard);

        KanbanColumn? kanbanColumn = _kanbanBoard.KanbanColumns
            .Where(column => column.TaskStatus == columnStatus)
            .FirstOrDefault();

        ArgumentNullException.ThrowIfNull(kanbanColumn);

        foreach (var kanbanTask in kanbanColumn.KanbanTasks)
        {
            if (kanbanTask.DataContext is not Task coreTask)
            {
                return;
            }

            action(kanbanTask, coreTask);
        }
    }

    public void StartDoubleAnimation(KanbanTask kanbanTask, double from, Duration duration)
    {
        var yAnimation = new DoubleAnimation
        {
            From = from,
            To = 0,
            Duration = duration
        };

        var transform = new TranslateTransform();
        kanbanTask.RenderTransform = transform;
        transform.BeginAnimation(TranslateTransform.YProperty, yAnimation, HandoffBehavior.SnapshotAndReplace);
    }

    public double GetCurrentTransformValue(KanbanTask kanbanTask)
    {
        return (double)kanbanTask.RenderTransform.GetValue(TranslateTransform.YProperty);
    }
    #endregion
}
