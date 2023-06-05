using System.Collections.Generic;
using TaskManager.Core.Models;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public interface ITaskCollectionManager
{
    void Setup(ICollection<Task> tasks);
    void AddTask(Task task, bool updateOrder = true);
    void RemoveTask(Task task, bool updateOrder = true);
}
