namespace TaskManager.Service.Database.Models;

internal class DbSpace : DbBase
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Key { get; set; }
}
