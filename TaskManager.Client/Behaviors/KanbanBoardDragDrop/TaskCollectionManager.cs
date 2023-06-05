using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Core.Models;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public class TaskCollectionManager : ITaskCollectionManager
{
    private ICollection<Task>? _tasks;
    
    public void Setup(ICollection<Task> tasks)
    {
        ArgumentNullException.ThrowIfNull(tasks);
        _tasks = tasks;
    }

    public void AddTask(Task task, bool updateOrder = true)
    {
        ArgumentNullException.ThrowIfNull(_tasks);
        _tasks.Add(task);
        if (updateOrder)
        {
            UpdateTasksOrder(_tasks);
        }
    }

    public void RemoveTask(Task task, bool updateOrder = true)
    {
        ArgumentNullException.ThrowIfNull(_tasks);
        _tasks.Remove(task);
        if(updateOrder)
        {
            UpdateTasksOrder(_tasks);
        }
    }

    private static void UpdateTasksOrder(ICollection<Task> tasks)
    {
        foreach (ETaskStatus taskStatus in Enum.GetValues(typeof(ETaskStatus)))
        {
            UpdateColumnOrder(taskStatus);
        }

        void UpdateColumnOrder(ETaskStatus status)
        {
            var orderedTasks = tasks.Where(t => t.Status == status).OrderBy(t => t.OrderValue).ToList();
            int currentOrder = 10;

            foreach (var item in orderedTasks)
            {
                item.OrderValue = currentOrder;
                currentOrder += 10;
            }
        }
    }
}
