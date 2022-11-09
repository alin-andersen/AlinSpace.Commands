using System.Threading.Tasks;
using System.Windows.Input;

namespace AlinSpace.Commands
{
    /// <summary>
    /// Extensions for <see cref="IAsyncCommand"/>.
    /// </summary>
    public static class AsyncCommandExtensions
    {
        /// <summary>
        /// Converts to <see cref="ICommand"/>.
        /// </summary>
        /// <param name="asyncCommand">The asynchronous command to convert.</param>
        /// <param name="fireAndForgetWhenExecuted">Flag indicates whether or not the asynchronous command shall be fire and forget when executed.</param>
        /// <returns>Command.</returns>
        public static ICommand ToCommand(this IAsyncCommand asyncCommand, bool fireAndForgetWhenExecuted = true)
        {
            return new ToCommand(asyncCommand, fireAndForgetWhenExecuted);
        }

        /// <summary>
        /// Safe execution of the asynchronous command.
        /// </summary>
        /// <param name="asyncCommand">Asynchronous command to execute safely.</param>
        /// <param name="parameter">Command parameter.</param>
        /// <param name="callCanExecuteBeforeExecution">Call CanExecute before command execution.</param>
        /// <param name="catchIgnoreExceptions">Catch and ignore any exceptions.</param>
        public static async Task SafeExecuteAsync(
            this IAsyncCommand asyncCommand,
            object? parameter = null,
            bool callCanExecuteBeforeExecution = true,
            bool catchIgnoreExceptions = true)
        {
            if (asyncCommand == null)
                return;

            try
            {
                if (callCanExecuteBeforeExecution)
                {
                    if (asyncCommand.CanExecute(parameter))
                        return;
                }

                await asyncCommand.ExecuteAsync(parameter);
            }
            catch
            {
                if (!catchIgnoreExceptions)
                    throw;
            }
        }

        /// <summary>
        /// Safe execution of the asynchronous command.
        /// </summary>
        /// <param name="command">Command to execute safely.</param>
        /// <param name="parameter">Command parameter.</param>
        /// <param name="callCanExecuteBeforeExecution">Call CanExecute before command execution.</param>
        /// <param name="catchIgnoreExceptions">Catch and ignore any exceptions.</param>
        public static void SafeExecute(
            ICommand command,
            object? parameter = null,
            bool callCanExecuteBeforeExecution = true,
            bool catchIgnoreExceptions = true)
        {
            if (command == null)
                return;

            try
            {
                if (callCanExecuteBeforeExecution)
                {
                    if (command.CanExecute(parameter))
                        return;
                }

                command.Execute(parameter);
            }
            catch
            {
                if (!catchIgnoreExceptions)
                    throw;
            }
        }
    }
}
