using System;

namespace FluentCommands
{
    /// <summary>
    /// Extensions for <see cref="ICommandManager"/>.
    /// </summary>
    public static class CommandManagerExtensions
    {
        /// <summary>
        /// When executing a command from this execution group lock all groups.
        /// </summary>
        /// <param name="commandManager">Command manager.</param>
        /// <param name="executionGroupCallback">Execution group callback.</param>
        /// <returns>Command manager.</returns>
        public static ICommandManager LockAll(this ICommandManager commandManager, Action<IExecutionGroup> executionGroupCallback)
        {
            return commandManager.AddGroup(executionGroupCallback, ExecutionLock.LockAllGroups);
        }

        /// <summary>
        /// When executing a command from this execution group lock all others groups.
        /// </summary>
        /// <param name="commandManager">Command manager.</param>
        /// <param name="executionGroupCallback">Execution group callback.</param>
        /// <returns>Command manager.</returns>
        public static ICommandManager LockOtherGroups(this ICommandManager commandManager, Action<IExecutionGroup> executionGroupCallback)
        {
            return commandManager.AddGroup(executionGroupCallback, ExecutionLock.LockAllOthersGroups);
        }

        /// <summary>
        /// When executing a command from this execution group lock this group.
        /// </summary>
        /// <param name="commandManager">Command manager.</param>
        /// <param name="executionGroupCallback">Execution group callback.</param>
        /// <returns>Command manager.</returns>
        public static ICommandManager LockThisGroup(this ICommandManager commandManager, Action<IExecutionGroup> executionGroupCallback)
        {
            return commandManager.AddGroup(executionGroupCallback, ExecutionLock.LockThisGroup);
        }
    }
}
