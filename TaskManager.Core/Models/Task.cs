namespace TaskManager.Core.Models;

public class Task : ITask
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ETaskStatus? Status { get; set; }
}
