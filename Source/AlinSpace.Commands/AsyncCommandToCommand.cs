using System;
using System.Windows.Input;

namespace AlinSpace.Exceptions
{
    /// <summary>
    /// <see cref="AsyncCommand"/> to <see cref="ICommand"/>.
    /// </summary>
    public class AsyncCommandToCommand : ICommand
    {
        readonly IAsyncCommand command;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="command">Async command.</param>
        public AsyncCommandToCommand(IAsyncCommand command)
        {
            this.command = command ?? throw new ArgumentNullException(nameof(command));
            command.CanExecuteChanged += (sender, args) => CanExecuteChanged(sender, args);
        }

        /// <summary>
        /// Can execute changed.
        /// </summary>
        /// <remarks>
        /// Raised when <see cref="CanExecute(object)"/> changes.
        /// </remarks>
        public event EventHandler CanExecuteChanged = delegate { };

        /// <summary>
        /// Can command execute.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>True, if command can be executed; false otherwise.</returns>
        public bool CanExecute(object parameter = null)
        {
            return command.CanExecute(parameter);
        }

        /// <summary>
        /// Execute command.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        public void Execute(object parameter = null)
        {
            var _ = command.ExecuteAsync(parameter);
        }
    }
}
