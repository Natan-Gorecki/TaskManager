using TaskManager.Client.View.Kanban;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public class Animation
{
    public required int Id { get; init; }
    public required KanbanTask KanbanTask { get; init; }
    public required double From { get; init; }
    public required Direction Direction { get; init; }
}
