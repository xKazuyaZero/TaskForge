using TaskForge.Cli;
using TaskForge.Cli.Services;
using TaskForge.Cli.Storage;

var tasksFilePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "tasks.json"));

var storage = new TaskStorage(tasksFilePath);
var loadedTasks = storage.LoadTasks();
var service = new TaskService(loadedTasks);
var handler = new CommandHandler(service, storage);

Console.WriteLine("TaskForge v0.1");
Console.WriteLine("Type 'help' for commands.");

while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
    {
        continue;
    }

    var shouldContinue = handler.Handle(input);

    if (!shouldContinue)
    {
        break;
    }
}