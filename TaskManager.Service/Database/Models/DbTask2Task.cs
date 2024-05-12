namespace TaskManager.Service.Database.Models;

internal class DbTask2Task : IDbBase
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }

    public required string ParentId { get; set; }
    public required string ChildId { get; set; }

    public DbTask? ParentTask { get; set; }
    public DbTask? ChildTask { get; set; }
}
