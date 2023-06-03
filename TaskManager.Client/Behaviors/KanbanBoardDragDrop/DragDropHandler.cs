using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TaskManager.Client.Extensions;
using TaskManager.Client.Model;
using TaskManager.Client.View.Kanban;
using TaskManager.Core.Extensions;
using TaskManager.Core.Models;
using Task = TaskManager.Core.Models.Task;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public class DragDropHandler : IDragDropHandler
{
    DragDropEventArgs _eventArgs;

    private IAnimationHandler _animationHandler = App.IoC.GetRequiredService<IAnimationHandler>();

    private bool _isStarted = false;
    private ObservableCollection<Task>? _tasks;
    private Canvas? _previewCanvas;

    public bool IsStarted() => _isStarted;

    public void StartDragDrop(KanbanBoard kanbanBoard, KanbanTask kanbanTask, Point initialPosition, Point mouseInsideControl)
    {
        ArgumentNullException.ThrowIfNull(kanbanBoard);
        ArgumentNullException.ThrowIfNull(kanbanTask);

        _isStarted = true;
        _tasks = kanbanBoard.TaskCollection;
        _previewCanvas = kanbanBoard.previewCanvas;

        _eventArgs = CreateDragDropEventArgs(kanbanTask, initialPosition, mouseInsideControl);
        _animationHandler.Setup(kanbanBoard, _eventArgs.KanbanTask.Height);



        _tasks.Remove(_eventArgs.Task);
        UpdateTaskOrder();
        _tasks.Add(_eventArgs.PreviewTask!);

        _previewCanvas.Children.Add(_eventArgs.DraggedKanbanTask);
        Canvas.SetLeft(_eventArgs.DraggedKanbanTask, _eventArgs.KanbanTask.TopLeft.X);
        Canvas.SetTop(_eventArgs.DraggedKanbanTask, _eventArgs.KanbanTask.TopLeft.Y);
    }

    public void UpdateDragDrop(Point currentPosition)
    {
        double offsetX = currentPosition.X - _eventArgs.InitialPosition.X;
        double offsetY = currentPosition.Y - _eventArgs.InitialPosition.Y;

        _eventArgs.DraggedKanbanTask.RenderTransform = new TranslateTransform(offsetX, offsetY);

        Point topLeft = _eventArgs.DraggedKanbanTask.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
        ControlDimensions draggedTaskCords = new ControlDimensions
        {
            TopLeft = topLeft,
            Height = _eventArgs.DraggedKanbanTask.ActualHeight,
            Width = _eventArgs.DraggedKanbanTask.ActualWidth,
        };

        if (IsOutsideKanbanBoard(draggedTaskCords, _eventArgs) && _eventArgs.PreviewTask != null)
        {
            _tasks.Remove(_eventArgs.PreviewTask);
            _animationHandler.HandleAnimation(_eventArgs.PreviewTask, null);
            _eventArgs.PreviewTask = null;
            return;
        }

        var kanbanColumn = Application.Current.MainWindow.FindUnderlyingControl<KanbanColumn, KanbanTask>(draggedTaskCords.Center, _eventArgs.DraggedKanbanTask);
        if (kanbanColumn is null)
        {
            return;
        }

        UpdatePreviewTask(kanbanColumn, draggedTaskCords.Center);
    }

    public void StopDragDrop()
    {
        _isStarted = false;

        if (_eventArgs.PreviewTask != null)
        {
            _tasks.Remove(_eventArgs.PreviewTask);
            _eventArgs.Task.Status = _eventArgs.PreviewTask.Status;
            _eventArgs.Task.OrderValue = _eventArgs.PreviewTask.OrderValue;
        }

        _tasks.Add(_eventArgs.Task);
        UpdateTaskOrder();
        _previewCanvas.Children.Remove(_eventArgs.DraggedKanbanTask);

        _eventArgs = null;
    }

    private static bool IsOutsideKanbanBoard(ControlDimensions draggedTaskCords, DragDropEventArgs eventArgs)
    {
        return draggedTaskCords.Center.X < eventArgs.KanbanBoard.TopLeft.X
            || draggedTaskCords.Center.X > eventArgs.KanbanBoard.RightBottom.X
            || draggedTaskCords.Center.Y < eventArgs.KanbanColumn.TopLeft.Y
            || draggedTaskCords.Center.Y > eventArgs.KanbanColumn.RightBottom.Y;
    }

    private void UpdateTaskOrder()
    {
        foreach (ETaskStatus taskStatus in Enum.GetValues(typeof(ETaskStatus)))
        {
            UpdateColumnOrder(taskStatus);
        }

        void UpdateColumnOrder(ETaskStatus status)
        {
            var orderedTasks = _tasks.Where(t => t.Status == status).OrderBy(t => t.OrderValue).ToList();
            int currentOrder = 10;

            foreach (var item in orderedTasks)
            {
                item.OrderValue = currentOrder;
                currentOrder += 10;
            }
        }
    }

    private DragDropEventArgs CreateDragDropEventArgs(KanbanTask kanbanTask, Point initialPosition, Point mouseInsideControl)
    {
        ArgumentNullException.ThrowIfNull(kanbanTask);

        KanbanColumn? kanbanColumn = kanbanTask.FindAncestor<KanbanColumn>();
        if (kanbanColumn is null)
        {
            throw new ArgumentException($"Missing {nameof(KanbanColumn)} parent control.");
        }

        KanbanBoard? kanbanBoard = kanbanColumn.FindAncestor<KanbanBoard>();
        if (kanbanBoard is null)
        {
            throw new ArgumentException($"Missing {nameof(KanbanBoard)} parent control.");
        }

        if (kanbanTask.DataContext is not Task task)
        {
            throw new ArgumentException($"DataContext of {nameof(KanbanTask)} is not {typeof(Task).Name}.");
        }

        var topLeft = kanbanTask.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));

        Task previewTask = ShallowCopy(task);
        previewTask.IsPreview = true;
        previewTask.OrderValue = task.OrderValue - 9;

        KanbanTask previewKanbanTask = ShallowCopy(kanbanTask);

        DragDropEventArgs eventArgs = new DragDropEventArgs
        {
            InitialPosition = initialPosition,
            MouseInsideControl = mouseInsideControl,

            MainWindow = new ControlDimensions
            {
                TopLeft = new Point(0, 0),
                Width = Application.Current.MainWindow.ActualWidth,
                Height = Application.Current.MainWindow.ActualHeight,
            },
            KanbanBoard = new ControlDimensions
            {
                TopLeft = kanbanBoard.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)),
                Width = kanbanBoard.ActualWidth,
                Height = kanbanBoard.ActualHeight,
            },
            KanbanColumn = new ControlDimensions
            {
                TopLeft = kanbanColumn.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)),
                Width = kanbanColumn.ActualWidth,
                Height = kanbanColumn.ActualHeight,
            },
            KanbanTask = new ControlDimensions
            {
                TopLeft = kanbanTask.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)),
                Width = kanbanTask.ActualWidth,
                Height = kanbanTask.ActualHeight,
            },

            Task = task,

            PreviewTask = previewTask,
            DraggedKanbanTask = previewKanbanTask
        };

        return eventArgs;
    }

    private Task ShallowCopy(Task origin)
    {
        Task task = new Task();
        task.CopyFrom(origin);
        return task;
    }

    private KanbanTask ShallowCopy(KanbanTask origin)
    {
        KanbanTask kanbanTask = new KanbanTask();

        kanbanTask.CopyFrom(origin);
        kanbanTask.Width = origin.ActualWidth;
        kanbanTask.Height = origin.ActualHeight;
        kanbanTask.DataContext = origin.DataContext;

        return kanbanTask;
    }

    private void UpdatePreviewTask(KanbanColumn kanbanColumn, Point point)
    {
        KanbanTask kanbanTask = _eventArgs!.DraggedKanbanTask;

        ListView? columnListView = kanbanColumn.FindChild<ListView>();
        if (columnListView is null)
        {
            throw new ArgumentException($"{nameof(KanbanColumn)} doesnt have {nameof(ListView)} child control.");
        }

        Point columnTopLeft = columnListView.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));

        double itemHeight = kanbanTask.ActualHeight + kanbanTask.Margin.Top + kanbanTask.Margin.Bottom;
        double offsetY = point.Y - columnTopLeft.Y;

        int orderValue = 1 + (int)Math.Floor(offsetY / itemHeight) * 10;
        if (orderValue < 0) orderValue = 1;

        //Trace.WriteLine($"Order value: {orderValue}");

        Task previewTask = new Task
        {
            Status = kanbanColumn.TaskStatus,
            OrderValue = orderValue,
            IsPreview = true
        };

        if (_eventArgs.PreviewTask is null)
        {
            _eventArgs.PreviousPreviewTask = previewTask;
            _eventArgs.PreviewTask = previewTask;

            _tasks.Add(previewTask);
            _animationHandler.HandleAnimation(null, previewTask);

            return;
        }

        if (_eventArgs.PreviewTask.OrderValue != previewTask.OrderValue
            || _eventArgs.PreviewTask.Status != previewTask.Status)
        {
            _tasks.Remove(_eventArgs.PreviewTask);

            _eventArgs.PreviousPreviewTask = _eventArgs.PreviewTask;
            _eventArgs.PreviewTask = previewTask;

            _tasks.Add(previewTask);

            _animationHandler.HandleAnimation(_eventArgs.PreviousPreviewTask, _eventArgs.PreviewTask);
        }
    }
}
