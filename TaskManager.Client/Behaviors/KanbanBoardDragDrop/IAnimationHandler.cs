﻿using TaskManager.Client.View.Kanban;
using TaskManager.Core.Models;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public interface IAnimationHandler
{
    void Setup(IViewService viewService);
    void HandleAnimation(Task? oldTask, Task? newTask);
    void AddAnimation(Animation animation);
}
