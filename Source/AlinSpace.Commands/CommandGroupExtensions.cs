namespace AlinSpace.Exceptions
{
    /// <summary>
    /// Extensions for <see cref="IAsyncCommandGroup"/>.
    /// </summary>
    public static class CommandGroupExtensions
    {
        /// <summary>
        /// Register command.
        /// </summary>
        /// <param name="group">Command group.</param>
        /// <param name="command">Command.</param>
        public static void Register(IAsyncCommandGroup group, ref IAsyncCommand command)
        {
            command = group.Register(command);
        }
    }
}
