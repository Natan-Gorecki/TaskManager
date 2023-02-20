using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TaskManager.Client.Extensions;
using TaskManager.Client.Model;
using TaskManager.Core.Models;

namespace TaskManager.Client.View.Kanban;

/// <summary>
/// Interaction logic for KanbanBoard.xaml
/// </summary>
public partial class KanbanBoard : UserControl
{
    public static readonly DependencyProperty TaskCollectionProperty =
        DependencyProperty.Register("TaskCollection", typeof(ObservableCollection<Task>), typeof(KanbanBoard));

    public ObservableCollection<Task> TaskCollection
    {
        get { return (ObservableCollection<Task>)GetValue(TaskCollectionProperty); }
        set { SetValue(TaskCollectionProperty, value); }
    }

    DragDropEventArgs? _dragDropEventArgs = null;

    public KanbanBoard()
    {
        InitializeComponent();
    }

    private void KanbanBoard_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        DependencyObject dependencyObject  = (DependencyObject)e.OriginalSource;
        KanbanTask? kanbanTask = dependencyObject.FindControl<KanbanTask>();

        if(kanbanTask is null)
        {
            return;
        }
        
        ListViewItem columnItem = kanbanTask.FindAncestor<ListViewItem>()!;
        ListViewItem boardItem = columnItem.FindAncestor<ListViewItem>()!;

        _dragDropEventArgs = new DragDropEventArgs
        {
            InitialPosition = e.GetPosition(Application.Current.MainWindow),
            KanbanTask = kanbanTask,
            ColumnListViewItem = columnItem,
            BoardListViewItem = boardItem,
            ColumnItemZIndex = Panel.GetZIndex(columnItem),
            BoardItemZIndex = Panel.GetZIndex(boardItem)
        };

        Panel.SetZIndex(columnItem, int.MaxValue);
        Panel.SetZIndex(boardItem, int.MaxValue);
    }

    private void KanbanBoard_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (_dragDropEventArgs is null)
        {
            return;
        }

        Point currentPosition = e.GetPosition(Application.Current.MainWindow);
        double offsetX = currentPosition.X - _dragDropEventArgs.InitialPosition.X;
        double offsetY = currentPosition.Y - _dragDropEventArgs.InitialPosition.Y;

        _dragDropEventArgs.KanbanTask.RenderTransform = new TranslateTransform(offsetX, offsetY);
    }

    private void KanbanBoard_PreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
        if (_dragDropEventArgs != null)
        {
            Panel.SetZIndex(_dragDropEventArgs.ColumnListViewItem, _dragDropEventArgs.ColumnItemZIndex);
            Panel.SetZIndex(_dragDropEventArgs.BoardListViewItem, _dragDropEventArgs.BoardItemZIndex);

            _dragDropEventArgs.KanbanTask.RenderTransform = new TranslateTransform();

            _dragDropEventArgs = null;
        }
    }
}
