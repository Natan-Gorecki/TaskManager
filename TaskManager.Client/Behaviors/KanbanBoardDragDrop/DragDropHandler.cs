using System;
using TaskManager.Core.Extensions;
using TaskManager.Core.Models;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public class DragDropHandler : IDragDropHandler
{
    private IAnimationHandler _animationHandler = App.IoC.GetRequiredService<IAnimationHandler>();
    private ITaskCollectionManager _taskCollectionManager = App.IoC.GetRequiredService<ITaskCollectionManager>();
    private IViewService? _viewService;

    private Task? _orginalTask;
    private Task? _previewTask;

    public bool IsStarted { get; private set; } = false;

    public void StartDragDrop(IViewService viewService)
    {
        ArgumentNullException.ThrowIfNull(viewService);
        _viewService = viewService;

        IsStarted = true;
        _animationHandler.Setup(_viewService);
        _viewService.SetupTaskCollectionManager(_taskCollectionManager);

        _orginalTask = _viewService.CoreTask;
        _previewTask = new Task();
        _previewTask.CopyFrom(_orginalTask);
        _previewTask.IsPreview = true;
        _previewTask.OrderValue -= 9;

        _taskCollectionManager.RemoveTask(_orginalTask, updateOrder: true);
        _taskCollectionManager.AddTask(_previewTask, updateOrder: false);

        _viewService.ShowDraggedKanbanTask();
    }

    public void UpdateDragDrop()
    {
        ArgumentNullException.ThrowIfNull(_viewService);

        double offsetX = _viewService.CurrentPosition.X - _viewService.InitialPosition.X;
        double offsetY = _viewService.CurrentPosition.Y - _viewService.InitialPosition.Y;
        _viewService.UpdateDraggedKanbanTask(offsetX, offsetY);


        if (_viewService.IsDraggedKanbanTaskOutsideKanbanBoard() && _previewTask != null)
        {
            _taskCollectionManager.RemoveTask(_previewTask, updateOrder: false);
            _animationHandler.HandleAnimation(_previewTask, null);
            _previewTask = null;
            return;
        }

        if (!_viewService.IsDraggedKanbanTaskOverKandanColumn(out var columnStatus, out var itemTotalHeight, out var offsetInsideColumn))
        {
            return;
        }

        Task newPreviewTask = new Task
        {
            Status = columnStatus,
            OrderValue = CalculateOrderValue(itemTotalHeight, offsetInsideColumn),
            IsPreview = true
        };

        if (_previewTask is null)
        {
            _previewTask = newPreviewTask;
            _taskCollectionManager.AddTask(_previewTask, updateOrder: false);
            _animationHandler.HandleAnimation(null, _previewTask);
            return;
        }

        if (!PreviewTaskPositionChanged(_previewTask, newPreviewTask))
        {
            return;
        }

        _taskCollectionManager.RemoveTask(_previewTask, updateOrder: false);
        _taskCollectionManager.AddTask(newPreviewTask, updateOrder: false);
        _animationHandler.HandleAnimation(_previewTask, newPreviewTask);
        _previewTask = newPreviewTask;
    }

    public void StopDragDrop()
    {
        ArgumentNullException.ThrowIfNull(_viewService);
        ArgumentNullException.ThrowIfNull(_orginalTask);

        IsStarted = false;

        if(_previewTask != null)
        {
            _taskCollectionManager.RemoveTask(_previewTask, updateOrder: false);
            _orginalTask.Status = _previewTask.Status;
            _orginalTask.OrderValue = _previewTask.OrderValue;
        }

        _taskCollectionManager.AddTask(_orginalTask, updateOrder: true);
        _viewService.HideDraggedKanbanTask();
    }

    private int CalculateOrderValue(double itemTotalHeight, double offsetInsideColumn)
    {
        int orderValue = 1 + (int)Math.Floor(offsetInsideColumn / itemTotalHeight) * 10;
        if (orderValue < 0)
        {
            orderValue = 1;
        }
        return orderValue;
    }

    private static bool PreviewTaskPositionChanged(Task oldTask, Task newTask)
    {
        return oldTask.OrderValue != newTask.OrderValue
            || oldTask.Status != newTask.Status;
    }
}
