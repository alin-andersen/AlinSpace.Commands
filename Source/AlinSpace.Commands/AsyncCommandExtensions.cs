using System.Threading.Tasks;
using System.Windows.Input;

namespace AlinSpace.Exceptions
{
    /// <summary>
    /// Extensions for <see cref="IAsyncCommand"/>.
    /// </summary>
    public static class AsyncCommandExtensions
    {
        /// <summary>
        /// Convert to <see cref="ICommand"/>.
        /// </summary>
        /// <param name="asyncCommand">Async command to convert.</param>
        /// <returns>Command.</returns>
        public static ICommand ToCommand(this IAsyncCommand asyncCommand)
        {
            return new AsyncCommandToCommand(asyncCommand);
        }

        /// <summary>
        /// Save execute command asynchronously.
        /// </summary>
        /// <param name="asyncCommand">Async command to execute safely.</param>
        /// <param name="parameter">Command parameter.</param>
        /// <param name="callCanExecuteBeforeExecution">Call CanExecute before command execution.</param>
        /// <param name="ignoreExceptions">Ignore exceptions from command.</param>
        public static async Task SafeExecuteAsync(
            this IAsyncCommand asyncCommand,
            object parameter = null,
            bool callCanExecuteBeforeExecution = true,
            bool ignoreExceptions = true)
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
                if (!ignoreExceptions)
                    throw;
            }
        }

        /// <summary>
        /// Save execute command.
        /// </summary>
        /// <param name="command">Command to execute safely.</param>
        /// <param name="parameter">Command parameter.</param>
        /// <param name="callCanExecuteBeforeExecution">Call CanExecute before command execution.</param>
        /// <param name="ignoreExceptions">Ignore exceptions from command.</param>
        public static void SafeExecute(
            this ICommand command,
            object parameter = null,
            bool callCanExecuteBeforeExecution = true,
            bool ignoreExceptions = true)
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
                if (!ignoreExceptions)
                    throw;
            }
        }
    }
}
