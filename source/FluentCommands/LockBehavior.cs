namespace FluentCommands
{
    /// <summary>
    /// Execution behaviour.
    /// </summary>
    public enum LockBehavior
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
