using System.IO;
using TaskForge.Core.Services;
using TaskForge.Core.Storage;

namespace TaskForge.Cli;

public class CommandHandler
{
    private readonly TaskService _service;
    private readonly ITaskStorage _storage;
    private readonly TextWriter _output;

    public CommandHandler(TaskService service, ITaskStorage storage, TextWriter output)
    {
        _service = service;
        _storage = storage;
        _output = output;
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

            case "delete":
                HandleDelete(arg);
                return true;

            case "exit":
                return false;

            default:
                _output.WriteLine("Unknown command. Type 'help'.");
                return true;
        }
    }

    private void PrintHelp()
    {
        _output.WriteLine("Commands:");
        _output.WriteLine(" add <title>     Adds a task");
        _output.WriteLine(" list            Lists tasks");
        _output.WriteLine(" done <id>       Marks task done");
        _output.WriteLine(" delete <id>     Deletes a task");
        _output.WriteLine(" exit            Quits");
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
            _output.WriteLine(ex.Message);
        }
    }

    private void HandleList()
    {
        var tasks = _service.GetAll();

        if (tasks.Count == 0)
        {
            _output.WriteLine("No tasks yet.");
            return;
        }

        foreach (var t in tasks)
        {
            var mark = t.IsDone ? "x" : " ";
            _output.WriteLine($"[{mark}] {t.Id}: {t.Title}");
        }
    }

    private void HandleDone(string arg)
    {
        if (!int.TryParse(arg, out var id))
        {
            _output.WriteLine("Invalid id. Example: done 1");
            return;
        }

        var ok = _service.MarkDone(id);

        if (!ok)
        {
            _output.WriteLine($"Task not found: {id}");
            return;
        }

        PrintSaveResult(
            _storage.TrySaveTasks(_service.GetAll(), out var saveError),
            saveError,
            $"Marked done: {id}");
    }

    private void HandleDelete(string arg)
    {
        if (!int.TryParse(arg, out var id))
        {
            _output.WriteLine("Invalid id. Example: delete 1");
            return;
        }

        var ok = _service.Delete(id);

        if (!ok)
        {
            _output.WriteLine($"Task not found: {id}");
            return;
        }

        PrintSaveResult(
            _storage.TrySaveTasks(_service.GetAll(), out var saveError),
            saveError,
            $"Deleted task: {id}");
    }

    private void PrintSaveResult(bool saveSucceeded, string saveError, string successMessage)
    {
        _output.WriteLine(successMessage);

        if (!saveSucceeded)
        {
            _output.WriteLine($"{saveError} Changes are only in memory and may be lost when the app exits.");
        }
    }
}