using Microsoft.Extensions.Logging;

namespace TaskManager.Console.Models;

internal class Command : ICommand
{
    public Command(string commandTypeString, string[] args)
    {
        CommandTypeString = commandTypeString;
        Args = args;
    }

    public ECommandType CommandType 
    {
        get
        {
            bool parseResult = Enum.TryParse<ECommandType>(CommandTypeString, out var commandType);
            return parseResult ? commandType : ECommandType.Undefined;
        }
    }

    public string CommandTypeString { get; }

    public string[] Args { get; }
}
