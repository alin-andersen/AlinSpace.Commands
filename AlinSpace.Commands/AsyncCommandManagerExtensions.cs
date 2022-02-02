using System;

namespace AlinSpace.Commands
{
    /// <summary>
    /// Extensions for <see cref="IAsyncCommandManager"/>.
    /// </summary>
    public static class AsyncCommandManagerExtensions
    {
        /// <summary>
        /// When executing a command from this execution group lock all groups.
        /// </summary>
        /// <param name="commandManager">Command manager.</param>
        /// <param name="executionGroupCallback">Execution group callback.</param>
        /// <returns>Command manager.</returns>
        public static IAsyncCommandManager LockAll(this IAsyncCommandManager commandManager, Action<IAsyncCommandGroup> executionGroupCallback)
        {
            return commandManager.AddGroup(executionGroupCallback, GroupLockBehavior.LockAllGroups);
        }

        /// <summary>
        /// When executing a command from this execution group lock all other groups.
        /// </summary>
        /// <param name="commandManager">Command manager.</param>
        /// <param name="executionGroupCallback">Execution group callback.</param>
        /// <returns>Command manager.</returns>
        public static IAsyncCommandManager LockOthers(this IAsyncCommandManager commandManager, Action<IAsyncCommandGroup> executionGroupCallback)
        {
            return commandManager.AddGroup(executionGroupCallback, GroupLockBehavior.LockAllOtherGroups);
        }

        /// <summary>
        /// When executing a command from this execution group lock this group.
        /// </summary>
        /// <param name="commandManager">Command manager.</param>
        /// <param name="executionGroupCallback">Execution group callback.</param>
        /// <returns>Command manager.</returns>
        public static IAsyncCommandManager LockThis(this IAsyncCommandManager commandManager, Action<IAsyncCommandGroup> executionGroupCallback)
        {
            return commandManager.AddGroup(executionGroupCallback, GroupLockBehavior.LockThisGroup);
        }

        /// <summary>
        /// When executing a command from this execution group lock nothing.
        /// </summary>
        /// <param name="commandManager">Command manager.</param>
        /// <param name="executionGroupCallback">Execution group callback.</param>
        /// <returns>Command manager.</returns>
        public static IAsyncCommandManager LockNothing(this IAsyncCommandManager commandManager, Action<IAsyncCommandGroup> executionGroupCallback)
        {
            return commandManager.AddGroup(executionGroupCallback, GroupLockBehavior.LockNothing);
        }
    }
}
