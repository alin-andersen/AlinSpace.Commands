using System;

namespace AlinSpace.Commands
{
    /// <summary>
    /// Async command manager interface.
    /// </summary>
    public interface IAsyncCommandManager
    {
        /// <summary>
        /// Add execution group.
        /// </summary>
        /// <param name="exectionGroupCallback">Execution group callback.</param>
        /// <param name="lock">Lock.</param>
        /// <returns>Command manager.</returns>
        IAsyncCommandManager AddGroup(Action<IAsyncCommandGroup> exectionGroupCallback, GroupLockBehavior @lock = GroupLockBehavior.LockAllGroups);
    }
}
