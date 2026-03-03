using TaskForge.Cli.Services;

var service = new TaskService();

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
            // TODO: call service.Add(arg) and print confirmation
            break;

        case "list":
            // TODO: print tasks with [ ] or [x]
            break;

        case "done":
            // TODO: parse id, call service.MarkDone(id)
            break;

        case "exit":
            return;

        default:
            Console.WriteLine("Unknown command. Type 'help'.");
            break;
    }
    
}