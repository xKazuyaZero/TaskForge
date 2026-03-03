using System;

Console.WriteLine("TaskForge booted.");
Console.Write("Enter your name: ");
var name = Console.ReadLine();

if (string.IsNullOrWhiteSpace(name))
{
    Console.WriteLine("No name entered. Exiting.");
    return;
}

Console.WriteLine($"Welcome, {name}. Today we ship.");