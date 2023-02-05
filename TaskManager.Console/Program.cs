using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TasksManager.Core;

var serviceProvider = new ServiceCollection()
    .AddTasksManager()
    .AddLogging((loggingBuilder) =>
    {
        loggingBuilder.AddConsole();
    })
    .BuildServiceProvider();

Console.WriteLine("Hello, World!");