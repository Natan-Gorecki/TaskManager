using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TaskManager.Console;
using TaskManager.Console.Models;
using TasksManager.Core;

var serviceProvider = new ServiceCollection()
    .AddTasksManager()
    .AddConsoleManager()
    .AddLogging((loggingBuilder) =>
    {
        loggingBuilder.ClearProviders();
    })
    .BuildServiceProvider();

IConsoleManager consoleManager = serviceProvider.GetRequiredService<IConsoleManager>();

while (true)
{
    consoleManager.DisplayAvailableCommands();
    
    ICommand command = consoleManager.ReadCommandFromInput();
    Console.Clear();

    bool validationResult = consoleManager.ValidateArguments(command);
    if (!validationResult)
    {
        continue;
    }

    if(command.CommandType == ECommandType.Exit)
    {
        Console.WriteLine("Exiting application...");
        break;
    }

    bool executionResult = consoleManager.ExecuteCommand(command);
}