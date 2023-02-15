using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
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
                Status = ETaskStatus.Waiting
            },
            new()
            {
                Id = 7,
                Name = "Task 7",
                Status = ETaskStatus.Waiting
            },
            new Task()
            {
                Id = 2,
                Name = "Task 2",
                Status = ETaskStatus.InProgress
            },
            new Task()
            {
                Id = 3,
                Name = "Task 4",
                Status = ETaskStatus.Completed
            },
            new Task()
            {
                Id = 5,
                Name = "Task 5",
                Status = ETaskStatus.Completed
            },
            new Task()
            {
                Id = 6,
                Name = "Task 6",
                Status = ETaskStatus.Completed
            }
        };
    }
}
