using TaskForge.Core.Models;

namespace TaskForge.Core.Storage;

public interface ITaskStorage
{
    List<TaskItem> LoadTasks(out string? warningMessage);
    bool TrySaveTasks(IReadOnlyList<TaskItem> tasks, out string errorMessage);
}