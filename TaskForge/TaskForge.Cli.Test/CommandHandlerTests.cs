using TaskForge.Cli;
using TaskForge.Core.Models;
using TaskForge.Core.Services;
using TaskForge.Core.Storage;

namespace TaskForge.Core.Tests;

public class CommandHandlerTests
{
    [Fact]
    public void Handle_HelpCommand_PrintsCommandList()
    {
        // Arrange
        var service = new TaskService(new List<TaskItem>());
        var storage = new FakeTaskStorage();
        var output = new StringWriter();
        var handler = new CommandHandler(service, storage, output);

        // Act
        var result = handler.Handle("help");
        var text = output.ToString();

        // Assert
        Assert.True(result);
        Assert.Contains("Commands:", text);
        Assert.Contains("add <title>", text);
        Assert.Contains("list", text);
        Assert.Contains("done <id>", text);
        Assert.Contains("delete <id>", text);
        Assert.Contains("exit", text);
    }

    [Fact]
    public void Handle_DoneWithInvalidId_PrintsInvalidIdMessage()
    {
        // Arrange
        var service = new TaskService(new List<TaskItem>());
        var storage = new FakeTaskStorage();
        var output = new StringWriter();
        var handler = new CommandHandler(service, storage, output);

        // Act
        var result = handler.Handle("done abc");
        var text = output.ToString();

        // Assert
        Assert.True(result);
        Assert.Contains("Invalid id. Example: done 1", text);
    }

    [Fact]
    public void Handle_DeleteMissingTask_PrintsTaskNotFoundMessage()
    {
        // Arrange
        var service = new TaskService(new List<TaskItem>());
        var storage = new FakeTaskStorage();
        var output = new StringWriter();
        var handler = new CommandHandler(service, storage, output);

        // Act
        var result = handler.Handle("delete 999");
        var text = output.ToString();

        // Assert
        Assert.True(result);
        Assert.Contains("Task not found: 999", text);
    }

    [Fact]
    public void Handle_ExitCommand_ReturnsFalse()
    {
        // Arrange
        var service = new TaskService(new List<TaskItem>());
        var storage = new FakeTaskStorage();
        var output = new StringWriter();
        var handler = new CommandHandler(service, storage, output);

        // Act
        var result = handler.Handle("exit");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Handle_AddCommand_PrintsSuccessMessage()
    {
        // Arrange
        var service = new TaskService(new List<TaskItem>());
        var storage = new FakeTaskStorage();
        var output = new StringWriter();
        var handler = new CommandHandler(service, storage, output);

        // Act
        var result = handler.Handle("add Buy milk");
        var text = output.ToString();

        // Assert
        Assert.True(result);
        Assert.Contains("Added: [ ] 1: Buy milk", text);
        Assert.True(storage.TrySaveTasksWasCalled);
    }

    private class FakeTaskStorage : ITaskStorage
    {
        public bool TrySaveTasksWasCalled { get; private set; }
        public bool SaveResult { get; set; } = true;
        public string SaveErrorMessage { get; set; } = string.Empty;

        public List<TaskItem> LoadTasks(out string? warningMessage)
        {
            warningMessage = null;
            return new List<TaskItem>();
        }

        public bool TrySaveTasks(IReadOnlyList<TaskItem> tasks, out string errorMessage)
        {
            TrySaveTasksWasCalled = true;
            errorMessage = SaveErrorMessage;
            return SaveResult;
        }
    }
}