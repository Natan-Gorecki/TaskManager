using Microsoft.EntityFrameworkCore;

namespace TaskManager.Service.Database.Models;

[PrimaryKey(nameof(TaskId), nameof(LabelId))]
internal class DbTask2Label : IDbBase
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }

    public required string TaskId { get; set; }
    public required string LabelId { get; set; }

    public DbTask? Task { get; set; }
    public DbLabel? Label { get; set; }
}
