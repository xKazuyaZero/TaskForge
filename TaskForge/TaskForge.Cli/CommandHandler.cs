using TaskForge.Core.Services;
using TaskForge.Core.Storage;

namespace TaskForge.Cli;

public class CommandHandler
{
    private readonly TaskService _service;
    private readonly TaskStorage _storage;

    public CommandHandler(TaskService service, TaskStorage storage)
    {
        _service = service;
        _storage = storage;
    }

    public bool Handle(string input)
    {
        var parts = input.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        var command = parts[0].ToLowerInvariant();
        var arg = parts.Length > 1 ? parts[1] : "";

        switch (command)
        {
            case "help":
                PrintHelp();
                return true;

            case "add":
                HandleAdd(arg);
                return true;

            case "list":
                HandleList();
                return true;

            case "done":
                HandleDone(arg);
                return true;

            case "exit":
                return false;

            default:
                Console.WriteLine("Unknown command. Type 'help'.");
                return true;
        }
    }

    private void PrintHelp()
    {
        Console.WriteLine("Commands:");
        Console.WriteLine(" add <title>     Adds a task");
        Console.WriteLine(" list            Lists tasks");
        Console.WriteLine(" done <id>       Marks task done");
        Console.WriteLine(" exit            Quits");
    }

    private void HandleAdd(string arg)
    {
        try
        {
            var created = _service.Add(arg);
            PrintSaveResult(
                _storage.TrySaveTasks(_service.GetAll(), out var saveError),
                saveError,
                $"Added: [ ] {created.Id}: {created.Title}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void HandleList()
    {
        var tasks = _service.GetAll();

        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks yet.");
            return;
        }

        foreach (var t in tasks)
        {
            var mark = t.IsDone ? "x" : " ";
            Console.WriteLine($"[{mark}] {t.Id}: {t.Title}");
        }
    }

    private void HandleDone(string arg)
    {
        if (!int.TryParse(arg, out var id))
        {
            Console.WriteLine("Invalid id. Example: done 1");
            return;
        }

        var ok = _service.MarkDone(id);

        if (!ok)
        {
            Console.WriteLine($"Task not found: {id}");
            return;
        }

        PrintSaveResult(
            _storage.TrySaveTasks(_service.GetAll(), out var saveError),
            saveError,
            $"Marked done: {id}");
    }

    private void PrintSaveResult(bool saveSucceeded, string saveError, string successMessage)
    {
        Console.WriteLine(successMessage);

        if (!saveSucceeded)
        {
            Console.WriteLine($"{saveError} Changes are only in memory and may be lost when the app exits.");
        }
    }
}