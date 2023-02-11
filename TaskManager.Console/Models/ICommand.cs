namespace TaskManager.Console.Models;

internal interface ICommand
{
    ECommandType CommandType { get; }
    string CommandTypeString { get; }
    string[] Args { get; }
}
