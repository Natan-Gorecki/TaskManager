namespace TaskManager.Service.Database.Models;

internal class DbLabel : DbBase
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string SpaceId { get; set; }

    public DbSpace? Space { get; set; }
}
