using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Service.Database.Models;

[PrimaryKey(nameof(ParentId), nameof(ChildId))]
internal class DbTask2Task : IDbBase
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }

    public required string ParentId { get; set; }
    public required string ChildId { get; set; }

    [ForeignKey(nameof(ParentId))]
    [InverseProperty(nameof(DbTask.ChildTaskRelations))]
    public DbTask? ParentTask { get; set; }

    [ForeignKey(nameof(ChildId))]
    [InverseProperty(nameof(DbTask.ParentTaskRelations))]
    public DbTask? ChildTask { get; set; }
}
