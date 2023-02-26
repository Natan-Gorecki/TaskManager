using System.Windows;
using System.Windows.Controls;
using TaskManager.Client.View.Kanban;
using TaskManager.Core.Models;

namespace TaskManager.Client.Model;

internal class DragDropEventArgs
{
    public Point InitialPosition { get; set; }
    public Point MouseInsideControl { get; set; }

    public ControlDimensions MainWindow { get; set; }
    public ControlDimensions KanbanBoard { get; set; }
    public ControlDimensions KanbanColumn { get; set; }
    public ControlDimensions KanbanTask { get; set; }

    public Task Task { get; set; }
    public Task? PreviewTask { get; set; }

    public KanbanTask DraggedKanbanTask { get; set; }
}
