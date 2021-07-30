<img src="https://github.com/onixion/AlinSpace.Commands/blob/main/Assets/Icon.jpg" width="200" height="200">

# AsyncCommands
[![NuGet version (AsyncCommands)](https://img.shields.io/nuget/v/AlinSpace.Commands.svg?style=flat-square)](https://www.nuget.org/packages/AlinSpace.Commands/)

A simple Async library for command and command manager patterns.

The *IAsyncCommand* interface is very similar to the *ICommand* interface, but they are **not** the same.
The Async interface is a fully asynchronous command interface, whereas *ICommand* is **not**.

[NuGet package](https://www.nuget.org/packages/AlinSpace.Commands/)

## Examples - AsyncCommand

This is the *AsyncCommand*:

 ```csharp
var asyncCommand = AsyncCommand
    .New()
    .OnCanChange(_ => canExecute)
    .OnExecuteAsync(ExecuteSomethingAsync);
    
var genericAsyncCommand = AsyncCommand
    .New<int>()
    .OnCanChange(_ => canExecute)
    .OnExecuteAsync(ExecuteSomethingWithIntAsync);
    
```

## Examples - AbstractAsyncCommand

This is the *AbstractAsyncCommand*:

 ```csharp
public class MyAsyncCommand : AbstractAsyncCommand
{
    ...
    public async Task ExecuteAsync(object parameter = null)
    {
        await DoSomethingAsync(parameter);
    }
    ...
}

public class MyGenericAsyncCommand<int> : AbstractAsyncCommand
{
    ...
    public async Task ExecuteAsync(int parameter = null)
    {
        await DoSomethingWithIntAsync(parameter);
    }
    ...
}
```

## Examples - AsyncCommandManager

The *AsyncCommandManager* allows to create *command groups*. 
Command groups dictate the availability of command execution.
Async commands are registered to one command group.
When registering a *AsyncCommand* to a group, it will return an *ICommand* instance.
This instance can be passed to the view that consumes the command.
The command manager will hide all the logic for locking, unlocking, and notifying commands for you.
Additionally, each registered Async command can also add instance-specific logic for CanExecute.

Here are some examples:

```csharp
AsyncCommandManager
    .New()
    .LockAll(e => 
    {
       SaveCommand = e.Register(SaveAsyncCommand);
       DeleteCommand = e.Register(DeleteAsyncCommand);
    });
```

Each command group defines a group lock behavior. This behavior defines how the group will affect other groups when it gets locked.
On default the command group lock is set to *LockAllGroups*, meaning all groups will be locked when a command is executed from this group.
When an group is locked by at least one group, all commands registered to this group will not be able to execute until released by all groups.

These are the currently supported group lock behaviors:
 * *LockAllGroups*: Locks all groups when executing a command registered to this group.
 * *LockOtherGroups*: Locks all other groups when executing a command registered to this group.
 * *LockThisGroup*: Locks this group when executing a command registered to this group.
 
 In the following example, *SaveCommand* and *DeleteCommand* will lock all commands registered to the command manager when executed.
 However, the *SearchCommand* will lock all commands registered to the command manager except itself when executed.
 
 ```csharp
AsyncCommandManager
    .New()
    .LockAll(eg => 
    {
       SaveCommand = eg.Register(SaveAsyncCommand);
       DeleteCommand = eg.Register(DeleteAsyncCommand);
    })
    .LockAllOthers(eg => 
    {
       SearchCommand = eg.Register(SearchAsyncCommand);
    });
```

An important detail is that locking requirements can **overlap**.
Here is an example:
The *Block10Command* will do 10 seconds of work.
The *Block20Command* will do 20 seconds of work.

```csharp
AsyncCommandManager
    .New()
    .LockThis(eg => Block10Command = eg.Register(Block10Command))
    .LockAll(eg => Block20Command = eg.Register(Block20Command));
```

When you press the *Block10Command* the command manager will lock the *Block10Command* for 10 seconds.
If you wait 5 seconds and then execute the *Block20Command* command, all commands will be locked for another 20 seconds.
After waiting for 5 seconds the *Block10Command* will be done and unlock its group, but because the other command group still blocks all groups for another 15 seconds, *Block10Command* will stay blocked for 15 seconds.
*Block10Command* and *Block20Command* will both be unlocked at the same time.
