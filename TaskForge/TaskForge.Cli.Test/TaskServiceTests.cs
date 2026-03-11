using TaskForge.Cli.Services;

namespace TaskForge.Cli.Test;

public class TaskServiceTest
{
    [Fact]
    public void Add_FirstTask_GetsIdOne()
    {
        // Arrange
        var service = new TaskService(new List<TaskForge.Cli.Models.TaskItem>());

        // Act
        var task = service.Add("Buy Milk");

        // Assert
        Assert.Equal(1, task.Id);
    }

    [Fact]
    public void Add_TitleIsTrimmed()
    {
        // Arrange
        var service = new TaskService(new List<TaskForge.Cli.Models.TaskItem>());

        // Act
        var task = service.Add("      Buy milk       ");

        // Assert
        Assert.Equal("Buy milk", task.Title);
    }

    [Fact]
    public void Add_WhitespaceTitle_Throws()
    {
        // Arrange
        var service = new TaskService(new List<TaskForge.Cli.Models.TaskItem>());

        // Act + Assert
        Assert.Throws<ArgumentException>(() => service.Add("   "));
    }
}
