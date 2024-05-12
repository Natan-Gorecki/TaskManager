namespace TaskManager.Service.Database.Models;

internal class DbTimeEntry : DbBase
{
    public required DateTime StartTime { get; set; }
    public required TimeSpan Duration { get; set; }
    public string? Description { get; set; }
    public required string TaskId { get; set; }

    public DbTask? Task { get; set; }
}
