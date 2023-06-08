namespace TaskManager.Core.Models;

public interface ITask
{
    public int Id { get; }
    public string? Name { get; }
    public ETaskStatus Status { get; }
}
