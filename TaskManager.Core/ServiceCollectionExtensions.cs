using Microsoft.Extensions.DependencyInjection;
using TaskManager.Core.Models;
using TaskManager.Core.JsonFile;

namespace TasksManager.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTasksManager(this IServiceCollection serviceColletion, ETaskManagerType managerType = ETaskManagerType.JsonFile)
    {
        switch (managerType)
        {
            case ETaskManagerType.JsonFile:
                return serviceColletion.AddSingleton<ITaskManager, TaskManager_JsonFile>();
            case ETaskManagerType.SqlLite:
            default:
                throw new NotImplementedException($"TaskManager {managerType} type is not implemented");
        }
    }
}
