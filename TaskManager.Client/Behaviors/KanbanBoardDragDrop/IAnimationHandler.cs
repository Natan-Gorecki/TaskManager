using TaskManager.Client.View.Kanban;
using TaskManager.Core.Models;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

internal interface IAnimationHandler
{
    void Setup(KanbanBoard kanbanBoard, double kanbanTaskHeight);
    void HandleAnimation(Task? oldTask, Task? newTask);
}
