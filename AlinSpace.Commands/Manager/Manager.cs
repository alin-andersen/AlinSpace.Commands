using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlinSpace.Commands
{
    /// <summary>
    /// Default implementation of <see cref="IManager"/>.
    /// </summary>
    public partial class Manager : IManager
    {
        /// <summary>
        /// Settings.
        /// </summary>
        private readonly ManagerSettings settings = new ManagerSettings();

        /// <summary>
        /// Execution groups.
        /// </summary>
        private readonly IList<Group> executionGroups = new List<Group>();

        /// <summary>
        /// Internal spinlock.
        /// </summary>
        private readonly SpinLock spinlock = new SpinLock(false);

        /// <summary>
        /// Create command manager.
        /// </summary>
        /// <param name="settingsCallback">Optional settings callback.</param>
        /// <returns>Async command manager.</returns>
        public static Manager New(Action<ISettings> settingsCallback = null)
        {
            return new Manager(settingsCallback);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settingsCallback">Optional settings callback.</param>
        public Manager(Action<ISettings> settingsCallback = null)
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

            spinlock.Execute(() => executionGroups.Add(group));

            return this;
        }
    }
}