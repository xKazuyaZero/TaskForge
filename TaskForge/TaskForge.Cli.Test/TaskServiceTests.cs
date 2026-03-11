using TaskForge.Core.Services;

namespace TaskForge.Core.Test;

public class TaskServiceTest
{
    [Fact]
    public void Add_FirstTask_GetsIdOne()
    {
        // Arrange
        var service = new TaskService(new List<TaskForge.Core.Models.TaskItem>());

        // Act
        var task = service.Add("Buy Milk");

        // Assert
        Assert.Equal(1, task.Id);
    }

    [Fact]
    public void Add_TitleIsTrimmed()
    {
        // Arrange
        var service = new TaskService(new List<TaskForge.Core.Models.TaskItem>());

        // Act
        var task = service.Add("      Buy milk       ");

        // Assert
        Assert.Equal("Buy milk", task.Title);
    }

    [Fact]
    public void Add_WhitespaceTitle_Throws()
    {
        // Arrange
        var service = new TaskService(new List<TaskForge.Core.Models.TaskItem>());

        // Act + Assert
        Assert.Throws<ArgumentException>(() => service.Add("   "));
    }

    [Fact]
    public void MarkDone_TaskReturnsTrue()
    {
        // Arrange
        var service = new TaskService(new List<TaskForge.Core.Models.TaskItem>());
        var task = service.Add("Buy milk");

        // Act
        var result = service.MarkDone(task.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void MarkDone_TaskBecomesDone()
    {
        // Arrange
        var service = new TaskService(new List<TaskForge.Core.Models.TaskItem>());
        var task = service.Add("Buy milk");

        // Act
        service.MarkDone(task.Id);

        // Assert
        Assert.True(task.IsDone);
    }

    [Fact]
    public void MarkDone_MissingTaskReturnsFalse()
    {
        // Arrange
        var service = new TaskService(new List<TaskForge.Core.Models.TaskItem>());

        // Act
        var result = service.MarkDone(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Add_PreloadedTasksUsesMaxIdPlusOne()
    {
        // Arrange
        var loadedTasks = new List<TaskForge.Core.Models.TaskItem>
        {
            new TaskForge.Core.Models.TaskItem(1, "Task 1"),
            new TaskForge.Core.Models.TaskItem(2, "Task 2"),
            new TaskForge.Core.Models.TaskItem(5, "Task 3")
        };

        var service = new TaskService(loadedTasks);

        // Act
        var newTask = service.Add("New Task");

        // Assert
        Assert.Equal(6, newTask.Id);
    }

    [Fact]
    public void Delete_ExistingTaskReturnsTrue()
    {
        // Arrange
        var service = new TaskService(new List<TaskForge.Core.Models.TaskItem>());
        var task = service.Add("Buy milk");

        // Act 
        var result = service.Delete(task.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Delete_ExistingTaskRemovesItFromList()
    {
        // Arrange
        var service = new TaskService(new List<TaskForge.Core.Models.TaskItem>());
        var task = service.Add("Buy milk");

        // Act
        service.Delete(task.Id);

        // Assert
        Assert.Empty(service.GetAll());
    }

    [Fact]
    public void Delete_MissingTaskReturnsFalse()
    {
        // Arrange
        var service = new TaskService(new List<TaskForge.Core.Models.TaskItem>());

        // Act
        var result = service.Delete(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Delete_OneTaskDoesNotAffectOthers()
    {
        // Arrange
        var service = new TaskService(new List<TaskForge.Core.Models.TaskItem>());
        var first = service.Add("Buy milk");
        var second = service.Add("Walk dog");

        // Act
        service.Delete(first.Id);

        // Assert
        var tasks = service.GetAll();
        Assert.Single(tasks);
        Assert.Equal(second.Id, tasks[0].Id);
        Assert.Equal("Walk dog", tasks[0].Title);
    }
}
