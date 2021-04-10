using System.Windows.Input;

namespace AlinSpace.FluentCommands
{
    /// <summary>
    /// Extensions for <see cref="ICommand"/>.
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// Convert to <see cref="IFluentCommand"/>.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <returns>Fluent command.</returns>
        public static IFluentCommand ToFluentCommand(this ICommand command)
        {
            return new CommandToFluentCommand(command);
        }
    }
}
