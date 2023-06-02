using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManager.Client.Extensions;
using TaskManager.Core.Models;

namespace TaskManager.Client.View.Kanban
{
    /// <summary>
    /// Interaction logic for KanbanColumn.xaml
    /// </summary>
    public partial class KanbanColumn : UserControl
    {
        public static readonly DependencyProperty TaskCollectionProperty =
        DependencyProperty.Register("TaskCollection", typeof(ObservableCollection<Task>), typeof(KanbanColumn));

        
        public static readonly DependencyProperty TaskStatusProperty =
        DependencyProperty.Register("TaskStatus", typeof(ETaskStatus), typeof(KanbanColumn), new PropertyMetadata(ETaskStatus.Waiting));

        public ObservableCollection<Task> TaskCollection
        {
            get { return (ObservableCollection<Task>)GetValue(TaskStatusProperty); }
            set { SetValue(TaskStatusProperty, value); }
        }

        public ETaskStatus TaskStatus
        {
            get { return (ETaskStatus)GetValue(TaskStatusProperty); }
            set { SetValue(TaskStatusProperty, value); }
        }

        public IEnumerable<KanbanTask> KanbanTasks
        {
            get => this.FindChildren<KanbanTask>();
        }

        public KanbanColumn()
        {
            InitializeComponent();
        }

        private void TaskCollection_StatusFilter(object sender, FilterEventArgs e)
        {
            var task = e.Item as Task;
            e.Accepted = task?.Status == TaskStatus;
        }
    }
}
