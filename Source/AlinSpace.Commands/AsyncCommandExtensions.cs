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
        /// <param name="AsyncCommand">Async command.</param>
        /// <returns>Command.</returns>
        public static ICommand ToCommand(this IAsyncCommand AsyncCommand)
        {
            return new AsyncCommandToCommand(AsyncCommand);
        }
    }
}
