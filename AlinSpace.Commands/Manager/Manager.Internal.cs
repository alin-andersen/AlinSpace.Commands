using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlinSpace.Commands
{
    public partial class Manager
    {
        class ManagerSettings : ISettings
        {
            public bool RaiseCanExecuteAfterExecutionForAllGroups { get; set; } = Constants.RaiseCanExecuteAfterExecutionForAllCommands;
            public bool VerifyCanExecuteBeforeExecution { get; set; } = Constants.VerifyCanExecuteBeforeExecution;
            public bool IgnoreIndividualCanExecute { get; set; } = Constants.IgnoreIndividualCanExecute;
            public bool IgnoreExceptionsFromCommands { get; set; } = Constants.IgnoreExceptionsFromCommands;
            public bool ContinueOnCapturedContext { get; set; } = Constants.ContinueOnCapturedContext;
        }

        /// <summary>
        /// Get the affected groups of the given group. 
        /// </summary>
        /// <param name="group">Group.</param>
        /// <returns>Enumerable of affected groups.</returns>
        IEnumerable<Group> GetAffectedGroups(Group group)
        {
            if (group.Lock == GroupLockBehavior.LockNothing)
                return new Group[] { };

            if (group.Lock == GroupLockBehavior.LockThisGroup)
                return new Group[] { group };

            if (group.Lock == GroupLockBehavior.LockAllOtherGroups)
            {
                return executionGroups
                    .Where(x => !ReferenceEquals(x, group))
                    .ToList();
            }

            return executionGroups.ToList();
        }

        #region Locking / Unlocking

        /// <summary>
        /// Tries to lock the group.
        /// </summary>
        /// <param name="group">Group to lock.</param>
        /// <remarks>True, locking was successful; false otherwise.</remarks>
        bool TryLockGroup(Group group)
        {
            IEnumerable<Group> affectedGroups = null;

            var didLockGroup = spinlock.Execute(() =>
            {
                // If the current group is locked, return.
                if (group.LockedCounter > 0)
                    return false;

                // Get all affected groups required to be locked.
                affectedGroups = GetAffectedGroups(group);

                // Incremet counters.
                affectedGroups.ForEach(x => x.LockedCounter += 1);

                return true;
            });

            if (!didLockGroup)
            {
                return false;
            }

            // Raise CanExecuteChanged for all affected groups.
            affectedGroups.ForEach(x => RaiseCanExecuteChangedForExecutionGroup(x));
            
            return true;
        }

        /// <summary>
        /// Unlock group.
        /// </summary>
        /// <param name="group">Group to unlock.</param>
        void UnlockGroup(Group group)
        {
            IEnumerable<Group> affectedGroups = null;

            spinlock.Execute(() =>
            {
                // Get all affected groups required to be unlocked.
                affectedGroups = GetAffectedGroups(group);

                // Decrement counters.
                affectedGroups.ForEach(x => x.LockedCounter -= 1);
            });

            if (settings.RaiseCanExecuteAfterExecutionForAllGroups)
            {
                var allGroups = spinlock.Execute(() => executionGroups.ToList());
                allGroups.ForEach(x => RaiseCanExecuteChangedForExecutionGroup(x));
            }
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
            // Check if execution group allows the execution.
            var canExecute = spinlock.Execute(() => command.Group.LockedCounter == 0);
            if (!canExecute)
                return false;

            // Check if we should ignore the individual can execute method.
            if (settings.IgnoreIndividualCanExecute)
                return true;

            // Call can execute of the command.
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
                try
                {
                    if (!command.OriginalCommand.CanExecute(parameter))
                        return;
                }
                catch (Exception)
                {
                    if (settings.IgnoreExceptionsFromCommands)
                        return;

                    throw;
                }
            }

            try
            {
                if (!TryLockGroup(command.Group))
                    return;

                await command
                    .OriginalCommand
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
        private class Group : IGroupRegistrator
        {
            private readonly Manager manager;

            public GroupLockBehavior Lock { get; }

            public int LockedCounter = 0;

            public IList<ICanExecuteChangedCommand> Commands = new List<ICanExecuteChangedCommand>();

            public Group(Manager manager, GroupLockBehavior @lock)
            {
                this.manager = manager;
                Lock = @lock;
            }

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

    }
}
