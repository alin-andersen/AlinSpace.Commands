# ![Icon](assets/Icon.png)
# FluentCommands
[![NuGet version (FluentCommands)](https://img.shields.io/nuget/v/FluentCommandsNet.svg?style=flat-square)](https://www.nuget.org/packages/FluentCommandsNet/)

A simple fluent library for command and command manager patterns.

# Examples - FluentCommand

The fluent command is not really useful yet. But I will keep it and see what can be done with it.

 ```csharp
var fluentCommand = FluentCommand
    .New()
    .OnCanChange(parameter => canExecute)
    .OnExecute(parameter => ExecuteSomething(p));
```

# Examples - FluentCommandManager
The FluentCommandManager allows to add execution groups. Commands are then registered into an execution group.
The currently active execution group dictates the locking of the commands.

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

Each execution group defines an execution lock. This lock defines the locking behaviour when a command of this group is executed.
On default the execution lock is set to *LockAllGroups*, meaning all execution groups will be locked when a command is executed from the group.
When an execution group is locked by itself or another group, all commands registered to the group will not be able to execute.

These are the currently supported execution locks:
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
 
 Note: This is a still a work-in-progress.
