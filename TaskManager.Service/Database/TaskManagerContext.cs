using Microsoft.EntityFrameworkCore;
using TaskManager.Service.Database.Models;

namespace TaskManager.Service.Database;

public class TaskManagerContext : DbContext
{
    internal DbSet<DbLabel> Labels { get; set; }
    internal DbSet<DbSpace> Spaces { get; set; }
    internal DbSet<DbTask> Tasks { get; set; }
    internal DbSet<DbTimeEntry> TimeEntries { get; set; }

    public override int SaveChanges()
    {
        var utcNow = DateTime.UtcNow;

        foreach (var changedEntity in ChangeTracker.Entries())
        {
            if (changedEntity.Entity is DbBase entity)
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
            .HasMany(x => x.Labels)
            .WithMany(x => x.Tasks)
            .UsingEntity("Task2LabelJoins");
    }
}
