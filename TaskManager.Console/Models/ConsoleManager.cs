using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using TaskManager.Core.Models;

namespace TaskManager.Console.Models;

internal partial class ConsoleManager : IConsoleManager
{
    private readonly ITaskManager _taskManager;
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public ConsoleManager(ITaskManager taskManager)
    {
        _taskManager = taskManager;
    }

    public void DisplayAvailableCommands()
    {
        System.Console.WriteLine(@"Avaible commands:
    - AddTask Id Name Status
    - GetTaskById Id
    - GetTaskByName Name
    - GetAllTasks
    - RemoveTask Id
    - UpdateTask Id Name Status
    - Exit");
    }

    public ICommand ReadCommandFromInput()
    {
        string? commandLine = System.Console.ReadLine();
        ArgumentNullException.ThrowIfNull(commandLine);

        var commandLineArgs = Regex_CommandLineArgs()
            .Matches(commandLine)
            .Cast<Match>()
            .Select(x => x.Value.Trim('"'))
            .ToList();
        
        ICommand command = new Command(commandLineArgs[0], commandLineArgs.Skip(1).ToArray());
        return command;
    }

    public bool ValidateArguments(ICommand command)
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));

        switch (command.CommandType)
        {
            case ECommandType.AddTask:
                {
                    if (command?.Args?.Length != 3)
                    {
                        System.Console.WriteLine($"Invalid arguments count for \"{command!.CommandTypeString}\" command.");
                        return false;
                    }

                    bool parseResult = int.TryParse(command.Args[0], out int intValue);
                    if (!parseResult)
                    {
                        System.Console.WriteLine($"{command.Args[0]} is not integer.");
                        return false;
                    }

                    if (string.IsNullOrWhiteSpace(command.Args[1]))
                    {
                        System.Console.WriteLine($"{command.Args[1]} does not have value.");
                        return false;
                    }

                    parseResult = Enum.TryParse<ETaskStatus>(command.Args[2], out ETaskStatus taskValue);
                    if (!parseResult)
                    {
                        System.Console.WriteLine($"{command.Args[2]} is not {nameof(ETaskStatus)}");
                        return false;
                    }

                    return true;
                }
            case ECommandType.GetTaskById:
                {
                    if (command?.Args?.Length != 1)
                    {
                        System.Console.WriteLine($"Invalid arguments count for \"{command!.CommandTypeString}\" command.");
                        return false;
                    }

                    bool parseResult = int.TryParse(command.Args[0], out int intValue);
                    if (!parseResult)
                    {
                        System.Console.WriteLine($"{command.Args[0]} is not integer.");
                        return false;
                    }

                    return true;
                }
            case ECommandType.GetTaskByName:
                {
                    if (command?.Args?.Length != 1)
                    {
                        System.Console.WriteLine($"Invalid arguments count for \"{command!.CommandTypeString}\" command.");
                        return false;
                    }

                    if (string.IsNullOrWhiteSpace(command.Args[0]))
                    {
                        System.Console.WriteLine($"{command.Args[0]} does not have value.");
                        return false;
                    }
                    
                    return true;
                }
            case ECommandType.GetAllTasks:
                {
                    return true;
                }
            case ECommandType.RemoveTask:
                {
                    if (command?.Args?.Length != 1)
                    {
                        System.Console.WriteLine($"Invalid arguments count for \"{command!.CommandTypeString}\" command.");
                        return false;
                    }

                    bool parseResult = int.TryParse(command.Args[0], out int intValue);
                    if (!parseResult)
                    {
                        System.Console.WriteLine($"{command.Args[0]} is not integer.");
                        return false;
                    }
                    
                    return true;
                }
            case ECommandType.UpdateTask:
                {
                    if (command?.Args?.Length != 3)
                    {
                        System.Console.WriteLine($"Invalid arguments count for \"{command!.CommandTypeString}\" command.");                        
                        return false;
                    }

                    bool parseResult = int.TryParse(command.Args[0], out int intValue);
                    if (!parseResult)
                    {
                        System.Console.WriteLine($"{command.Args[0]} is not integer.");
                        return false;
                    }

                    if (string.IsNullOrWhiteSpace(command.Args[1]))
                    {
                        System.Console.WriteLine($"{command.Args[1]} does not have value.");
                        return false;
                    }

                    parseResult = Enum.TryParse<ETaskStatus>(command.Args[2], out ETaskStatus taskValue);
                    if (!parseResult)
                    {
                        System.Console.WriteLine($"{command.Args[2]} is not {nameof(ETaskStatus)}");
                        return false;
                    }

                    return true;
                }
            case ECommandType.Exit:
                {
                    return true;
                }
            case ECommandType.Undefined:
            default:
                {
                    System.Console.WriteLine($"Undefined \"{command.CommandTypeString}\" command.");
                    return false;
                };
        }
    }

    public bool ExecuteCommand(ICommand command)
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));

        switch (command.CommandType)
        {
            case ECommandType.AddTask:
                {
                    bool result = _taskManager.AddTask(new()
                    {
                        Id = int.Parse(command.Args[0]),
                        Name = command.Args[1],
                        Status = Enum.Parse<ETaskStatus>(command.Args[2])
                    });

                    if (!result)
                    {
                        System.Console.WriteLine($"Failed to add new task with id {int.Parse(command.Args[0])}.");
                    }
                    else
                    {
                        System.Console.WriteLine($"Task with id {int.Parse(command.Args[0])} added successfully.");
                    }

                    return result;
                }
            case ECommandType.GetTaskById:
                {
                    int taskId = int.Parse(command.Args[0]);
                    var task = _taskManager.GetTask(taskId);

                    if (task is null)
                    {
                        System.Console.WriteLine($"Failed to get task with id {taskId}.");
                        return false;
                    }
                    else
                    {
                        System.Console.WriteLine($"Task with id {taskId}:{Environment.NewLine}{JsonSerializer.Serialize(task, _serializerOptions)}");
                        return true;
                    }
                }
            case ECommandType.GetTaskByName:
                {
                    string taskName = command.Args[0];
                    var task = _taskManager.GetTask(taskName);

                    if (task is null)
                    {
                        System.Console.WriteLine($"Failed to get task with name \"{taskName}\".");
                        return false;
                    }
                    else
                    {
                        System.Console.WriteLine($"Task with name {taskName}:{Environment.NewLine}{JsonSerializer.Serialize(task, _serializerOptions)}");
                        return true;
                    }
                }
            case ECommandType.GetAllTasks:
                {
                    var tasks = _taskManager.GetAllTasks();
                    System.Console.WriteLine($"All stored tasks:{Environment.NewLine}{JsonSerializer.Serialize(tasks, _serializerOptions)}");
                    return true;
                }
            case ECommandType.RemoveTask:
                {
                    int taskId = int.Parse(command.Args[0]);
                    bool result = _taskManager.RemoveTask(taskId);

                    if (!result)
                    {
                        System.Console.WriteLine($"Failed to remove task with id {taskId}.");
                    }
                    else
                    {
                        System.Console.WriteLine($"Task with id {taskId} removed successfully.");
                    }

                    return result;
                }
            case ECommandType.UpdateTask:
                {
                    int taskId = int.Parse(command.Args[0]);
                    bool result = _taskManager.UpdateTask(new Core.Models.Task()
                    {
                        Id = taskId,
                        Name = command.Args[1],
                        Status = Enum.Parse<ETaskStatus>(command.Args[2])
                    });

                    if (!result)
                    {
                        System.Console.WriteLine($"Failed to update task with id {taskId}.");
                    }
                    else
                    {
                        System.Console.WriteLine($"Task with id {taskId} updated successfully.");
                    }

                    return result;
                }
            case ECommandType.Undefined:
            default:
                {
                    System.Console.WriteLine($"Undefined \"{command.CommandTypeString}\" command.");
                    return false;
                }
        }
    }

    [GeneratedRegex(@"[\""].+?[\""]|[^ ]+")]
    private static partial Regex Regex_CommandLineArgs();
}