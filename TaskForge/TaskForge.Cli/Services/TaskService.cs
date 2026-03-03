using TaskForge.Cli.Models;

namespace TaskForge.Cli.Services;

public class TaskService
{
    private readonly List<TaskItem> _tasks = new();
    private int _nextId = 1;

    public TaskItem Add(string title)
    {
        // TODO: validate title (no null/whitespace)
        // TODO: create TaskItem with next id
        // TODO: add to list
        // TODO: return created item
        throw new NotImplementedException();
    }

    public IReadOnlyList<TaskItem> GetAll()
    {
        return _tasks;
    }

    public bool MarkDone(int id)
    {
        // TODO: find task by id
        // TODO: if not found return false
        // TODO: mark done and return true
        throw new NotImplementedException();
    }
}