using System;

namespace AlinSpace.FluentCommands
{
    /// <summary>
    /// Extensions for <see cref="IFluentCommandManager"/>.
    /// </summary>
    public static class FluentCommandManagerExtensions
    {
        /// <summary>
        /// When executing a command from this execution group lock all groups.
        /// </summary>
        /// <param name="commandManager">Command manager.</param>
        /// <param name="executionGroupCallback">Execution group callback.</param>
        /// <returns>Command manager.</returns>
        public static IFluentCommandManager LockAll(this IFluentCommandManager commandManager, Action<IExecutionGroup> executionGroupCallback)
        {
            return commandManager.AddGroup(executionGroupCallback, LockBehavior.LockAllGroups);
        }

        /// <summary>
        /// When executing a command from this execution group lock all other groups.
        /// </summary>
        /// <param name="commandManager">Command manager.</param>
        /// <param name="executionGroupCallback">Execution group callback.</param>
        /// <returns>Command manager.</returns>
        public static IFluentCommandManager LockOthers(this IFluentCommandManager commandManager, Action<IExecutionGroup> executionGroupCallback)
        {
            return commandManager.AddGroup(executionGroupCallback, LockBehavior.LockAllOtherGroups);
        }

        /// <summary>
        /// When executing a command from this execution group lock this group.
        /// </summary>
        /// <param name="commandManager">Command manager.</param>
        /// <param name="executionGroupCallback">Execution group callback.</param>
        /// <returns>Command manager.</returns>
        public static IFluentCommandManager LockThis(this IFluentCommandManager commandManager, Action<IExecutionGroup> executionGroupCallback)
        {
            return commandManager.AddGroup(executionGroupCallback, LockBehavior.LockThisGroup);
        }

        /// <summary>
        /// When executing a command from this execution group lock nothing.
        /// </summary>
        /// <param name="commandManager">Command manager.</param>
        /// <param name="executionGroupCallback">Execution group callback.</param>
        /// <returns>Command manager.</returns>
        public static IFluentCommandManager LockNothing(this IFluentCommandManager commandManager, Action<IExecutionGroup> executionGroupCallback)
        {
            return commandManager.AddGroup(executionGroupCallback, LockBehavior.LockNothing);
        }
    }
}
