namespace TaskManager.Core.Models;

public interface ITaskManager
{
    bool AddTask(Task task);
    
    ITask? GetTask(string name);
    ITask? GetTask(int id);

    IEnumerable<ITask> GetAllTasks();

    bool RemoveTask(int id);

    bool UpdateTask(ITask task);
}
