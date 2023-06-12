using Microsoft.Extensions.DependencyInjection;
using TaskManager.Core;
using TaskManager.Core.JsonFile;
using TaskManager.Core.Models;
using TaskManager.Core.SqlLite;

namespace TasksManager.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTasksManager(this IServiceCollection serviceColletion, TaskManagerType type)
    {
        return type switch
        {
            TaskManagerType.JsonFile => serviceColletion.AddSingleton<ITaskManager, TaskManager_JsonFile>(),
            TaskManagerType.SqlLite => serviceColletion.AddSingleton<ITaskManager, TaskManager_SqlLite>(),
            _ => throw new NotImplementedException($"TaskManager {type} type is not implemented.")
        };
    }
}
