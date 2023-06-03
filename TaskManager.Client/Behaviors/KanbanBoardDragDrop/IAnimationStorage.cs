namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public interface IAnimationStorage
{
    void Setup(double kanbanTaskHeight);
    void AddAnimation(Animation animation);
}
