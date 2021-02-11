# ![Icon](assets/Icon.png)
# FluentCommands
[![NuGet version (FluentCommands)](https://img.shields.io/nuget/v/FluentCommandsNet.svg?style=flat-square)](https://www.nuget.org/packages/FluentCommandsNet/)

A simple fluent library for command and command manager patterns.

The *IFluentCommand* interface is very similar to the *ICommand* interface, but they are **not** the same.
The fluent interface is a fully asynchronous command interface, whereas *ICommand* is **not**.

# Examples - FluentCommand

This is the *FluentCommand*:

 ```csharp
var fluentCommand = FluentCommand
    .New()
    .OnCanChange(_ => canExecute)
    .OnExecuteAsync(ExecuteSomethingAsync);
    
var genericFluentCommand = FluentCommand
    .New<int>()
    .OnCanChange(_ => canExecute)
    .OnExecuteAsync(ExecuteSomethingWithIntAsync);
    
```

# Examples - AbstractFluentCommand

This is the *AbstractFluentCommand*:

 ```csharp
public class MyFluentCommand : AbstractFluentCommand
{
    ...
    public async Task ExecuteAsync(object parameter = null)
    {
        await DoSomethingAsync(parameter);
    }
    ...
}

public class MyGenericFluentCommand<int> : AbstractFluentCommand
{
    ...
    public async Task ExecuteAsync(int parameter = null)
    {
        await DoSomethingWithIntAsync(parameter);
    }
    ...
}
```

# Examples - FluentCommandManager
The *FluentCommandManager* allows to create *execution groups*. 
Execution groups dictate the availability of command execution.
Fluent commands are registered to one execution group.
When registering a *FluentCommand* to an execution group, it will return an *ICommand* instance.
This instance can be passed to the view that consumes the command.
The command manager will hide all the logic for locking, unlocking, and notifying commands for you.
Additionally, each registered fluent command can also add instance-specific logic for CanExecute.

Here are some examples:

```csharp
FluentCommandManager
    .New()
    .LockAll(e => 
    {
       SaveCommand = e.Register(SaveFluentCommand);
       DeleteCommand = e.Register(DeleteFluentCommand);
    });
      
# Same configuration as above
FluentCommandManager
  .New()
  .AddGroup(executionGroup => 
  {
     SaveCommand = executionGroup.Register(SaveFluentCommand);
     DeleteCommand = executionGroup.Register(DeleteFluentCommand);
  });
        
# Same configuration as above
FluentCommandManager
  .New()
  .AddGroup(
      executionGroup => 
      {
        SaveCommand = executionGroup.Register(SaveFluentCommand);
        DeleteCommand = executionGroup.Register(DeleteFluentCommand);
      },
      LockBehavior.LockAllGroups);
```

Each execution group defines a lock behavior. This behavior defines how the execution group will affect other execution groups when it gets locked.
On default the execution group lock is set to *LockAllGroups*, meaning all execution groups will be locked when a command is executed from the group.
When an execution group is locked by at least one execution group, all commands registered to this group will not be able to execute until released by all execution groups.

These are the currently supported execution lock behaviors:
 * *LockAllGroups*: Locks all groups when executing a command registered to this group.
 * *LockOtherGroups*: Locks all other groups when executing a command registered to this group.
 * *LockThisGroup*: Locks this group when executing a command registered to this group.
 
 In the following example, *SaveCommand* and *DeleteCommand* will lock all commands registered to the command manager when executed.
 However, the *SearchCommand* will lock all commands registered to the command manager except itself when executed.
 
 ```csharp
FluentCommandManager
    .New()
    .LockAll(eg => 
    {
       SaveCommand = eg.Register(SaveFluentCommand);
       DeleteCommand = eg.Register(DeleteFluentCommand);
    })
    .LockAllOthers(eg => 
    {
       SearchCommand = eg.Register(SearchFluentCommand);
    });
```

An important detail is that locking requirements can **overlap**.
Here is an example:
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
