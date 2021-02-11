# ![Icon](assets/Icon.png)
# FluentCommands
[![NuGet version (FluentCommands)](https://img.shields.io/nuget/v/FluentCommandsNet.svg?style=flat-square)](https://www.nuget.org/packages/FluentCommandsNet/)

A simple fluent library for command and command manager patterns.

# Examples - FluentCommand

This is the *FluentCommand*:
 ```csharp
var fluentCommand = FluentCommand
    .New()
    .OnCanChange(_ => canExecute)
    .OnExecute(parameter => ExecuteSomething(parameter));
```

There is also the asynchronous counterpart *FluentAsyncCommand*.
```csharp
var fluentCommand = FluentAsyncCommand
    .New()
    .OnCanChange(_ => canExecute)
    .OnExecuteAsync(parameter => await ExecuteSomethingAsync(parameter));
```

# Examples - FluentCommandManager
The *FluentCommandManager* allows to add *execution groups*. Commands are then registered into one or multiple execution groups.
Execution groups dictate availability of command execution.

```csharp

FluentCommandManager
    .New()
    .LockAll(e => 
    {
       SaveCommand = e.Register(SaveCommand);
       DeleteCommand = e.Register(DeleteCommand);
    });
      
# Same configuration as above
FluentCommandManager
  .New()
  .AddGroup(executionGroup => 
  {
     SaveCommand = executionGroup.Register(SaveCommand);
     DeleteCommand = executionGroup.Register(DeleteCommand);
  });
        
# Same configuration as above
FluentCommandManager
  .New()
  .AddGroup(
      executionGroup => 
      {
        SaveCommand = executionGroup.Register(SaveCommand);
        DeleteCommand = executionGroup.Register(DeleteCommand);
      },
      ExecutionLock.LockAllGroups);
```

Each execution group defines an execution lock behaviour. This lock behahviour defines how the execution group will affect other execution groups when it gets locked.
On default the execution lock is set to *LockAllGroups*, meaning all execution groups will be locked when a command is executed from the group.
When an execution group is locked by at least one execution group, all commands registered to this group will not be able execute until released by all execution groups.

These are the currently supported execution lock behaviours:
 * *LockAllGroups*: Locks all groups when executing a command registered to this group.
 * *LockOtherGroups*: Locks all other groups when executing a command registered to this group.
 * *LockThisGroup*: Locks this group when executing a command registered to this group.
 
 In the following example *SaveCommand* and *DeleteCommand* will lock all commands registered to the command manager when executed.
 However, the *SearchCommand* will lock all commands registered to the command manager except itself when executed.
 
 ```csharp
FluentCommandManager
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

As already mentioned above, one command can be registered to multiple execution groups:
```csharp
FluentCommandManager
    .New()
    .LockThis(eg => 
    {
       StoreCommand = eg.Register(StoreCommand);
       LoadCommand = eg.Register(LoadCommand);
    })
    .LockThis(eg => 
    {
       StoreCommand = eg.Register(StoreCommand);
       FetchCommand = eg.Register(FetchCommand);
    });
```

Here is another example.
The *Block10Command* will do 10 seconds of work.
The *Block20Command* will do 20 seconds of work.
```csharp
FluentCommandManager
    .New()
    .LockThis(eg => 
    {
       Block10Command = eg.Register(Block10Command);
    })
    .LockAll(eg => 
    {
       Block20Command = eg.Register(Block20Command);
    });
```
When you press the *Block10Command* the command manager will lock the *Block10Command* for 10 seconds.
If you wait 5 seconds and then execute the *Block20Command* command, all commands will be locked for another 20 seconds.
After waiting for 5 seconds the *Block10Command* will be done and unlock its execution group, but because the other execution group still blocks all groups for another 15 seconds, *Block10Command* will stay blocked for 15 seconds.
*Block10Command* and *Block20Command* will both be unlocked at the same time.
 
    Note: This is a still a work-in-progress.
