using Microsoft.EntityFrameworkCore;

namespace TaskManager.Service.Database.Models;

[PrimaryKey(nameof(ParentId), nameof(ChildId))]
internal class DbTask2TaskJoin : IDbBase
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }

    public required string ParentId { get; set; }
    public required string ChildId { get; set; }
}
