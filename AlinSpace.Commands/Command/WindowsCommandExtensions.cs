namespace AlinSpace.Commands
{
    /// <summary>
    /// Extensions for <see cref="System.Windows.Input.ICommand"/>.
    /// </summary>
    public static class WindowsCommandExtensions
    {
        /// <summary>
        /// Convert to <see cref="ICommand"/>.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <returns>Async command.</returns>
        public static ICommand ToCommand(this global::System.Windows.Input.ICommand command)
        {
            return new ToCommand(command);
        }
    }
}
