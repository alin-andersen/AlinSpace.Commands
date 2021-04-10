using System.Windows.Input;

namespace AlinSpace.FluentCommands
{
    /// <summary>
    /// Extensions for <see cref="IFluentCommand"/>.
    /// </summary>
    public static class FluentCommandExtensions
    {
        /// <summary>
        /// Convert to <see cref="ICommand"/>.
        /// </summary>
        /// <param name="fluentCommand">Fluent command.</param>
        /// <returns>Command.</returns>
        public static ICommand ToCommand(this IFluentCommand fluentCommand)
        {
            return new FluentCommandToCommand(fluentCommand);
        }
    }
}
