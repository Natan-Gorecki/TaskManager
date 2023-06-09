using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TaskManager.Client.View.Kanban;
using TaskManager.Core.Models;

namespace TaskManager.Client.View.Backlog;

/// <summary>
/// Interaction logic for BacklogList.xaml
/// </summary>
public partial class BacklogList : UserControl
{
    public static readonly DependencyProperty TaskCollectionProperty =
        DependencyProperty.Register("TaskCollection", typeof(ObservableCollection<Task>), typeof(BacklogList));

    public ObservableCollection<Task> TaskCollection
    {
        get { return (ObservableCollection<Task>)GetValue(TaskCollectionProperty); }
        set { SetValue(TaskCollectionProperty, value); }
    }

    public BacklogList()
    {
        InitializeComponent();
    }
}
