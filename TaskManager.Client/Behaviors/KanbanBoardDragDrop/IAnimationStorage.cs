namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

internal interface IAnimationStorage
{
    void Setup(double kanbanTaskHeight);
    void AddAnimation(Animation animation);
}
