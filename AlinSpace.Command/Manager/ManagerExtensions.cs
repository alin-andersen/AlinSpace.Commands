using System;

namespace AlinSpace.Command
{
    /// <summary>
    /// Extensions for <see cref="IAsyncCommandManager"/>.
    /// </summary>
    public static class ManagerExtensions
    {
        /// <summary>
        /// When executing a command from this group lock all groups.
        /// </summary>
        /// <param name="manager">Command manager.</param>
        /// <param name="commandRegistrations">Command registrations.</param>
        /// <returns>Command manager.</returns>
        public static IManager LockAll(this IManager manager, Action<IGroupRegistrator> commandRegistrations)
        {
            return manager.AddGroup(commandRegistrations, GroupLockBehavior.LockAllGroups);
        }

        /// <summary>
        /// When executing a command from this group lock all other groups.
        /// </summary>
        /// <param name="manager">Command manager.</param>
        /// <param name="commandRegistrations">Command registrations.</param>
        /// <returns>Command manager.</returns>
        public static IManager LockOthers(this IManager manager, Action<IGroupRegistrator> commandRegistrations)
        {
            return manager.AddGroup(commandRegistrations, GroupLockBehavior.LockAllOtherGroups);
        }

        /// <summary>
        /// When executing a command from this group lock this group.
        /// </summary>
        /// <param name="manager">Command manager.</param>
        /// <param name="commandRegistrations">Command registrations.</param>
        /// <returns>Command manager.</returns>
        public static IManager LockThis(this IManager manager, Action<IGroupRegistrator> commandRegistrations)
        {
            return manager.AddGroup(commandRegistrations, GroupLockBehavior.LockThisGroup);
        }

        /// <summary>
        /// When executing a command from this group lock nothing.
        /// </summary>
        /// <param name="manager">Command manager.</param>
        /// <param name="commandRegistrations">Command registrations.</param>
        /// <returns>Command manager.</returns>
        public static IManager LockNothing(this IManager manager, Action<IGroupRegistrator> commandRegistrations)
        {
            return manager.AddGroup(commandRegistrations, GroupLockBehavior.LockNothing);
        }
    }
}
