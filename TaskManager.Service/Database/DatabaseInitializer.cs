using Ardalis.GuardClauses;
using Microsoft.Data.Sqlite;

namespace TaskManager.Service.Database;

public static class DatabaseInitializer
{
    private const string DatabaseName = "Tasks.db";
    private const string SecretsFile = "Secrets.json";

    public static void AddTaskManagerContext(this IServiceCollection serviceCollection, ConfigurationManager configurationManager)
    {
        var dataFolder = configurationManager.GetSection("DataFolder").Value;
        Guard.Against.NullOrWhiteSpace(dataFolder, nameof(dataFolder));

        var builder = new SqliteConnectionStringBuilder
        {
            DataSource = Path.Combine(dataFolder, DatabaseName),
            Password = BuildPassword(dataFolder)
        };
        serviceCollection.AddSqlite<TaskManagerContext>(builder.ConnectionString);
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
