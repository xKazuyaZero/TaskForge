using TaskForge.Core.Models;

namespace TaskForge.Core.Services;

public class TaskService
{
    private readonly List<TaskItem> _tasks = new();
    private int _nextId = 1;

    public TaskService(List<TaskItem> loadedTasks)
    {
        _tasks = loadedTasks ?? new List<TaskItem>();
        _nextId = _tasks.Count > 0 ? _tasks.Max(t => t.Id) + 1 : 1;
    }

    public TaskItem Add(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("<title> cannot be empty.", nameof(title));

        var cleaned = title.Trim();
        var item = new TaskItem(_nextId++, cleaned);
        _tasks.Add(item);
        return item;
    }

    public IReadOnlyList<TaskItem> GetAll()
    {
        return _tasks.AsReadOnly();
    }

    public bool MarkDone(int id)
    {
        var item = _tasks.FirstOrDefault(t => t.Id == id);
        if (item is null)
            return false; 

        item.MarkDone();
        return true;
    }
}