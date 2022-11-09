using System.Windows.Input;

namespace AlinSpace.Commands
{
    /// <summary>
    /// Extensions for <see cref="ICommand"/>.
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// Converts <see cref="ICommand"/> to <see cref="IAsyncCommand"/>.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <returns>Asynchronous command.</returns>
        public static IAsyncCommand ToAsyncCommand(this ICommand command)
        {
            return new ToAsyncCommand(command);
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
