namespace AlinSpace.FluentCommands
{
    /// <summary>
    /// Extensions for <see cref="ICommandGroup"/>.
    /// </summary>
    public static class CommandGroupExtensions
    {
        /// <summary>
        /// Register command.
        /// </summary>
        /// <param name="group">Command group.</param>
        /// <param name="command">Command.</param>
        public static void Register(ICommandGroup group, ref IFluentCommand command)
        {
            command = group.Register(command);
        }
    }
}
