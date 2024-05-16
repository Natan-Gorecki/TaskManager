namespace TaskManager.Service.Database.Models;

internal class DbTask : DbBase
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? ParentTaskId { get; set; }
    public required string SpaceId { get; set; }
    public required TaskStatus Status { get; set; }
    public required TaskType Type { get; set; }

    public List<DbTask> ChildTasks { get; set; } = new();
    public List<DbLabel> Labels { get; set; } = new();
    public DbTask? ParentTask { get; set; }
    public DbSpace? Space { get; set; }
}
