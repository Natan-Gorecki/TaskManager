using Microsoft.Extensions.Logging;
using System.Text.Json;
using TaskManager.Core.Extensions;
using TaskManager.Core.Models;

namespace TaskManager.Core.JsonFile;

internal class TaskManager_JsonFile : ITaskManager
{
    private readonly ILogger _logger;
    private List<Models.Task> _tasks = new();
    
    private string FilePath { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "tasks.json");

    public TaskManager_JsonFile(ILogger<TaskManager_JsonFile> logger)
    {
        _logger = logger;

        Initialize();
    }

    public bool AddTask(Models.Task task)
    {
        ArgumentNullException.ThrowIfNull(task);

        if(_tasks.Any(t => t.Id == task.Id))
        {
            _logger.LogWarning($"Task with id {task.Id} already exist.");
            return false;
        }

        _tasks.Add(task);
        SaveToFile();
        return true;
    }

    public IEnumerable<ITask> GetAllTasks()
    {
        return _tasks;
    }

    public ITask? GetTask(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        return _tasks.FirstOrDefault(t => t.Name == name);
    }

    public ITask? GetTask(int id)
    {
        return _tasks.FirstOrDefault(t => t.Id == id);
    }

    public bool RemoveTask(int id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if(task is null)
        {
            _logger.LogWarning($"Task with id {id} doesn't exist.");
            return false;
        }

        _tasks.Remove(task);
        SaveToFile();
        return true;
    }

    public bool UpdateTask(ITask task)
    {
        ArgumentNullException.ThrowIfNull(task);

        var storedTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
        if(storedTask is null)
        {
            _logger.LogWarning($"Task with id {task.Id} doesn't exist.");
            return false;
        }

        storedTask.CopyFrom(task);
        SaveToFile();
        return true;
    }

    private void Initialize()
    {
        string dataDirectory = Path.GetDirectoryName(FilePath)!;
        if (!Directory.Exists(dataDirectory))
        {
            _logger.LogDebug($"Directory doesn't exist. Creating directory {dataDirectory}.");
            Directory.CreateDirectory(dataDirectory);
        }

        if (!File.Exists(FilePath))
        {
            _logger.LogDebug($"File doesn't exist. Creating {FilePath}.");
            File.Create(FilePath);
        }

        FileInfo fileInfo = new(FilePath);
        if(fileInfo.Length == 0) 
        {
            _logger.LogDebug($"File {FilePath} is empty. Skipping to parse content.");
        }
        else
        {
            ReadFromFile();
        }   
    }

    private void SaveToFile()
    {
        _logger.LogTrace($"Saving tasks to file {FilePath}.");

        _tasks = _tasks.OrderBy(t => t.Id).ToList();
        string fileContent = JsonSerializer.Serialize(_tasks);
        File.WriteAllText(FilePath, fileContent);
    }

    private void ReadFromFile()
    {
        string fileContent = File.ReadAllText(FilePath);
        _tasks = JsonSerializer.Deserialize<List<Models.Task>>(fileContent) ?? new();
        _logger.LogInformation($"Loaded {_tasks.Count} tasks from file");
    }
}
