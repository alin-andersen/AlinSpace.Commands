﻿using System;
using System.Windows.Input;

namespace AlinSpace.Exceptions
{
    /// <summary>
    /// <see cref="AsyncCommand"/> to <see cref="ICommand"/>.
    /// </summary>
    public class AsyncCommandToCommand : ICommand
    {
        private readonly IAsyncCommand command;
        private readonly bool fireAndForgetOnExecution;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="command">Async command.</param>
        /// <param name="fireAndForgetOnExecution">Flag indicates whether or not the async command shall be fire and forget on execution.</param>
        public AsyncCommandToCommand(IAsyncCommand command, bool fireAndForgetOnExecution = true)
        {
            this.command = command ?? throw new ArgumentNullException(nameof(command));
            this.fireAndForgetOnExecution = fireAndForgetOnExecution;

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
            var task = command.ExecuteAsync(parameter);

            if (!fireAndForgetOnExecution)
                task.Wait();
        }
    }
}