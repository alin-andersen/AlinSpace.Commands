using System;
using System.Threading.Tasks;

namespace FluentCommands
{
    /// <summary>
    /// Abstract implementation of <see cref="IFluentCommand"/>.
    /// </summary>
    public abstract class AbstractFluentCommand : IFluentCommand
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

    /// <summary>
    /// Abstract implementation for <see cref="IFluentCommand{TParameter}"/>.
    /// </summary>
    /// <typeparam name="TParameter">Type of command parameter.</typeparam>
    public abstract class AbstractFluentCommand<TParameter> : IFluentCommand<TParameter>
    {
        /// <summary>
        /// Can execute changed.
        /// </summary>
        /// <remarks>
        /// Raised when <see cref="CanExecute(TParameter)"/> changes.
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
        public virtual bool CanExecute(TParameter parameter = default)
        {
            return true;
        }

        /// <summary>
        /// Execute command asynchronously.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>Task.</returns>
        public abstract Task ExecuteAsync(TParameter parameter = default);
    }
}
