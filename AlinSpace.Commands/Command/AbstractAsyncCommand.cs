using System;
using System.Threading.Tasks;

namespace AlinSpace.Commands
{
    /// <summary>
    /// Abstract implementation of the <see cref="IAsyncCommand"/> interface.
    /// </summary>
    public abstract class AbstractAsyncCommand : IAsyncCommand
    {
        /// <summary>
        /// Can execute changed.
        /// </summary>
        /// <remarks>
        /// Raised when <see cref="CanExecute(object)"/> changes.
        /// </remarks>
        public event EventHandler? CanExecuteChanged = delegate { };

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Evaluates whether or not the command can execute asynchronously.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>True, if command can be executed; false otherwise.</returns>
        public virtual bool CanExecute(object? parameter = null)
        {
            return true;
        }

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        public abstract Task ExecuteAsync(object? parameter = null);
    }
}
