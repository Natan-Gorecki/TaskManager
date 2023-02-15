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

    public KanbanBoard()
    {
        InitializeComponent();
    }
}
