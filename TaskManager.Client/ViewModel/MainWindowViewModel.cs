using System.Collections.ObjectModel;
using TaskManager.Core.Models;

namespace TaskManager.Client.ViewModel;

internal class MainWindowViewModel
{
    public ObservableCollection<Task> TaskCollection { get; set; }

    public MainWindowViewModel()
    {
        TaskCollection = new ObservableCollection<Task>()
        {
            new()
            {
                Id = 1,
                Name = "Task 1",
                Status = ETaskStatus.Waiting,
                OrderValue = 10
            },
            new()
            {
                Id = 7,
                Name = "Task 7",
                Status = ETaskStatus.Waiting,
                OrderValue = 20
            },
            new Task()
            {
                Id = 2,
                Name = "Task 2",
                Status = ETaskStatus.InProgress,
                OrderValue = 10
            },
            new Task()
            {
                Id = 3,
                Name = "Task 4",
                Status = ETaskStatus.Completed,
                OrderValue = 30
            },
            new Task()
            {
                Id = 5,
                Name = "Task 5",
                Status = ETaskStatus.Completed,
                OrderValue = 20
            },
            new Task()
            {
                Id = 6,
                Name = "Task 6",
                Status = ETaskStatus.Completed,
                OrderValue = 10
            }
        };
    }
}
