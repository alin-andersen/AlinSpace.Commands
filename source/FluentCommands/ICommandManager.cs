using System;

namespace FluentCommands
{
    /// <summary>
    /// Command manager interface.
    /// </summary>
    public interface ICommandManager
    {
        /// <summary>
        /// Add execution group.
        /// </summary>
        /// <param name="exectionGroupCallback">Execution group callback.</param>
        /// <param name="lock">Lock.</param>
        /// <returns>Command manager.</returns>
        ICommandManager AddGroup(Action<IExecutionGroup> exectionGroupCallback, ExecutionLock @lock = ExecutionLock.LockAllGroups);
    }
}
