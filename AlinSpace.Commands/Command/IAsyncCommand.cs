using System;
using System.Threading.Tasks;

namespace AlinSpace.Commands
{
    /// <summary>
    /// Represents an asynchronous command interface.
    /// </summary>
    public interface IAsyncCommand
    {
        /// <summary>
        /// Can execute changed.
        /// </summary>
        /// <remarks>
        /// Raised when <see cref="CanExecute(object)"/> changes.
        /// </remarks>
        event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        void RaiseCanExecuteChanged();

        /// <summary>
        /// Evaluates whether or not the command can execute asynchronously.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>True, if command can be executed; false otherwise.</returns>
        bool CanExecute(object? parameter = null);

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        Task ExecuteAsync(object? parameter = null);
    }
}
