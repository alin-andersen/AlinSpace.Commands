using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace FluentCommands
{
    /// <summary>
    /// Implementation of <see cref="ICommandManager"/>.
    /// </summary>
    public class CommandManager : ICommandManager
    {
        readonly bool verifyCanExecuteBeforeExecution;
        readonly bool ignoreIndividualCanExecute;

        readonly IList<ExecutionGroup> executionGroups = new List<ExecutionGroup>();

        ExecutionGroup currentExecutionGroup;

        /// <summary>
        /// Create command manager.
        /// </summary>
        /// <param name="verifyCanExecuteBeforeExecution"></param>
        /// <param name="ignoreIndividualCanExecute"></param>
        /// <returns>Command manager.</returns>
        public static CommandManager Create(
            bool verifyCanExecuteBeforeExecution = false,
            bool ignoreIndividualCanExecute = false)
        {
            return new CommandManager(verifyCanExecuteBeforeExecution, ignoreIndividualCanExecute);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="verifyCanExecuteBeforeExecution"></param>
        /// <param name="ignoreIndividualCanExecute"></param>
        public CommandManager(
            bool verifyCanExecuteBeforeExecution = false, 
            bool ignoreIndividualCanExecute = false)
        {
            this.verifyCanExecuteBeforeExecution = verifyCanExecuteBeforeExecution;
            this.ignoreIndividualCanExecute = ignoreIndividualCanExecute;
        }

        /// <summary>
        /// Add execution group.
        /// </summary>
        /// <param name="exectionGroupCallback">Execution group callback.</param>
        /// <param name="lock">Lock.</param>
        /// <returns>Command manager.</returns>
        public ICommandManager AddGroup(Action<IExecutionGroup> exectionGroupCallback, ExecutionLock @lock = ExecutionLock.LockAllGroups)
        {
            var executionGroup = new ExecutionGroup(
                commandManager: this,
                @lock: @lock);

            executionGroups.Add(executionGroup);

            try
            {
                exectionGroupCallback(executionGroup);
            }
            catch
            {
                // ignore
            }

            return this;
        }

        void StartExecution(ExecutionGroup executionGroup)
        {
            currentExecutionGroup = executionGroup;
            RaiseCanExecuteChanged();
        }

        void EndExecution()
        {
            currentExecutionGroup = null;
            RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Can group execute.
        /// </summary>
        /// <param name="executionGroup">Execution group.</param>
        /// <returns>True, if the group can execut; false otherwise.</returns>
        bool CanGroupExecute(ExecutionGroup executionGroup)
        {
            // If there is no execution group active,
            // the command manager allows the original
            // command to execute (if he wants to).
            if (currentExecutionGroup == null)
                return true;

            // Is the command in this command in the execution group?
            if (ReferenceEquals(currentExecutionGroup, executionGroup))
            {
                switch (currentExecutionGroup.Lock)
                {
                    case ExecutionLock.LockAllGroups:
                    case ExecutionLock.LockThisGroup:
                        return false;

                    default:
                        return true;
                }
            }
            else
            {
                switch (currentExecutionGroup.Lock)
                {
                    case ExecutionLock.LockAllGroups:
                    case ExecutionLock.LockAllOthersGroups:
                        return false;

                    default:
                        return true;
                }
            }
        }

        void RaiseCanExecuteChanged()
        {
            foreach (var executionGroup in executionGroups)
            {
                foreach (var executionGroupCommand in executionGroup.Commands)
                {
                    try
                    {
                        executionGroupCommand.RaiseCanExecuteChanged();
                    }
                    catch
                    {
                        // ignore
                    }
                }
            }
        }

        /// <summary>
        /// Execution group.
        /// </summary>
        class ExecutionGroup : IExecutionGroup
        {
            readonly CommandManager commandManager;

            /// <summary>
            /// Execution lock.
            /// </summary>
            public ExecutionLock Lock { get; }

            /// <summary>
            /// Commands.
            /// </summary>
            public IList<ExecutionGroupCommand> Commands = new List<ExecutionGroupCommand>();

            /// <summary>
            /// Constructor.
            /// </summary>
            public ExecutionGroup(CommandManager commandManager, ExecutionLock @lock)
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
            /// Execution group command.
            /// </summary>
            public class ExecutionGroupCommand : ICommand
            {
                readonly CommandManager commandManager;
                readonly ExecutionGroup executionGroup;
                readonly ICommand originalCommand;

                /// <summary>
                /// Constructor.
                /// </summary>
                public ExecutionGroupCommand(
                    CommandManager commandManager,
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
                    catch
                    {
                        // ignore
                    }
                }

                public bool CanExecute(object parameter)
                {
                    // Check if command manager allows the command to exeucte.
                    if (!commandManager.CanGroupExecute(executionGroup))
                        return false;

                    // Check if we should ignore the individual can execute method.
                    if (commandManager.ignoreIndividualCanExecute)
                        return true;

                    return originalCommand.CanExecute(parameter);
                }

                public void Execute(object parameter)
                {
                    if (commandManager.verifyCanExecuteBeforeExecution)
                    {
                        if (!CanExecute(parameter))
                            return;
                    }

                    try
                    {
                        commandManager.StartExecution(executionGroup);
                        originalCommand.Execute(parameter);
                    }
                    catch
                    {
                        // ignore
                    }
                    finally
                    {
                        commandManager.EndExecution();
                    }
                }
            }
        }
    }
}
