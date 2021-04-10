using System;
using System.Windows.Input;

namespace AlinSpace.FluentCommands
{
    /// <summary>
    /// <see cref="FluentCommand"/> to <see cref="ICommand"/>.
    /// </summary>
    public class FluentCommandToCommand : ICommand
    {
        readonly IFluentCommand command;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="command">Fluent command.</param>
        public FluentCommandToCommand(IFluentCommand command)
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
