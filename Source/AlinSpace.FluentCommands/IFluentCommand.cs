using System;
using System.Threading.Tasks;

namespace AlinSpace.FluentCommands
{
    /// <summary>
    /// Fluent command interface.
    /// </summary>
    public interface IFluentCommand
    {
        /// <summary>
        /// Can execute changed.
        /// </summary>
        /// <remarks>
        /// Raised when <see cref="CanExecute(object)"/> changes.
        /// </remarks>
        event EventHandler CanExecuteChanged;

        /// <summary>
        /// Can command execute.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>True, if command can be executed; false otherwise.</returns>
        bool CanExecute(object parameter = null);

        /// <summary>
        /// Execute command asynchronously.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>Task.</returns>
        Task ExecuteAsync(object parameter = null);
    }
}
