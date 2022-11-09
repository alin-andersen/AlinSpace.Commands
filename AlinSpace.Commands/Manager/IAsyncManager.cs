using System;

namespace AlinSpace.Commands
{
    /// <summary>
    /// Represents the manager interface.
    /// </summary>
    public interface IAsyncManager
    {
        /// <summary>
        /// Add group.
        /// </summary>
        /// <param name="commandRegistrationsDelegate">Command registrations delegate.</param>
        /// <param name="lock">Lock.</param>
        /// <returns>Command manager.</returns>
        IAsyncManager AddGroup(Action<IGroupRegistrator> commandRegistrationsDelegate, GroupLockBehavior @lock = GroupLockBehavior.LockAllGroups);
    }
}
