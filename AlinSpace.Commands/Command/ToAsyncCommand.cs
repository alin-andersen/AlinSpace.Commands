using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AlinSpace.Commands
{
    /// <summary>
    /// <see cref="ICommand"/> to <see cref="IAsyncCommand"/>.
    /// </summary>
    public class ToAsyncCommand : IAsyncCommand
    {
        private readonly ICommand command;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="command">Command.</param>
        public ToAsyncCommand(ICommand command)
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
        public bool CanExecute(object? parameter = null)
        {
            return command.CanExecute(parameter);
        }

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        public Task ExecuteAsync(object? parameter = null)
        {
            command.Execute(parameter);
            return Task.CompletedTask;
        }
    }
}
