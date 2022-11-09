using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlinSpace.Commands
{
    /// <summary>
    /// Default implementation of <see cref="IAsyncManager"/>.
    /// </summary>
    public partial class AsyncManager : IAsyncManager
    {
        private readonly AsyncManagerSettings settings;
        private readonly IList<Group> executionGroups = new List<Group>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">Settings.</param>
        public AsyncManager(AsyncManagerSettings? settings = null)
        {
            this.settings = settings ?? new AsyncManagerSettings();
        }

        /// <summary>
        /// Creates a new command manager.
        /// </summary>
        /// <param name="settings">Settings.</param>
        /// <returns>Asynchronous command manager.</returns>
        public static AsyncManager New(AsyncManagerSettings? settings = null)
        {
            return new AsyncManager(settings);
        }

        /// <summary>
        /// Adds the execution group.
        /// </summary>
        /// <param name="commandRegistrationsDelegate">Command registrations delegate.</param>
        /// <param name="lock">Lock.</param>
        /// <returns>Asynchronous command manager.</returns>
        public IAsyncManager AddGroup(Action<IGroupRegistrator> commandRegistrationsDelegate, GroupLockBehavior @lock = GroupLockBehavior.LockAllGroups)
        {
            var group = new Group(
                manager: this,
                @lock: @lock);

            commandRegistrationsDelegate(group);

            executionGroups.Add(group);
            return this;
        }

        #region Internal

        /// <summary>
        /// Get groups to lock based on the given group.
        /// </summary>
        /// <param name="group">Group the locking is based on.</param>
        /// <returns>Enumerable of groups that shall be locked.</returns>
        IEnumerable<Group> GetGroupsToLock(Group group)
        {
            if (group.Lock == GroupLockBehavior.LockNothing)
                return new Group[] { };

            if (group.Lock == GroupLockBehavior.LockThisGroup)
                return new Group[] { group };

            var groupsToLock = new List<Group>();

            foreach (var executionGroup in executionGroups)
            {
                if (ReferenceEquals(executionGroup, group))
                {
                    if (group.Lock == GroupLockBehavior.LockAllOtherGroups)
                        continue;
                }

                groupsToLock.Add(executionGroup);
            }

            return groupsToLock;
        }

        #region Locking / Unlocking

        /// <summary>
        /// Locks the group and the affected groups.
        /// </summary>
        /// <param name="group">Group to lock.</param>
        void LockGroupAndAffectedGroups(Group group)
        {
            var groups = GetGroupsToLock(group);

            groups.ForEach(x => Interlocked.Increment(ref x.LockedCounter));
            groups.ForEach(x => x.RaiseCanExecuteChangedForAllCommands());
        }

        /// <summary>
        /// Unlocks the group and the affected groups.
        /// </summary>
        /// <param name="group">Group to unlock.</param>
        void UnlockGroupAndAffectedGroups(Group group)
        {
            var groups = GetGroupsToLock(group);

            groups.ForEach(x => Interlocked.Decrement(ref x.LockedCounter));
            groups.ForEach(x => x.RaiseCanExecuteChangedForAllCommands());
        }

        #endregion

        void RaiseCanExecuteChangeForAllCommands()
        {
            foreach(var executionGroup in executionGroups)
            {
                executionGroup.RaiseCanExecuteChangedForAllCommands();
            }
        }

        #region CanExecute / Execute

        bool CanExecuteCommandFromGroup(GroupAsyncCommand command, object? parameter)
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

        async Task ExecuteCommandFromGroupAsync(GroupAsyncCommand command, object? parameter)
        {
            if (settings.VerifyCanExecuteBeforeExecution)
            {
                if (!command.OriginalCommand.CanExecute(parameter))
                    return;
            }

            try
            {
                LockGroupAndAffectedGroups(command.Group);

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
                if (settings.RaiseCanExecuteChangedOnAllCommandsAfterAnyCommandExecution)
                {
                    RaiseCanExecuteChangeForAllCommands();
                }

                UnlockGroupAndAffectedGroups(command.Group);
            }
        }

        #endregion

        /// <summary>
        /// Represents the execution group.
        /// </summary>
        class Group : IGroupRegistrator
        {
            private readonly AsyncManager manager;

            public GroupLockBehavior Lock { get; }

            public int LockedCounter = 0;

            public IList<IAsyncCommand> Commands = new List<IAsyncCommand>();

            internal Group(AsyncManager manager, GroupLockBehavior @lock)
            {
                this.manager = manager;
                Lock = @lock;
            }

            public IAsyncCommand Register(IAsyncCommand command)
            {
                var groupCommand = new GroupAsyncCommand(
                    manager: manager,
                    group: this,
                    originalCommand: command);

                Commands.Add(groupCommand);
                return groupCommand;
            }

            public void RaiseCanExecuteChangedForAllCommands()
            {
                foreach (var executionGroupCommand in Commands)
                {
                    try
                    {
                        executionGroupCommand.RaiseCanExecuteChanged();
                    }
                    catch (Exception)
                    {
                        if (manager.settings.IgnoreExceptionsFromCommands)
                            return;

                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Group command.
        /// </summary>
        class GroupAsyncCommand : IAsyncCommand
        {
            private readonly AsyncManager manager;

            public Group Group;

            public IAsyncCommand OriginalCommand;

            public GroupAsyncCommand(
                AsyncManager manager,
                Group group,
                IAsyncCommand originalCommand)
            {
                this.manager = manager;
                Group = group;
                OriginalCommand = originalCommand;

                OriginalCommand.CanExecuteChanged += (s, e) => RaiseCanExecuteChanged();
            }

            public event EventHandler? CanExecuteChanged;

            public void RaiseCanExecuteChanged()
            {
                try
                {
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception)
                {
                    if (manager.settings.IgnoreExceptionsFromCommands)
                        return;

                    throw;
                }
            }

            public bool CanExecute(object? parameter = null)
            {
                return manager.CanExecuteCommandFromGroup(this, parameter);
            }

            public Task ExecuteAsync(object? parameter = null)
            {
                return manager.ExecuteCommandFromGroupAsync(this, parameter);
            }

            public static implicit operator ToCommand(GroupAsyncCommand command)
            {
                return new ToCommand(command);
            }
        }

        #endregion
    }
}