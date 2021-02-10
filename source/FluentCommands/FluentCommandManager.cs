using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;

namespace FluentCommands
{
    /// <summary>
    /// Implementation of <see cref="IFluentCommandManager"/>.
    /// </summary>
    public class FluentCommandManager : IFluentCommandManager
    {
        readonly bool verifyCanExecuteBeforeExecution;
        readonly bool ignoreIndividualCanExecute;
        readonly bool ignoreExceptionsFromCommands;

        readonly IList<ExecutionGroup> executionGroups = new List<ExecutionGroup>();

        /// <summary>
        /// Create command manager.
        /// </summary>
        /// <param name="verifyCanExecuteBeforeExecution"></param>
        /// <param name="ignoreIndividualCanExecute"></param>
        /// <param name="ignoreExceptionsFromCommands"></param>
        /// <returns>Command manager.</returns>
        public static FluentCommandManager New(
            bool verifyCanExecuteBeforeExecution = false,
            bool ignoreIndividualCanExecute = false,
            bool ignoreExceptionsFromCommands = true)
        {
            return new FluentCommandManager(
                verifyCanExecuteBeforeExecution, 
                ignoreIndividualCanExecute,
                ignoreExceptionsFromCommands);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="verifyCanExecuteBeforeExecution"></param>
        /// <param name="ignoreIndividualCanExecute"></param>
        /// <param name="ignoreExceptionsFromCommands"></param>
        public FluentCommandManager(
            bool verifyCanExecuteBeforeExecution = false, 
            bool ignoreIndividualCanExecute = false,
            bool ignoreExceptionsFromCommands = true)
        {
            this.verifyCanExecuteBeforeExecution = verifyCanExecuteBeforeExecution;
            this.ignoreIndividualCanExecute = ignoreIndividualCanExecute;
            this.ignoreExceptionsFromCommands = ignoreExceptionsFromCommands;
        }

        /// <summary>
        /// Add execution group.
        /// </summary>
        /// <param name="exectionGroupCallback">Execution group callback.</param>
        /// <param name="lock">Lock.</param>
        /// <returns>Command manager.</returns>
        public IFluentCommandManager AddGroup(Action<IExecutionGroup> exectionGroupCallback, ExecutionLock @lock = ExecutionLock.LockAllGroups)
        {
            var executionGroup = new ExecutionGroup(
                commandManager: this,
                @lock: @lock);

            exectionGroupCallback(executionGroup);

            executionGroups.Add(executionGroup);
            return this;
        }

        IEnumerable<ExecutionGroup> GetExecutionGroupsToLock(ExecutionGroup executionGroup)
        {
            if (executionGroup.Lock == ExecutionLock.LockThisGroup)
                return new[] { executionGroup };

            IList<ExecutionGroup> executionGroupsToLock = new List<ExecutionGroup>();

            foreach (var ex in executionGroups)
            {
                if (ReferenceEquals(ex, executionGroup))
                {
                    if (executionGroup.Lock == ExecutionLock.LockAllOthersGroups)
                        continue;
                }
                
                executionGroupsToLock.Add(ex);
            }

            return executionGroupsToLock;
        }

        void StartExecution(ExecutionGroup executionGroup)
        {
            var groups = GetExecutionGroupsToLock(executionGroup);

            groups.ForEach(group => Interlocked.Increment(ref group.LockedCounter));
            groups.ForEach(group => RaiseCanExecuteChangedForGroup(group));
        }

        void EndExecution(ExecutionGroup executionGroup)
        {
            var groups = GetExecutionGroupsToLock(executionGroup);

            groups.ForEach(group => Interlocked.Decrement(ref group.LockedCounter));
            groups.ForEach(group => RaiseCanExecuteChangedForGroup(group));
        }

        void RaiseCanExecuteChangedForGroup(ExecutionGroup executionGroup)
        {
            foreach (var executionGroupCommand in executionGroup.Commands)
            {
                try
                {
                    executionGroupCommand.RaiseCanExecuteChanged();
                }
                catch(Exception)
                {
                    if (ignoreExceptionsFromCommands)
                        return;

                    throw;
                }
            }
        }

        /// <summary>
        /// Execution group.
        /// </summary>
        class ExecutionGroup : IExecutionGroup
        {
            readonly FluentCommandManager commandManager;

            /// <summary>
            /// Execution lock.
            /// </summary>
            public ExecutionLock Lock { get; }

            /// <summary>
            /// Is locked.
            /// </summary>
            public int LockedCounter = 0;

            /// <summary>
            /// Commands.
            /// </summary>
            public IList<ExecutionGroupCommand> Commands = new List<ExecutionGroupCommand>();

            /// <summary>
            /// Constructor.
            /// </summary>
            public ExecutionGroup(FluentCommandManager commandManager, ExecutionLock @lock)
            {
                this.commandManager = commandManager;
                Lock = @lock;
            }

            /// <summary>
            /// Register command to the execution group.
            /// </summary>
            /// <param name="command">Command to register.</param>
            /// <returns>Registered command.</returns>
            public ICommand RegisterCommand(ICommand command)
            {
                var executionGroupCommand = new ExecutionGroupCommand(
                    commandManager: commandManager,
                    executionGroup: this, 
                    originalCommand: command);

                Commands.Add(executionGroupCommand);
                return executionGroupCommand;
            }

            /// <summary>
            /// Register command to the execution group.
            /// </summary>
            /// <param name="command">Command to register.</param>
            /// <returns>Registered command.</returns>
            public ICommand RegisterCommand(IAsyncCommand command)
            {
                var executionGroupCommand = new ExecutionGroupCommand(
                    commandManager: commandManager,
                    executionGroup: this,
                    originalCommand: command);

                Commands.Add(executionGroupCommand);
                return executionGroupCommand;
            }

            /// <summary>
            /// Execution group command.
            /// </summary>
            public class ExecutionGroupCommand : ICommand
            {
                readonly FluentCommandManager commandManager;
                readonly ExecutionGroup executionGroup;
                readonly ICommand originalCommand;

                /// <summary>
                /// Constructor.
                /// </summary>
                public ExecutionGroupCommand(
                    FluentCommandManager commandManager,
                    ExecutionGroup executionGroup, 
                    ICommand originalCommand)
                {
                    this.commandManager = commandManager;
                    this.executionGroup = executionGroup;
                    this.originalCommand = originalCommand;
                }

                /// <summary>
                /// Can execute changed.
                /// </summary>
                public event EventHandler CanExecuteChanged = delegate { };

                /// <summary>
                /// Raise can execute changed.
                /// </summary>
                public void RaiseCanExecuteChanged()
                {
                    try
                    {
                        CanExecuteChanged(this, EventArgs.Empty);
                    }
                    catch(Exception)
                    {
                        if (commandManager.ignoreExceptionsFromCommands)
                            return;

                        throw;
                    }
                }

                public bool CanExecute(object parameter)
                {
                    // Check if execution group allows the command to execute.
                    if (executionGroup.LockedCounter > 0)
                        return false;

                    // Check if we should ignore the individual can execute method.
                    if (commandManager.ignoreIndividualCanExecute)
                        return true;

                    try
                    {
                        return originalCommand.CanExecute(parameter);
                    }
                    catch (Exception)
                    {
                        // If an exception is thrown, and we should ignore it,
                        // then we will simply say the command shall not be called.
                        if (commandManager.ignoreExceptionsFromCommands)
                            return false;

                        throw;
                    }
                }

                public async void Execute(object parameter)
                {
                    if (commandManager.verifyCanExecuteBeforeExecution)
                    {
                        if (!CanExecute(parameter))
                            return;
                    }

                    try
                    {
                        commandManager.StartExecution(executionGroup);
                     
                        if (originalCommand is IAsyncCommand originalAsyncCommand)
                        {
                            await originalAsyncCommand.ExecuteAsync(parameter);
                        }
                        else
                        {
                            originalCommand.Execute(parameter);
                        }
                    }
                    catch (Exception)
                    {
                        if (commandManager.ignoreExceptionsFromCommands)
                            return;

                        throw;
                    }
                    finally
                    {
                        commandManager.EndExecution(executionGroup);
                    }
                }
            }
        }
    }
}
