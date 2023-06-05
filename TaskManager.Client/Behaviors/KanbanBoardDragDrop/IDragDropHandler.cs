using System.Windows;
using TaskManager.Client.View.Kanban;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public interface IDragDropHandler
{
    bool IsStarted { get; }
    void StartDragDrop(IViewService viewService);
    void UpdateDragDrop();
    void StopDragDrop();
}
