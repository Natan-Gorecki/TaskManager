using System.Collections.Generic;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public interface ITaskCollectionManager
{
    void Setup(ICollection<Task> tasks);
    void AddTask(Task task, bool updateOrder = true);
    void RemoveTask(Task task, bool updateOrder = true);
}
