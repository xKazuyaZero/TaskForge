using System.Text.Json.Serialization;

namespace TaskForge.Core.Models;

public class TaskItem
{
    public int Id { get; }
    public string Title { get; }
    public bool IsDone { get; private set; }

    [JsonConstructor]
    public TaskItem(int id, string title, bool isDone = false)
    {
        Id = id;
        Title = title;
        IsDone = isDone;
    }

    public void MarkDone()
    {
        IsDone = true;
    }
}