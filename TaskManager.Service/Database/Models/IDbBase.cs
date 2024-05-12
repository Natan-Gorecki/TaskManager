namespace TaskManager.Service.Database.Models;

public interface IDbBase
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}
