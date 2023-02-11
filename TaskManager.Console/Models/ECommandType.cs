namespace TaskManager.Console.Models;

internal enum ECommandType
{
    Undefined,
    AddTask,
    GetTaskById,
    GetTaskByName,
    GetAllTasks,
    RemoveTask,
    UpdateTask,
    Exit
}