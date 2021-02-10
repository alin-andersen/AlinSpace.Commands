using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommands
{
    /// <summary>
    /// Execution lock.
    /// </summary>
    public enum ExecutionLock
    {
        /// <summary>
        /// Lock all groups.
        /// </summary>
        LockAllGroups,

        /// <summary>
        /// Lock all other groups.
        /// </summary>
        LockAllOthersGroups,

        /// <summary>
        /// Lock this group.
        /// </summary>
        LockThisGroup,
    }
}
