using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using TaskManager.Client.View.Kanban;
using TaskManager.Core.Models;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public class AnimationHandler : IAnimationHandler
{
    private ILogger<AnimationHandler> _logger = App.IoC.GetRequiredService<ILogger<AnimationHandler>>();
    private IAnimationStorage _animationStorage = App.IoC.GetRequiredService<IAnimationStorage>();

    private KanbanBoard? _kanbanBoard;
    private double? _kanbanTaskHeight;

    public void Setup(KanbanBoard kanbanBoard, double kanbanTaskHeight)
    {
        ArgumentNullException.ThrowIfNull(kanbanBoard);

        _animationStorage.Setup(kanbanTaskHeight);

        _kanbanBoard = kanbanBoard;
        _kanbanTaskHeight = kanbanTaskHeight;
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

    private void AddAnimations(Direction direction, ETaskStatus? columnStatus, Predicate<Task> predicate)
    {
        ArgumentNullException.ThrowIfNull(_kanbanBoard);
        ArgumentNullException.ThrowIfNull(_kanbanTaskHeight);

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

            if (predicate(coreTask))
            {
                _animationStorage.AddAnimation(new Animation
                {
                    KanbanTask = kanbanTask,
                    From = direction == Direction.Top ? _kanbanTaskHeight.Value : -_kanbanTaskHeight.Value,
                    Direction = direction
                });
            }
        }
    }
}
