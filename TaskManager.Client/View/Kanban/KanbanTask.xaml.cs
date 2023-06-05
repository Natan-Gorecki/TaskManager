using System.Windows.Controls;
using TaskManager.Client.Extensions;

namespace TaskManager.Client.View.Kanban;

/// <summary>
/// Interaction logic for KanbanTask.xaml
/// </summary>
public partial class KanbanTask : UserControl
{
    public KanbanTask()
    {
        //InitializeComponent();
        this.LoadViewFromUri("/TaskManager.Client;component/view/kanban/kanbantask.xaml");
    }
}
