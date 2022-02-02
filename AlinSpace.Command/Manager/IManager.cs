using System;

namespace AlinSpace.Command
{
    /// <summary>
    /// Represents the commang manager interface.
    /// </summary>
    public interface IManager
    {
        /// <summary>
        /// Add group.
        /// </summary>
        /// <param name="commandRegistrations">Command registrations delegate.</param>
        /// <param name="lock">Lock.</param>
        /// <returns>Command manager.</returns>
        IManager AddGroup(Action<IGroupRegistrator> commandRegistrations, GroupLockBehavior @lock = GroupLockBehavior.LockAllGroups);
    }
}
