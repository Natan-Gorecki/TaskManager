using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Windows;
using TaskManager.Client.View.Kanban;
using TaskManager.Core.Models;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public class AnimationHandler : IAnimationHandler
{
    private ILogger<AnimationHandler> _logger = App.IoC.GetRequiredService<ILogger<AnimationHandler>>();
    private IViewService? _viewService;

    private readonly TimeSpan _animationDuration = TimeSpan.FromSeconds(0.15);
    private Dictionary<int, Animation> _ongoingAnimations = new();

    public void Setup(IViewService viewService)
    {
        ArgumentNullException.ThrowIfNull(viewService);
        _viewService = viewService;
    }

    public void HandleAnimation(Task? oldTask, Task? newTask)
    {
        if (oldTask == null && newTask != null)
        {
            AddAnimations(Direction.Bottom, newTask.Status, coreTask => coreTask.OrderValue > newTask.OrderValue);
            return;
        }

        if (oldTask != null && newTask == null)
        {
            AddAnimations(Direction.Top, oldTask.Status, coreTask => coreTask.OrderValue > oldTask.OrderValue);
            return;
        }

        if (oldTask == null || newTask == null)
        {
            _logger.LogError("Cannot handle animation - both tasks are null");
            return;
        }

        int oldValue = oldTask.OrderValue;
        int newValue = newTask.OrderValue;

        if (oldTask.Status != newTask.Status)
        {
            AddAnimations(Direction.Top, oldTask.Status, coreTask => coreTask.OrderValue > oldValue);
            AddAnimations(Direction.Bottom, newTask.Status, coreTask => coreTask.OrderValue > newValue);
        }
        else if (oldValue > newValue)
        {
            AddAnimations(Direction.Bottom, newTask.Status, coreTask => coreTask.OrderValue > newValue && coreTask.OrderValue < oldValue);
        }
        else
        {
            AddAnimations(Direction.Top, newTask.Status, coreTask => coreTask.OrderValue > oldValue && coreTask.OrderValue < newValue);
        }
    }

    public virtual void AddAnimation(Animation animation)
    {
        ArgumentNullException.ThrowIfNull(_viewService);

        RemoveIfCompleted(animation.Id, animation.KanbanTask);
        if (!_ongoingAnimations.TryGetValue(animation.Id, out Animation? ongoingAnimation))
        {
            _viewService.StartDoubleAnimation(animation.KanbanTask, animation.From, _animationDuration);
            _ongoingAnimations.Add(animation.Id, animation);
            return;
        }

        if (animation.Direction == ongoingAnimation.Direction)
        {
            return;
        }

        double yTransform = _viewService.GetCurrentTransformValue(animation.KanbanTask);
        double totalHeight = _viewService.KanbanTaskTotalHeight;
        double from = yTransform >= 0 ? yTransform - totalHeight : yTransform + totalHeight;
        Duration duration = Math.Abs(from / totalHeight) * _animationDuration;

        _viewService.StartDoubleAnimation(animation.KanbanTask, from, duration);

        _ongoingAnimations.Remove(animation.Id);
        _ongoingAnimations.Add(animation.Id, animation);
    }

    private void AddAnimations(Direction direction, ETaskStatus columnStatus, Predicate<Task> predicate)
    {
        ArgumentNullException.ThrowIfNull(_viewService);
        var totalHeight = _viewService.KanbanTaskTotalHeight;

        _viewService.ForEachKanbanTask(columnStatus, (kanbanTask, coreTask) =>
        {
            if (predicate(coreTask))
            {
                AddAnimation(new Animation
                {
                    Id = coreTask.Id,
                    KanbanTask = kanbanTask,
                    From = direction == Direction.Top ? totalHeight : -totalHeight,
                    Direction = direction
                });
            }
        });
    }

    private void RemoveIfCompleted(int taskId, KanbanTask kanbanTask)
    {
        ArgumentNullException.ThrowIfNull(_viewService);

        if (_viewService.GetCurrentTransformValue(kanbanTask) != 0)
        {
            return;
        }

        _ongoingAnimations.Remove(taskId);
    }
}
