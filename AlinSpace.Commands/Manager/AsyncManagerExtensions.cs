using System;

namespace AlinSpace.Commands
{
    /// <summary>
    /// Extensions for <see cref="IAsyncCommandManager"/>.
    /// </summary>
    public static class AsyncManagerExtensions
    {
        /// <summary>
        /// When executing a command from this group lock all groups.
        /// </summary>
        /// <param name="manager">Command manager.</param>
        /// <param name="commandRegistrations">Command registrations.</param>
        /// <returns>Command manager.</returns>
        public static IAsyncManager LockAll(this IAsyncManager manager, Action<IGroupRegistrator> commandRegistrations)
        {
            return manager.AddGroup(commandRegistrations, GroupLockBehavior.LockAllGroups);
        }

        /// <summary>
        /// When executing a command from this group lock all other groups.
        /// </summary>
        /// <param name="manager">Command manager.</param>
        /// <param name="commandRegistrations">Command registrations.</param>
        /// <returns>Command manager.</returns>
        public static IAsyncManager LockOthers(this IAsyncManager manager, Action<IGroupRegistrator> commandRegistrations)
        {
            return manager.AddGroup(commandRegistrations, GroupLockBehavior.LockAllOtherGroups);
        }

        /// <summary>
        /// When executing a command from this group lock this group.
        /// </summary>
        /// <param name="manager">Command manager.</param>
        /// <param name="commandRegistrations">Command registrations.</param>
        /// <returns>Command manager.</returns>
        public static IAsyncManager LockThis(this IAsyncManager manager, Action<IGroupRegistrator> commandRegistrations)
        {
            return manager.AddGroup(commandRegistrations, GroupLockBehavior.LockThisGroup);
        }

        /// <summary>
        /// When executing a command from this group lock nothing.
        /// </summary>
        /// <param name="manager">Command manager.</param>
        /// <param name="commandRegistrations">Command registrations.</param>
        /// <returns>Command manager.</returns>
        public static IAsyncManager LockNothing(this IAsyncManager manager, Action<IGroupRegistrator> commandRegistrations)
        {
            return manager.AddGroup(commandRegistrations, GroupLockBehavior.LockNothing);
        }
    }
}
