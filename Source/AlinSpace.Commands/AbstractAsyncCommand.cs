using System;
using System.Threading.Tasks;

namespace AlinSpace.Exceptions
{
    /// <summary>
    /// Abstract implementation of <see cref="IAsyncCommand"/>.
    /// </summary>
    public abstract class AbstractAsyncCommand : IAsyncCommand
    {
        /// <summary>
        /// Can execute changed.
        /// </summary>
        /// <remarks>
        /// Raised when <see cref="CanExecute(object)"/> changes.
        /// </remarks>
        public event EventHandler CanExecuteChanged = delegate { };

        /// <summary>
        /// Raises <see cref="CanExecuteChanged"/>.
        /// </summary>
        protected void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Can command execute.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>True, if command can be executed; false otherwise.</returns>
        public virtual bool CanExecute(object parameter = null)
        {
            return true;
        }

        /// <summary>
        /// Execute command asynchronously.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>Task.</returns>
        public abstract Task ExecuteAsync(object parameter = null);
    }
}
