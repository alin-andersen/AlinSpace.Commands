using System.Windows.Input;

namespace AlinSpace.Commands
{
    /// <summary>
    /// Extensions for <see cref="ICommand"/>.
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// Convert to <see cref="IAsyncCommand"/>.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <returns>Async command.</returns>
        public static IAsyncCommand ToAsyncCommand(this ICommand command)
        {
            return new CommandToAsyncCommand(command);
        }
    }
}
