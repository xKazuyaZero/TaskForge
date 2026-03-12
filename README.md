# TaskForge

[![CI](https://github.com/xKazuyaZero/TaskForge/actions/workflows/ci.yml/badge.svg)](https://github.com/xKazuyaZero/TaskForge/actions/workflows/ci.yml)

TaskForge is a C# command-line task manager built as a portfolio project to practice real software engineering fundamentals, not just syntax. It supports creating, listing, completing, and deleting tasks, persists data to JSON, and includes automated tests plus GitHub Actions CI.

## Project Overview

TaskForge started as a simple CLI app and was improved through multiple refactoring sprints. The goal was not only to make the application work, but to make the codebase cleaner, safer, easier to test, and easier to maintain over time.

This project focuses on:

- clear separation of concerns
- resilient file-based persistence
- automated unit testing
- dependency inversion
- testable command handling
- continuous integration with GitHub Actions

## Features

- Add tasks from the command line
- List all saved tasks
- Mark tasks as done
- Delete tasks
- Persist tasks to `tasks.json`
- Handle invalid input without crashing
- Recover cleanly from missing, empty, or invalid JSON
- Handle save failures without crashing the app
- Run automated tests for service, storage, and command behavior
- Validate builds and tests automatically in CI

## Solution Structure

TaskForge is split into separate projects so responsibilities stay clear.

```text
TaskForge/
├─ TaskForge.slnx
├─ TaskForge.Cli/
├─ TaskForge.Core/
└─ TaskForge.Core.Tests/
```

### `TaskForge.Cli`

Contains the application entry point and command-line interaction.

Responsibilities:
- application startup
- command loop
- dependency wiring
- delegating commands to `CommandHandler`

### `TaskForge.Core`

Contains the business logic and persistence layer.

Main components:
- `TaskItem` — task model
- `TaskService` — task operations such as add, complete, and delete
- `ITaskStorage` — storage abstraction
- `TaskStorage` — JSON-based persistence implementation

### `TaskForge.Core.Tests`

Contains automated tests for:
- task service behavior
- storage load/save behavior
- command handler behavior

## Architecture

TaskForge follows a simple layered design.

### CLI Layer

Responsible for:
- reading user input
- writing output
- routing commands through `CommandHandler`

### Service Layer

Responsible for:
- task-related business logic
- managing task state in memory
- assigning task IDs
- performing add, complete, and delete operations

### Storage Layer

Responsible for:
- loading tasks from JSON
- saving tasks to JSON
- returning warnings and save results without directly writing to the console

This separation keeps the project easier to test and easier to evolve.

## Commands

TaskForge supports the following commands:

| Command | Description |
|---|---|
| `help` | Shows available commands |
| `add <title>` | Adds a new task |
| `list` | Lists all tasks |
| `done <id>` | Marks a task as completed |
| `delete <id>` | Deletes a task |
| `exit` | Exits the application |

### Example Session

```text
> add Buy milk
Added: [ ] 1: Buy milk

> add Walk dog
Added: [ ] 2: Walk dog

> list
[ ] 1: Buy milk
[ ] 2: Walk dog

> done 1
Marked done: 1

> delete 2
Deleted task: 2

> list
[x] 1: Buy milk
```

## Persistence

Task data is stored in a JSON file so tasks survive between runs.

Persistence behavior includes:

- loading existing tasks at startup
- continuing safely when the file does not exist
- returning an empty list when the file is empty
- handling invalid JSON without crashing
- returning warnings instead of printing directly from the storage layer
- saving changes after successful add, complete, and delete operations
- reporting save failures cleanly instead of crashing

This design keeps storage concerns separate from CLI output concerns.

## Running the Project

### Prerequisites

- .NET 10 SDK

### Restore dependencies

```bash
dotnet restore TaskForge/TaskForge.slnx
```

### Build the solution

```bash
dotnet build TaskForge/TaskForge.slnx --configuration Release
```

### Run the CLI app

```bash
dotnet run --project TaskForge/TaskForge.Cli
```

## Running Tests

Run the full test suite with:

```bash
dotnet test TaskForge/TaskForge.slnx --configuration Release
```

The test suite covers:

- task creation and completion behavior
- deletion behavior
- ID generation for preloaded tasks
- JSON load behavior
- JSON save behavior
- save/load round-trip behavior
- command handler output and control flow

## Continuous Integration

This repository uses GitHub Actions CI to automatically:

- restore dependencies
- build the solution
- run all tests

The workflow runs on:

- `push`
- `pull_request`

## Why This Project Matters

TaskForge is more than a beginner CRUD app. It was built as a progression project to practice the kinds of improvements that make software more professional over time.

This project demonstrates:

- iterative refactoring
- separation of concerns
- dependency injection
- interface-based design
- testable CLI behavior
- resilient persistence
- automated CI validation

## Roadmap

Possible future improvements include:

- editing task titles
- task priorities
- due dates
- filtering and search
- command aliases
- richer error reporting
- integration tests for full CLI flows
- packaging as a distributable console tool

## Author

Built by Pietro as part of a C# learning journey focused on writing cleaner, more testable, and more maintainable software.