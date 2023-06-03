using TaskManager.Client.View.Kanban;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public class Animation
{
    public required KanbanTask KanbanTask { get; set; }
    public required double From { get; set; }
    public required Direction Direction { get; set; }
}
