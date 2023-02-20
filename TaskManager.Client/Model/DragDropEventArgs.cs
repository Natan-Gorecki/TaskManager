using System.Windows;
using System.Windows.Controls;
using TaskManager.Client.View.Kanban;

namespace TaskManager.Client.Model;

internal class DragDropEventArgs
{
    public Point InitialPosition { get; set; }
    
    public KanbanTask KanbanTask { get; set; }
    public ListViewItem ColumnListViewItem { get; set; }
    public ListViewItem BoardListViewItem { get; set; }

    public int ColumnItemZIndex { get; set; }
    public int BoardItemZIndex { get; set; }
}
