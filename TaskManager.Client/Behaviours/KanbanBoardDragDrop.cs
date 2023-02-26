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

namespace TaskManager.Client.Behaviours;

internal class KanbanBoardDragDrop : Behavior<KanbanBoard>
{
    DragDropEventArgs? _eventArgs = null;

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.PreviewMouseDown += KanbanBoard_PreviewMouseDown;
        AssociatedObject.PreviewMouseMove += KanbanBoard_PreviewMouseMove;
        AssociatedObject.PreviewMouseUp += KanbanBoard_PreviewMouseUp;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.PreviewMouseDown -= KanbanBoard_PreviewMouseDown;
        AssociatedObject.PreviewMouseMove -= KanbanBoard_PreviewMouseMove;
        AssociatedObject.PreviewMouseUp -= KanbanBoard_PreviewMouseUp;
    }

    private void KanbanBoard_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        AssociatedObject.CaptureMouse();

        DependencyObject dependencyObject = (DependencyObject)e.OriginalSource;
        KanbanTask? kanbanTask = dependencyObject.FindControlOrAncestor<KanbanTask>();
        if (kanbanTask is null)
        {
            return;
        }

        _eventArgs = CreateDragDropEventArgs(kanbanTask, e);

        AssociatedObject.TaskCollection.Remove(_eventArgs.Task);
        UpdateTaskOrder();
        AssociatedObject.TaskCollection.Add(_eventArgs.PreviewTask!);
        
        AssociatedObject.previewCanvas.Children.Add(_eventArgs.DraggedKanbanTask);
        Canvas.SetLeft(_eventArgs.DraggedKanbanTask, _eventArgs.KanbanTask.TopLeft.X);
        Canvas.SetTop(_eventArgs.DraggedKanbanTask, _eventArgs.KanbanTask.TopLeft.Y);
    }

    private void KanbanBoard_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (_eventArgs is null)
        {
            return;
        }

        Point currentPosition = e.GetPosition(Application.Current.MainWindow);
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

        if(draggedTaskCords.Center.X < _eventArgs.KanbanBoard.TopLeft.X || draggedTaskCords.Center.X > _eventArgs.KanbanBoard.RightBottom.X
            || draggedTaskCords.Center.Y < _eventArgs.KanbanColumn.TopLeft.Y || draggedTaskCords.Center.Y > _eventArgs.KanbanColumn.RightBottom.Y)
        {
            if(_eventArgs.PreviewTask != null)
            {
                AssociatedObject.TaskCollection.Remove(_eventArgs.PreviewTask);
                _eventArgs.PreviewTask = null;
                return;
            }
        }

        var kanbanColumn = Application.Current.MainWindow.FindUnderlyingControl<KanbanColumn, KanbanTask>(draggedTaskCords.Center, _eventArgs.DraggedKanbanTask);
        if (kanbanColumn != null)
        {
            UpdatePreviewTask(kanbanColumn, draggedTaskCords.Center);
            Debug.WriteLine($"Point X: {draggedTaskCords.Center.X}, Y: {draggedTaskCords.Center.Y} - column {kanbanColumn.TaskStatus}");

        }
        else
        {
            Debug.WriteLine($"Point X: {draggedTaskCords.Center.X}, Y: {draggedTaskCords.Center.Y} - column not found!");
        }
    }

    private void KanbanBoard_PreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
        if (_eventArgs != null)
        {
            AssociatedObject.ReleaseMouseCapture();

            if (_eventArgs.PreviewTask != null)
            {
                AssociatedObject.TaskCollection.Remove(_eventArgs.PreviewTask);
                _eventArgs.Task.Status = _eventArgs.PreviewTask.Status;
                _eventArgs.Task.OrderValue = _eventArgs.PreviewTask.OrderValue;
            }

            AssociatedObject.TaskCollection.Add(_eventArgs.Task);
            UpdateTaskOrder();
            AssociatedObject.previewCanvas.Children.Remove(_eventArgs.DraggedKanbanTask);

            _eventArgs = null;
        }
    }

    private void UpdateTaskOrder()
    {
        foreach (ETaskStatus taskStatus in Enum.GetValues(typeof(ETaskStatus)))
        {
            UpdateColumnOrder(taskStatus);
        }

        void UpdateColumnOrder(ETaskStatus status)
        {
            var orderedTasks = AssociatedObject.TaskCollection.Where(t => t.Status == status).OrderBy(t => t.OrderValue).ToList();
            int currentOrder = 10;

            foreach (var item in orderedTasks)
            {
                item.OrderValue = currentOrder;
                currentOrder += 10;
            }
        }
    }

    private DragDropEventArgs CreateDragDropEventArgs(KanbanTask kanbanTask, MouseEventArgs mouseEventArgs)
    {
        ArgumentNullException.ThrowIfNull(kanbanTask);
        ArgumentNullException.ThrowIfNull(mouseEventArgs);

        KanbanColumn? kanbanColumn = kanbanTask.FindAncestor<KanbanColumn>();
        if(kanbanColumn is null)
        {
            throw new ArgumentException($"Missing {nameof(KanbanColumn)} parent control.");
        }

        KanbanBoard? kanbanBoard = kanbanColumn.FindAncestor<KanbanBoard>();
        if(kanbanBoard is null)
        {
            throw new ArgumentException($"Missing {nameof(KanbanBoard)} parent control.");
        }

        if (kanbanTask.DataContext is not Task task)
        {
            throw new ArgumentException($"DataContext of {nameof(KanbanTask)} is not {typeof(Task).Name}.");
        }

        var topLeft = kanbanTask.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));

        Task previewTask = ShallowCopy(task);
        previewTask.OrderValue = task.OrderValue - 5;

        KanbanTask previewKanbanTask = ShallowCopy(kanbanTask);

        DragDropEventArgs eventArgs = new DragDropEventArgs
        {
            InitialPosition = mouseEventArgs.GetPosition(Application.Current.MainWindow),
            MouseInsideControl = mouseEventArgs.GetPosition(kanbanTask),

            MainWindow = new ControlDimensions
            {
                TopLeft = new Point(0,0),
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

        int orderValue = 1 + (int)Math.Floor(offsetY / (itemHeight / 2)) * 5;

        Task previewTask = new Task
        {
            Status = kanbanColumn.TaskStatus,
            OrderValue = orderValue,
            IsPreview = true
        };

        if (_eventArgs.PreviewTask is null)
        {
            _eventArgs.PreviewTask = previewTask;
            AssociatedObject.TaskCollection.Add(previewTask);
            return;
        }

        if (_eventArgs.PreviewTask.OrderValue != previewTask.OrderValue
            || _eventArgs.PreviewTask.Status != previewTask.Status)
        {
            AssociatedObject.TaskCollection.Remove(_eventArgs.PreviewTask);
            _eventArgs.PreviewTask = previewTask;
            AssociatedObject.TaskCollection.Add(previewTask);
        }
    }
}
