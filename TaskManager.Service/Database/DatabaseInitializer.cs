using Microsoft.EntityFrameworkCore;
using TaskManager.Service.Database.Models;
using TaskStatus = TaskManager.Service.Database.Models.TaskStatus;

namespace TaskManager.Service.Database;

public static class DatabaseInitializer
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        using var context = serviceProvider.GetRequiredService<TaskManagerContext>();
        context.Database.Migrate();

        if (context.Spaces.Any())
        {
            return;
        }

        SeedData(context);
    }

    private static void SeedData(TaskManagerContext context)
    {
        var space = new DbSpace
        {
            Id = Guid.NewGuid().ToString(),
            Description = "New Space",
            Key = "IT",
            Name = "Information Technology"
        };
        context.Spaces.Add(space);
        
        var epicTask = new DbTask
        {
            Id = "IT-001",
            SpaceId = space.Id,
            Name = "EpicTask",
            Description = "First Epic",
            Status = TaskStatus.InProgress,
            Type = TaskType.Epic
        };
        context.Tasks.Add(epicTask);

        var storyTask = new DbTask
        {
            Id = "IT-002",
            ParentTaskId = "IT-001",
            SpaceId = space.Id,
            Name = "Task Manger Refactor",
            Description = "Create new service",
            Status = TaskStatus.InProgress,
            Type = TaskType.Story
        };
        context.Tasks.Add(storyTask);

        var task3 = new DbTask
        {
            Id = "IT-003",
            ParentTaskId = "IT-002",
            SpaceId = space.Id,
            Name = "Prepare Docker pipeline",
            Description = "We need it to run microservice architecture",
            Status = TaskStatus.Completed,
            Type = TaskType.Task
        };
        context.Tasks.Add(task3);

        var task4 = new DbTask
        {
            Id = "IT-004",
            ParentTaskId = "IT-002",
            SpaceId = space.Id,
            Name = "Create Sqlite database initial version",
            Description = "We need it to store our data somewhere",
            Status = TaskStatus.InProgress,
            Type = TaskType.Task
        };
        context.Tasks.Add(task4);

        var task5 = new DbTask
        {
            Id = "IT-005",
            ParentTaskId = "IT-002",
            SpaceId = space.Id,
            Name = "Create REST API for database operations",
            Description = "We need to expose API for future UI",
            Status = TaskStatus.Waiting,
            Type = TaskType.Task
        };
        context.Tasks.Add(task5);

        context.SaveChanges();
    }
}
