namespace TaskManager.Service.Database.Models;

internal class DbTask : DbBase
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required TaskStatus Status { get; set; }
    public required TaskType Type { get; set; }
}
