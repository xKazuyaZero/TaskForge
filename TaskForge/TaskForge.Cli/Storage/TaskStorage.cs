using System.Text.Json;
using TaskForge.Cli.Models;

namespace TaskForge.Cli.Storage;

public class TaskStorage
{
    private readonly string _filePath;

    public TaskStorage(string filePath)
    {
        _filePath = filePath;
    }

    public List<TaskItem> LoadTasks()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);

            if (!string.IsNullOrWhiteSpace(json))
            {
                return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
            }
        }
        return new List<TaskItem>();
    }

    public void SaveTasks(IReadOnlyList<TaskItem> tasks)
    {
        string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}