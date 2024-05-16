using Ardalis.GuardClauses;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Service.Database.Sqlite;

public class SqliteTaskManagerContext : TaskManagerContext
{
    private const string DatabaseName = "Tasks.db";
    private const string SecretsFile = "Secrets.txt";
    private readonly IConfiguration _configuration;

    public SqliteTaskManagerContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dataFolder = EnsureDataFolder();

        var connectionBuilder = new SqliteConnectionStringBuilder
        {
            DataSource = Path.Combine(dataFolder, DatabaseName),
            Password = BuildPassword(dataFolder)
        };
        var sqliteConnection = new SqliteConnection(connectionBuilder.ToString());

        optionsBuilder.UseSqlite(sqliteConnection);
    }

    private string EnsureDataFolder()
    {
        var dataFolder = _configuration.GetSection("DataFolder").Value;
        Guard.Against.NullOrWhiteSpace(dataFolder, nameof(dataFolder));

        if (!Directory.Exists(dataFolder))
        {
            Directory.CreateDirectory(dataFolder);
        }

        return dataFolder;
    }

    private static string BuildPassword(string dataFolder)
    {
        var secretsFile = Path.Combine(dataFolder, SecretsFile);

        if (File.Exists(secretsFile))
        {
            return File.ReadAllText(secretsFile);
        }

        var password = Guid.NewGuid().ToString();
        File.WriteAllText(secretsFile, password);
        return password;
    }
}
