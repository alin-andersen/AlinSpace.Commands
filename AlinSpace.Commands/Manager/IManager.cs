using System;

namespace AlinSpace.Commands
{
    /// <summary>
    /// Represents the manager interface.
    /// </summary>
    public interface IManager
    {
        /// <summary>
        /// Add group.
        /// </summary>
        /// <param name="commandRegistrationsDelegate">Command registrations delegate.</param>
        /// <param name="lock">Lock.</param>
        /// <returns>Command manager.</returns>
        IManager AddGroup(Action<IGroupRegistrator> commandRegistrationsDelegate, GroupLockBehavior @lock = GroupLockBehavior.LockAllGroups);
    }
}
