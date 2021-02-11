using System;

namespace FluentCommands
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
            return commandManager.AddGroup(executionGroupCallback, LockBehaviour.LockAllGroups);
        }

        /// <summary>
        /// When executing a command from this execution group lock all other groups.
        /// </summary>
        /// <param name="commandManager">Command manager.</param>
        /// <param name="executionGroupCallback">Execution group callback.</param>
        /// <returns>Command manager.</returns>
        public static IFluentCommandManager LockOthers(this IFluentCommandManager commandManager, Action<IExecutionGroup> executionGroupCallback)
        {
            return commandManager.AddGroup(executionGroupCallback, LockBehaviour.LockAllOthersGroups);
        }

        /// <summary>
        /// When executing a command from this execution group lock this group.
        /// </summary>
        /// <param name="commandManager">Command manager.</param>
        /// <param name="executionGroupCallback">Execution group callback.</param>
        /// <returns>Command manager.</returns>
        public static IFluentCommandManager LockThis(this IFluentCommandManager commandManager, Action<IExecutionGroup> executionGroupCallback)
        {
            return commandManager.AddGroup(executionGroupCallback, LockBehaviour.LockThisGroup);
        }
    }
}
