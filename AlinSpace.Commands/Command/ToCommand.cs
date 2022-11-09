using System;
using System.Windows.Input;

namespace AlinSpace.Commands
{
    /// <summary>
    /// <see cref="AsyncCommand"/> to <see cref="ICommand"/>.
    /// </summary>
    public class ToCommand : ICommand
    {
        private readonly IAsyncCommand command;
        private readonly bool fireAndForgetWhenExecuted;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="command">Asynchronous command.</param>
        /// <param name="fireAndForgetWhenExecuted">Flag indicates whether or not the asynchronous command shall be fire and forget when executed.</param>
        public ToCommand(IAsyncCommand command, bool fireAndForgetWhenExecuted = true)
        {
            this.command = command ?? throw new ArgumentNullException(nameof(command));
            this.fireAndForgetWhenExecuted = fireAndForgetWhenExecuted;

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
        /// Evaluates whether or not the command can execute asynchronously.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>True, if command can be executed; false otherwise.</returns>
        public bool CanExecute(object? parameter = null)
        {
            return command.CanExecute(parameter);
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        public void Execute(object? parameter = null)
        {
            var task = command.ExecuteAsync(parameter);

            if (!fireAndForgetWhenExecuted)
            {
                task.Wait();
            }
        }
    }
}
