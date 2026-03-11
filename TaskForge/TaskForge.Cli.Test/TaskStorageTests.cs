using System.Text.Json;
using TaskForge.Core.Models;
using TaskForge.Core.Storage;

namespace TaskForge.Core.Tests;

public class TaskStorageTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly string _testFilePath;

    public TaskStorageTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), "TaskForgeTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_testDirectory);
        _testFilePath = Path.Combine(_testDirectory, "tasks.json");
    }

    [Fact]
    public void LoadTasks_FileDoesNotExist_ReturnsEmptyList()
    {
        // Arrange
        var storage = new TaskStorage(_testFilePath);

        // Act
        var tasks = storage.LoadTasks();

        // Assert
        Assert.Empty(tasks);
    }

    [Fact]
    public void LoadTasks_EmptyFile_ReturnsEmptyList()
    {
        // Arrange
        File.WriteAllText(_testFilePath, string.Empty);
        var storage = new TaskStorage(_testFilePath);

        // Act
        var tasks = storage.LoadTasks();

        // Assert
        Assert.Empty(tasks);
    }

    [Fact]
    public void LoadTasks_InvalidJson_ReturnsEmptyList()
    {
        // Arrange
        File.WriteAllText(_testFilePath, "{ this is not valid json }");
        var storage = new TaskStorage(_testFilePath);

        // Act
        var tasks = storage.LoadTasks();

        // Assert
        Assert.Empty(tasks);
    }

    [Fact]
    public void LoadTasks_ValidJson_ReturnsTasks()
    {
        // Arrange
        var expectedTasks = new List<TaskItem>()
        {
            new TaskItem(1, "Buy milk"),
            new TaskItem(2, "Walk dog", true)
        };

        var json = JsonSerializer.Serialize(expectedTasks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_testFilePath, json);

        var storage = new TaskStorage(_testFilePath);

        // Act
        var tasks = storage.LoadTasks();

        // Assert
        Assert.Equal(2, tasks.Count);

        Assert.Equal(1, tasks[0].Id);
        Assert.Equal("Buy milk", tasks[0].Title);
        Assert.False(tasks[0].IsDone);

        Assert.Equal(2, tasks[1].Id);
        Assert.Equal("Walk dog", tasks[1].Title);
        Assert.True(tasks[1].IsDone);
    }

    [Fact]
    public void TrySaveTasks_ValidFilePath_ReturnsTrue()
    {
        // Arrange
        var storage = new TaskStorage(_testFilePath);
        var tasks = new List<TaskItem>
        {
            new TaskItem(1, "Buy milk")
        };

        // Act
        var result = storage.TrySaveTasks(tasks, out var errorMessage);

        // Assert
        Assert.True(result);
        Assert.Equal(string.Empty, errorMessage);
    }

    [Fact]
    public void TrySaveTasks_CreatesOrOverwritesFile()
    {
        // Arrange
        File.WriteAllText(_testFilePath, "old content");

        var storage = new TaskStorage(_testFilePath);
        var tasks = new List<TaskItem>
        {
            new TaskItem(1, "Buy milk")
        };

        // Act
        var result = storage.TrySaveTasks(tasks, out var errorMessage);
        var savedJson = File.ReadAllText(_testFilePath);

        // Assert
        Assert.True(result);
        Assert.Equal(string.Empty, errorMessage);
        Assert.NotEqual("old content", savedJson);
        Assert.Contains("Buy milk", savedJson);
    }

    [Fact]
    public void TrySaveTasks_SaveThenLoad_RoundTripsTaskData()
    {
        // Arrange
        var originalTasks = new List<TaskItem>
        {
            new TaskItem(1, "Buy milk"),
            new TaskItem(2, "Walk dog", true)
        };

        var storage = new TaskStorage(_testFilePath);

        // Act
        var saveResult = storage.TrySaveTasks(originalTasks, out var errorMessage);
        var loadedTasks = storage.LoadTasks();

        // Assert
        Assert.True(saveResult);
        Assert.Equal(string.Empty, errorMessage);

        Assert.Equal(originalTasks.Count, loadedTasks.Count);

        Assert.Equal(originalTasks[0].Id, loadedTasks[0].Id);
        Assert.Equal(originalTasks[0].Title, loadedTasks[0].Title);
        Assert.Equal(originalTasks[0].IsDone, loadedTasks[0].IsDone);

        Assert.Equal(originalTasks[1].Id, loadedTasks[1].Id);
        Assert.Equal(originalTasks[1].Title, loadedTasks[1].Title);
        Assert.Equal(originalTasks[1].IsDone, loadedTasks[1].IsDone);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, recursive: true);
        }
    }
}

