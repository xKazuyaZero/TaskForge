using TaskForge.Cli.Models;

namespace TaskForge.Cli.Services;

public class TaskService
{
    private readonly List<TaskItem> _tasks = new();
    private int _nextId = 1;

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
        return _tasks;
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