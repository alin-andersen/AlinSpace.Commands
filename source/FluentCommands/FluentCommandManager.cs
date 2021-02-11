using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;

namespace FluentCommands
{
    /// <summary>
    /// Default implementation of <see cref="IFluentCommandManager"/>.
    /// </summary>
    public class FluentCommandManager : IFluentCommandManager
    {
        /// <summary>
        /// Verify CanExecute allows execution before invoking a command.
        /// </summary>
        public bool VerifyCanExecuteBeforeExecution { get; }

        /// <summary>
        /// Ignore CanExecute of all commands.
        /// </summary>
        public bool IgnoreIndividualCanExecute { get; }

        /// <summary>
        /// Ignore exceptions thrown from commands.
        /// </summary>
        public bool IgnoreExceptionsFromCommands { get; }

        /// <summary>
        /// Continue on captured context.
        /// </summary>
        public bool ContinueOnCapturedContext { get; }

        /// <summary>
        /// Execution groups.
        /// </summary>
        readonly IList<ExecutionGroup> executionGroups = new List<ExecutionGroup>();

        /// <summary>
        /// Create command manager.
        /// </summary>
        /// <param name="verifyCanExecuteBeforeExecution"></param>
        /// <param name="ignoreIndividualCanExecute"></param>
        /// <param name="ignoreExceptionsFromCommands"></param>
        /// <param name="continueOnCapturedContext"></param>
        /// <returns>Command manager.</returns>
        public static FluentCommandManager New(
            bool verifyCanExecuteBeforeExecution = false,
            bool ignoreIndividualCanExecute = false,
            bool ignoreExceptionsFromCommands = true,
            bool continueOnCapturedContext = true)
        {
            return new FluentCommandManager(
                verifyCanExecuteBeforeExecution, 
                ignoreIndividualCanExecute,
                ignoreExceptionsFromCommands,
                continueOnCapturedContext);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="verifyCanExecuteBeforeExecution"></param>
        /// <param name="ignoreIndividualCanExecute"></param>
        /// <param name="ignoreExceptionsFromCommands"></param>
        /// <param name="continueOnCapturedContext"></param>
        public FluentCommandManager(
            bool verifyCanExecuteBeforeExecution = false, 
            bool ignoreIndividualCanExecute = false,
            bool ignoreExceptionsFromCommands = true,
            bool continueOnCapturedContext = true)
        {
            VerifyCanExecuteBeforeExecution = verifyCanExecuteBeforeExecution;
            IgnoreIndividualCanExecute = ignoreIndividualCanExecute;
            IgnoreExceptionsFromCommands = ignoreExceptionsFromCommands;
            ContinueOnCapturedContext = continueOnCapturedContext;
        }

        /// <summary>
        /// Add execution group.
        /// </summary>
        /// <param name="exectionGroupCallback">Execution group callback.</param>
        /// <param name="lock">Lock.</param>
        /// <returns>Command manager.</returns>
        public IFluentCommandManager AddGroup(Action<IExecutionGroup> exectionGroupCallback, LockBehaviour @lock = LockBehaviour.LockAllGroups)
        {
            var executionGroup = new ExecutionGroup(
                commandManager: this,
                @lock: @lock);

            exectionGroupCallback(executionGroup);

            executionGroups.Add(executionGroup);
            return this;
        }

        #region Internal

        /// <summary>
        /// Get execution groups to lock for the given execution group.
        /// </summary>
        /// <param name="executionGroup">Execition group.</param>
        /// <returns>Enumerable of execution groups that shall be locked.</returns>
        IEnumerable<ExecutionGroup> GetExecutionGroupsToLock(ExecutionGroup executionGroup)
        {
            if (executionGroup.Lock == LockBehaviour.LockThisGroup)
                return new[] { executionGroup };

            IList<ExecutionGroup> executionGroupsToLock = new List<ExecutionGroup>();

            foreach (var ex in executionGroups)
            {
                if (ReferenceEquals(ex, executionGroup))
                {
                    if (executionGroup.Lock == LockBehaviour.LockAllOthersGroups)
                        continue;
                }
                
                executionGroupsToLock.Add(ex);
            }

            return executionGroupsToLock;
        }

        void LockExecutionGroup(ExecutionGroup executionGroup)
        {
            var groups = GetExecutionGroupsToLock(executionGroup);

            groups.ForEach(group => Interlocked.Increment(ref group.LockedCounter));
            groups.ForEach(group => RaiseCanExecuteChangedForExecutionGroup(group));
        }

        void UnlockExecutionGroup(ExecutionGroup executionGroup)
        {
            var groups = GetExecutionGroupsToLock(executionGroup);

            groups.ForEach(group => Interlocked.Decrement(ref group.LockedCounter));
            groups.ForEach(group => RaiseCanExecuteChangedForExecutionGroup(group));
        }

        void RaiseCanExecuteChangedForExecutionGroup(ExecutionGroup executionGroup)
        {
            foreach (var executionGroupCommand in executionGroup.Commands)
            {
                try
                {
                    executionGroupCommand.RaiseCanExecuteChanged();
                }
                catch(Exception)
                {
                    if (IgnoreExceptionsFromCommands)
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
            public LockBehaviour Lock { get; }

            /// <summary>
            /// Is locked.
            /// </summary>
            public int LockedCounter = 0;

            /// <summary>
            /// Commands.
            /// </summary>
            public IList<ICanExecuteChangedCommand> Commands = new List<ICanExecuteChangedCommand>();

            /// <summary>
            /// Constructor.
            /// </summary>
            public ExecutionGroup(FluentCommandManager commandManager, LockBehaviour @lock)
            {
                this.commandManager = commandManager;
                Lock = @lock;
            }

            /// <summary>
            /// Register command to the execution group.
            /// </summary>
            /// <param name="command">Command to register.</param>
            /// <returns>Registered command.</returns>
            public ICommand Register(IFluentCommand command)
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
            public ICommand Register<TParameter>(IFluentCommand<TParameter> command)
            {
                var executionGroupCommand = new ExecutionGroupCommand<TParameter>(
                    commandManager: commandManager,
                    executionGroup: this,
                    originalCommand: command);

                Commands.Add(executionGroupCommand);
                return (executionGroupCommand);
            }
        }

        /// <summary>
        /// Can execute changed command.
        /// </summary>
        interface ICanExecuteChangedCommand : ICommand
        {
            /// <summary>
            /// Raise can execute changed.
            /// </summary>
            void RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Execution group command.
        /// </summary>
        class ExecutionGroupCommand : ICanExecuteChangedCommand
        {
            readonly FluentCommandManager commandManager;
            readonly ExecutionGroup executionGroup;
            readonly IFluentCommand originalCommand;

            /// <summary>
            /// Constructor.
            /// </summary>
            public ExecutionGroupCommand(
                FluentCommandManager commandManager,
                ExecutionGroup executionGroup,
                IFluentCommand originalCommand)
            {
                this.commandManager = commandManager;
                this.executionGroup = executionGroup;
                this.originalCommand = originalCommand;

                this.originalCommand.CanExecuteChanged += (s, e) => RaiseCanExecuteChanged();
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
                catch (Exception)
                {
                    if (commandManager.IgnoreExceptionsFromCommands)
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
                if (commandManager.IgnoreIndividualCanExecute)
                    return true;

                try
                {
                    return originalCommand.CanExecute(parameter);
                }
                catch (Exception)
                {
                    // If an exception is thrown, and we should ignore it,
                    // then we will simply say the command shall not be called.
                    if (commandManager.IgnoreExceptionsFromCommands)
                        return false;

                    throw;
                }
            }

            public async void Execute(object parameter)
            {
                if (commandManager.VerifyCanExecuteBeforeExecution)
                {
                    if (!CanExecute(parameter))
                        return;
                }

                try
                {
                    commandManager.LockExecutionGroup(executionGroup);
                    await originalCommand.ExecuteAsync(parameter).ConfigureAwait(commandManager.ContinueOnCapturedContext);
                }
                catch (Exception)
                {
                    if (commandManager.IgnoreExceptionsFromCommands)
                        return;

                    throw;
                }
                finally
                {
                    commandManager.UnlockExecutionGroup(executionGroup);
                }
            }
        }

        /// <summary>
        /// Execution group command (generic).
        /// </summary>
        class ExecutionGroupCommand<TParameter> : ICanExecuteChangedCommand
        {
            readonly FluentCommandManager commandManager;
            readonly ExecutionGroup executionGroup;
            readonly IFluentCommand<TParameter> originalCommand;

            /// <summary>
            /// Constructor.
            /// </summary>
            public ExecutionGroupCommand(
                FluentCommandManager commandManager,
                ExecutionGroup executionGroup,
                IFluentCommand<TParameter> originalCommand)
            {
                this.commandManager = commandManager;
                this.executionGroup = executionGroup;
                this.originalCommand = originalCommand;

                this.originalCommand.CanExecuteChanged += (s, e) => RaiseCanExecuteChanged();
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
                catch (Exception)
                {
                    if (commandManager.IgnoreExceptionsFromCommands)
                        return;

                    throw;
                }
            }

            public bool CanExecute(object parameter)
            {
                return CanExecute((TParameter)parameter);
            }

            public bool CanExecute(TParameter parameter)
            {
                // Check if execution group allows the command to execute.
                if (executionGroup.LockedCounter > 0)
                    return false;

                // Check if we should ignore the individual can execute method.
                if (commandManager.IgnoreIndividualCanExecute)
                    return true;

                try
                {
                    return originalCommand.CanExecute(parameter);
                }
                catch (Exception)
                {
                    // If an exception is thrown, and we should ignore it,
                    // then we will simply say the command shall not be called.
                    if (commandManager.IgnoreExceptionsFromCommands)
                        return false;

                    throw;
                }
            }

            public void Execute(object parameter)
            {
                Execute((TParameter)parameter);
            }

            public async void Execute(TParameter parameter)
            {
                if (commandManager.VerifyCanExecuteBeforeExecution)
                {
                    if (!CanExecute(parameter))
                        return;
                }

                try
                {
                    commandManager.LockExecutionGroup(executionGroup);
                    await originalCommand.ExecuteAsync(parameter).ConfigureAwait(commandManager.ContinueOnCapturedContext);
                }
                catch (Exception)
                {
                    if (commandManager.IgnoreExceptionsFromCommands)
                        return;

                    throw;
                }
                finally
                {
                    commandManager.UnlockExecutionGroup(executionGroup);
                }
            }
        }

        #endregion
    }
}
