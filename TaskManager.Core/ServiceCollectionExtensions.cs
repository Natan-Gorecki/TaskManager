using Microsoft.Extensions.DependencyInjection;
using TaskManager.Core.JsonFile;
using TaskManager.Core.Models;

namespace TasksManager.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTasksManager(this IServiceCollection serviceColletion, ETaskManagerType managerType = ETaskManagerType.JsonFile)
    {
        return managerType switch
        {
            ETaskManagerType.JsonFile => serviceColletion.AddSingleton<ITaskManager, TaskManager_JsonFile>(),
            _ => throw new NotImplementedException($"TaskManager {managerType} type is not implemented"),
        };
    }
}
