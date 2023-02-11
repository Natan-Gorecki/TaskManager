namespace TaskManager.Console.Models;

internal interface IConsoleManager
{
    void DisplayAvailableCommands();
    ICommand ReadCommandFromInput();
    bool ValidateArguments(ICommand command);
    bool ExecuteCommand(ICommand command);
}
