using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AlinSpace.Exceptions
{
    /// <summary>
    /// <see cref="ICommand"/> to <see cref="IAsyncCommand"/>.
    /// </summary>
    public class CommandToAsyncCommand : IAsyncCommand
    {
        readonly ICommand command;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="command">Command.</param>
        public CommandToAsyncCommand(ICommand command)
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
        /// Execute command asynchronously.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>Task.</returns>
        public Task ExecuteAsync(object parameter = null)
        {
            command.Execute(parameter);
            return Task.CompletedTask;
        }
    }
}
