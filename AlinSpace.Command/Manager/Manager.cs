using AlinSpace.System;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlinSpace.Command
{
    /// <summary>
    /// Default implementation of <see cref="IManager"/>.
    /// </summary>
    public partial class Manager : IManager
    {
        /// <summary>
        /// Settings.
        /// </summary>
        readonly CommandManagerSettings settings = new CommandManagerSettings();

        /// <summary>
        /// Execution groups.
        /// </summary>
        readonly IList<Group> executionGroups = new List<Group>();

        /// <summary>
        /// Create command manager.
        /// </summary>
        /// <param name="settingsCallback">Optional settings callback.</param>
        /// <returns>Async command manager.</returns>
        public static Manager New(Action<IManagerSettings> settingsCallback = null)
        {
            return new Manager(settingsCallback);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settingsCallback">Optional settings callback.</param>
        public Manager(Action<IManagerSettings> settingsCallback = null)
        {
            settingsCallback?.Invoke(settings);
        }

        /// <summary>
        /// Add execution group.
        /// </summary>
        /// <param name="commandRegistrations">Command registrations.</param>
        /// <param name="lock">Lock.</param>
        /// <returns>Command manager.</returns>
        public IManager AddGroup(Action<IGroupRegistrator> commandRegistrations, GroupLockBehavior @lock = GroupLockBehavior.LockAllGroups)
        {
            var group = new Group(
                manager: this,
                @lock: @lock);

            commandRegistrations(group);

            executionGroups.Add(group);
            return this;
        }

        #region Internal

        /// <summary>
        /// Default value for <see cref="IManagerSettings"/>.
        /// </summary>
        class CommandManagerSettings : IManagerSettings
        {
            /// <summary>
            /// Verify CanExecute allows execution before invoking a command.
            /// </summary>
            public bool VerifyCanExecuteBeforeExecution { get; set; }

            /// <summary>
            /// Ignore CanExecute of all commands.
            /// </summary>
            public bool IgnoreIndividualCanExecute { get; set; }

            /// <summary>
            /// Ignore exceptions thrown from commands.
            /// </summary>
            public bool IgnoreExceptionsFromCommands { get; set; }

            /// <summary>
            /// Continue on captured context.
            /// </summary>
            public bool ContinueOnCapturedContext { get; set; } = true;
        }

        /// <summary>
        /// Get groups to lock for the given group.
        /// </summary>
        /// <param name="group">Group.</param>
        /// <returns>Enumerable of groups that shall be locked.</returns>
        IEnumerable<Group> GetGroupsToLock(Group group)
        {
            if (group.Lock == GroupLockBehavior.LockNothing)
                return new Group[] { };

            if (group.Lock == GroupLockBehavior.LockThisGroup)
                return new Group[] { group };

            IList<Group> groupsToLock = new List<Group>();

            foreach (var ex in executionGroups)
            {
                if (ReferenceEquals(ex, group))
                {
                    if (group.Lock == GroupLockBehavior.LockAllOtherGroups)
                        continue;
                }

                groupsToLock.Add(ex);
            }

            return groupsToLock;
        }

        #region Locking / Unlocking

        /// <summary>
        /// Lock group.
        /// </summary>
        /// <param name="group">Group to lock.</param>
        void LockGroup(Group group)
        {
            var groups = GetGroupsToLock(group);

            groups.ForEach(x => Interlocked.Increment(ref x.LockedCounter));
            groups.ForEach(x => RaiseCanExecuteChangedForExecutionGroup(x));
        }

        /// <summary>
        /// Unlock group.
        /// </summary>
        /// <param name="group">Group to unlock.</param>
        void UnlockGroup(Group group)
        {
            var groups = GetGroupsToLock(group);

            groups.ForEach(x => Interlocked.Decrement(ref x.LockedCounter));
            groups.ForEach(x => RaiseCanExecuteChangedForExecutionGroup(x));
        }

        #endregion

        /// <summary>
        /// Raise CanExecuteChanged for the given execution group.
        /// </summary>
        /// <param name="executionGroup">Execution group.</param>
        void RaiseCanExecuteChangedForExecutionGroup(Group executionGroup)
        {
            foreach (var executionGroupCommand in executionGroup.Commands)
            {
                try
                {
                    executionGroupCommand.RaiseCanExecuteChanged();
                }
                catch (Exception)
                {
                    if (settings.IgnoreExceptionsFromCommands)
                        return;

                    throw;
                }
            }
        }

        #region CanExecute / Execute

        /// <summary>
        /// Can execute command from execution group.
        /// </summary>
        /// <param name="command">Execution group command.</param>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>True, if the command can execute; false otherwise.</returns>
        bool CanExecuteCommandFromGroup(GroupCommand command, object parameter)
        {
            // Check if execution group allows the command to execute.
            if (command.Group.LockedCounter > 0)
                return false;

            // Check if we should ignore the individual can execute method.
            if (settings.IgnoreIndividualCanExecute)
                return true;

            try
            {
                return command.OriginalCommand.CanExecute(parameter);
            }
            catch (Exception)
            {
                // If an exception is thrown, and we should ignore it,
                // then we will simply say the command shall not be called.
                if (settings.IgnoreExceptionsFromCommands)
                    return false;

                throw;
            }
        }

        /// <summary>
        /// Execute command from execution group.
        /// </summary>
        /// <param name="command">Execution group command.</param>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>Task.</returns>
        async Task ExecuteCommandFromGroupAsync(GroupCommand command, object parameter)
        {
            if (settings.VerifyCanExecuteBeforeExecution)
            {
                if (!command.OriginalCommand.CanExecute(parameter))
                    return;
            }

            try
            {
                LockGroup(command.Group);

                await command.OriginalCommand
                    .ExecuteAsync(parameter)
                    .ConfigureAwait(settings.ContinueOnCapturedContext);
            }
            catch (Exception)
            {
                if (settings.IgnoreExceptionsFromCommands)
                    return;

                throw;
            }
            finally
            {
                UnlockGroup(command.Group);
            }
        }

        #endregion

        /// <summary>
        /// Execution group.
        /// </summary>
        class Group : IGroupRegistrator
        {
            readonly Manager manager;

            /// <summary>
            /// Execution lock.
            /// </summary>
            public GroupLockBehavior Lock { get; }

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
            public Group(Manager manager, GroupLockBehavior @lock)
            {
                this.manager = manager;
                Lock = @lock;
            }

            /// <summary>
            /// Register command to the execution group.
            /// </summary>
            /// <param name="command">Command to register.</param>
            /// <returns>Registered command.</returns>
            public ICommand Register(ICommand command)
            {
                var groupCommand = new GroupCommand(
                    manager: manager,
                    group: this,
                    originalCommand: command);

                Commands.Add(groupCommand);
                return groupCommand;
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
        /// Group command.
        /// </summary>
        class GroupCommand : ICanExecuteChangedCommand
        {
            readonly Manager manager;

            /// <summary>
            /// Execution group.
            /// </summary>
            public Group Group { get; }

            /// <summary>
            /// Original command.
            /// </summary>
            public ICommand OriginalCommand { get; }

            /// <summary>
            /// Constructor.
            /// </summary>
            public GroupCommand(
                Manager manager,
                Group group,
                ICommand originalCommand)
            {
                this.manager = manager;
                Group = group;
                OriginalCommand = originalCommand;

                OriginalCommand.CanExecuteChanged += (s, e) => RaiseCanExecuteChanged();
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
                    if (manager.settings.IgnoreExceptionsFromCommands)
                        return;

                    throw;
                }
            }

            /// <summary>
            /// Can execute.
            /// </summary>
            /// <param name="parameter">Command parameter.</param>
            /// <returns>True, if command can execute; false otherwise.</returns>
            public bool CanExecute(object parameter)
            {
                return manager.CanExecuteCommandFromGroup(this, parameter);
            }

            /// <summary>
            /// Execute command asynchronously.
            /// </summary>
            /// <param name="parameter">Command parameter.</param>
            /// <returns>Task.</returns>
            public Task ExecuteAsync(object parameter = null)
            {
                return manager.ExecuteCommandFromGroupAsync(this, parameter);
            }

            /// <summary>
            /// Implicit convertion to windows command.
            /// </summary>
            public static implicit operator ToWindowsCommand(GroupCommand command)
            {
                return new ToWindowsCommand(command);
            }
        }

        #endregion
    }
}