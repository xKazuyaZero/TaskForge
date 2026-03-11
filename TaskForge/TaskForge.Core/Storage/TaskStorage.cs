using System.Text.Json;
using TaskForge.Core.Models;

namespace TaskForge.Core.Storage;

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
            Console.WriteLine("Warning: tasks.json could not be read. Starting with an empty task list.");
            return new List<TaskItem>();
        }
    }

    public bool TrySaveTasks(IReadOnlyList<TaskItem> tasks, out string errorMessage)
    {
        try
        {
            string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
            errorMessage = string.Empty;
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            errorMessage = "Could not save tasks: access denied.";
            return false;
        }
        catch (IOException)
        {
            errorMessage = "Could not save tasks: file write failed.";
            return false;
        }
    }
}