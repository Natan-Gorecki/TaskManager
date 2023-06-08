using Microsoft.Extensions.DependencyInjection;
using TaskManager.Console.Models;

namespace TaskManager.Console;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleManager(this IServiceCollection serviceColletion)
    {
        return serviceColletion.AddSingleton<IConsoleManager, ConsoleManager>();
    }
}
