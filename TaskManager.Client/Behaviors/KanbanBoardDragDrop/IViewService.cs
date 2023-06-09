using System;
using System.Windows;
using System.Windows.Input;
using TaskManager.Client.View.Kanban;
using TaskManager.Core.Models;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public interface IViewService
{
    #region KanbanBoard DragDrop Behavior
    bool IsSingleClick(MouseButtonEventArgs e);
    bool IsKanbanTaskDragged(MouseButtonEventArgs e);
    void Setup(KanbanBoard kanbanBoard, MouseButtonEventArgs e);
    void CaptureMouse();
    void ReleaseMouseCapture();
    void UpdateMousePosition(MouseEventArgs e);
    #endregion

    #region IDragDropHandler
    Point InitialPosition { get; }
    Point CurrentPosition { get; }
    Point MouseInsideControl { get; }
    Task CoreTask { get; }
    void ShowDraggedKanbanTask(double x, double y);
    void UpdateDraggedKanbanTask(double offsetX, double offsetY);
    void HideDraggedKanbanTask();
    bool IsDraggedKanbanTaskOutsideKanbanBoard();
    bool IsDraggedKanbanTaskOverKandanColumn(out ETaskStatus columnStatus, out double offsetInsideColumn);
    void SetupTaskCollectionManager(ITaskCollectionManager taskCollectionManager);
    #endregion

    #region IAnimationHandler
    double KanbanTaskTotalHeight { get; }
    void ForEachKanbanTask(ETaskStatus columnStatus, Action<KanbanTask, Task> action);
    void StartDoubleAnimation(KanbanTask kanbanTask, double from, Duration duration);
    double GetCurrentTransformValue(KanbanTask kanbanTask);
    #endregion
}
