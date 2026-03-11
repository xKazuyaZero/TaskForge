using TaskForge.Cli.Services;
using TaskForge.Cli.Storage;

var tasksFilePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "tasks.json"));

var storage = new TaskStorage(tasksFilePath);
var loadedTasks = storage.LoadTasks();
var service = new TaskService(loadedTasks);

Console.WriteLine("TaskForge v0.1");
Console.WriteLine("Type 'help' for commands.");

while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input)) continue;

    var parts = input.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
    var command = parts[0].ToLowerInvariant();
    var arg = parts.Length > 1 ? parts[1] : "";

    switch (command)
    {
        case "help":
            Console.WriteLine("Commands:");
            Console.WriteLine(" add <title>     Adds a task");
            Console.WriteLine(" list            Lists tasks");
            Console.WriteLine(" done <id>       Marks task done");
            Console.WriteLine(" exit            Quits");
            break;

        case "add":
            try
            {
                var created = service.Add(arg);
                storage.SaveTasks(service.GetAll());
                Console.WriteLine($"Added: [ ] {created.Id}: {created.Title}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            break;

        case "list":
            var tasks = service.GetAll();

            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks yet.");
                break;
            }

            foreach (var t in tasks)
            {
                var mark = t.IsDone ? "x" : " ";
                Console.WriteLine($"[{mark}] {t.Id}: {t.Title}");
            }
            break;

        case "done":
            if (!int.TryParse(arg, out var id))
            {
                Console.WriteLine("Invalid id. Example: done 1");
                break;
            }

            var ok = service.MarkDone(id);

            if (ok)
            {
                storage.SaveTasks(service.GetAll());
                Console.WriteLine($"Marked done: {id}");
            }
            else
            {
                Console.WriteLine($"Task not found: {id}");
            }
            break;

        case "exit":
            return;

        default:
            Console.WriteLine("Unknown command. Type 'help'.");
            break;
    }
    
}