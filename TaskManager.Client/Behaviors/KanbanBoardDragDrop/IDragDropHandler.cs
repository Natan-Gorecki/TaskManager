using System.Windows;
using TaskManager.Client.View.Kanban;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public interface IDragDropHandler
{
    bool IsStarted();
    void StartDragDrop(KanbanBoard kanbanBoard, KanbanTask kanbanTask, Point initialPosition, Point mouseInsideControl);
    void UpdateDragDrop(Point currentPosition);
    void StopDragDrop();
}
