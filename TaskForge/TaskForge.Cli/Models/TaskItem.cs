namespace TaskForge.Cli.Models;

public class TaskItem
{
    public int Id { get; }
    public string Title { get; }
    public bool IsDone { get; private set; }

    public TaskItem(int id, string title)
    {
        Id = id;
        Title = title;
    }

    public void MarkDone()
    {
        IsDone = true;
    }
}