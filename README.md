<img src="https://github.com/onixion/AlinSpace.Commands/blob/main/Assets/Icon.png" width="200" height="200">

# AlinSpace.Commands
[![NuGet version (AlinSpace.Commands)](https://img.shields.io/nuget/v/AlinSpace.Commands.svg?style=flat-square)](https://www.nuget.org/packages/AlinSpace.Commands/)

An asynchronous command and command manager implementation.

[NuGet package](https://www.nuget.org/packages/AlinSpace.Commands/)

# Why?

When you need or have **complex concurrent command locking rules**, this library is something for you.

# Asynchronous Commands

## Examples - *AsyncCommand*

This is the *AsyncCommand*:

 ```csharp
var command = AsyncCommand
    .New()
    .SetCanChange(_ => canExecute)
    .SetExecuteAsync(ExecuteSomethingAsync);
    
var genericCommand = AsyncCommand
    .New<int>()
    .SetCanChange(_ => canExecute)
    .SetExecuteAsync(ExecuteSomethingWithIntAsync);
    
```

## Examples - *AbstractAsyncCommand*

This is the *AbstractAsyncCommand*:

 ```csharp
public class MyAsyncCommand : AbstractAsyncCommand
{
    ...
    public async Task ExecuteAsync(object? parameter = null)
    {
        await DoSomethingAsync(parameter);
    }
    ...
}

public class MyGenericAsyncCommand<int> : AbstractAsyncCommand
{
    ...
    public async Task ExecuteAsync(int? parameter = null)
    {
        await DoSomethingWithIntAsync(parameter);
    }
    ...
}
```

# Asynchronous Command Manager

The *AsyncManager* allows you to create *command execution groups*. 
Command execution groups dictate the availability of command execution.
Commands are registered to one (or more) command groups.
When registering a *IAsyncCommand* to a group, it will return a new instance of *IAsyncCommand*.
This instance can be passed to the view that will act on the the command.
The command manager will hide all the logic for locking, unlocking, and notifying commands for you.
Additionally, each registered asynchronous command can also add command-specific logic for CanExecute.

## Examples - *AsyncManager*

Here are some examples:

```csharp
AsyncManager
    .New()
    .LockAll(e => 
    {
       SaveCommand = e.Register(SaveCommand);
       DeleteCommand = e.Register(DeleteCommand);
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
AsyncManager
    .New()
    .LockAll(eg => 
    {
       SaveCommand = eg.Register(SaveCommand);
       DeleteCommand = eg.Register(DeleteCommand);
    })
    .LockAllOthers(eg => 
    {
       SearchCommand = eg.Register(SearchCommand);
    });
```

An important detail is that locking requirements can **overlap**.
Here is an example:
The *Block10Command* will do 10 seconds of work.
The *Block20Command* will do 20 seconds of work.

```csharp
AsyncManager
    .New()
    .LockThis(eg => Block10Command = eg.Register(Block10Command))
    .LockAll(eg => Block20Command = eg.Register(Block20Command));
```

When you press the *Block10Command* the command manager will lock the *Block10Command* for 10 seconds.
If you wait 5 seconds and then execute the *Block20Command* command, all commands will be locked for another 20 seconds.
After waiting for 5 seconds the *Block10Command* will be done and unlock its group, but because the other command group still blocks all groups for another 15 seconds, *Block10Command* will stay blocked for 15 seconds.
*Block10Command* and *Block20Command* will both be unlocked at the same time.

## AsyncManager Settings

There are a couple of manager settings:
- **VerifyCanExecuteBeforeExecution**: This will force the manager to call *CanExecute* before any command execution.
- **IgnoreIndividualCanExecute**: This will ignore the *CanExecute* method of the command itself and only listen to the command manager.
- **IgnoreExceptionsFromCommands**: This will ignore any exceptions thrown by any given command. This is useful when you never ever want to crash your application and instead ignore it silently.
- **ContinueOnCapturedContext**: Whether or not to continue on the captured context.
- **RaiseCanExecuteChangedOnAllCommandsAfterAnyCommandExecution**: Raise *CanExeucteChanged* on all commands after any command executed.
