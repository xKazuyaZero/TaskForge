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
        if (!File.Exists(_filePath))
            return new List<TaskItem>();

        try
        {
            string json = File.ReadAllText(_filePath);

            if (string.IsNullOrWhiteSpace(json))
                return new List<TaskItem>();

            return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }
        catch (JsonException)
        {
            Console.WriteLine("Warning: tasks.json is invalid. Starting with an empty task list.");
            return new List<TaskItem>();
        }
        catch (IOException)
        {
            Console.WriteLine("Warning: tasks.json could not be read. Starting with an empty tasks list.");
            return new List<TaskItem>();
        }
    }

    public void SaveTasks(IReadOnlyList<TaskItem> tasks)
    {
        string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}