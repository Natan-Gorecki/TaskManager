using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
