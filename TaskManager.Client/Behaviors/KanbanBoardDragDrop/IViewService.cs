using System.Windows;
using System.Windows.Input;
using TaskManager.Client.View.Kanban;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public interface IViewService
{
    #region KanbanBoard DragDrop Behavior
    bool IsKanbanTaskDragged(MouseButtonEventArgs e);
    void Setup(KanbanBoard kanbanBoard, MouseButtonEventArgs e);
    void CaptureMouse();
    void ReleaseMouseCapture();
    void UpdateMousePosition(MouseEventArgs e);
    #endregion

    #region IDragDropHandler
    KanbanBoard GetKanbanBoard();
    KanbanTask GetKanbanTask();
    Point GetCurrentPosition();
    Point GetMouseInsideControl();
    #endregion
}
