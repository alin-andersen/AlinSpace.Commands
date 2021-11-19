namespace AlinSpace.Exceptions
{
    /// <summary>
    /// Group lock behaviour.
    /// </summary>
    /// <remarks>
    /// The locking behavior when a command of this group is executed.
    /// </remarks>
    public enum GroupLockBehavior
    {
        /// <summary>
        /// Lock all groups.
        /// </summary>
        LockAllGroups,

        /// <summary>
        /// Lock all other groups.
        /// </summary>
        LockAllOtherGroups,

        /// <summary>
        /// Lock this group.
        /// </summary>
        LockThisGroup,

        /// <summary>
        /// Lock nothing.
        /// </summary>
        LockNothing,
    }
}
