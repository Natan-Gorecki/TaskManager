using Microsoft.EntityFrameworkCore;
using TaskManager.Service.Database.Models;

namespace TaskManager.Service.Database;

public class TaskManagerContext : DbContext
{
    internal DbSet<DbLabel> Labels { get; set; }
    internal DbSet<DbSpace> Spaces { get; set; }
    internal DbSet<DbTask> Tasks { get; set; }
    internal DbSet<DbTask2Label> Task2Labels { get; set; }
    internal DbSet<DbTask2TaskJoin> Task2TaskJoins { get; set; }
    internal DbSet<DbTimeEntry> TimeEntries { get; set; }

    public override int SaveChanges()
    {
        var utcNow = DateTime.UtcNow;

        foreach (var changedEntity in ChangeTracker.Entries())
        {
            if (changedEntity.Entity is IDbBase entity)
            {
                switch (changedEntity.State)
                {
                    case EntityState.Added:
                        entity.CreatedAt = utcNow;
                        entity.ModifiedAt = utcNow;
                        break;

                    case EntityState.Modified:
                        Entry(entity).Property(x => x.CreatedAt).IsModified = false;
                        entity.ModifiedAt = utcNow;
                        break;
                }
            }
        }

        return base.SaveChanges();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Enum>().HaveConversion<string>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbTask>()
            .HasMany(x => x.ChildTasks)
            .WithMany(x => x.ParentTasks)
            .UsingEntity<DbTask2TaskJoin>(
                left => left.HasOne<DbTask>().WithMany().HasForeignKey(x => x.ChildId),
                right => right.HasOne<DbTask>().WithMany().HasForeignKey(x => x.ParentId)
            );
    }
}
