namespace AlinSpace.Command
{
    /// <summary>
    /// Extensions for <see cref="IGroup"/>.
    /// </summary>
    public static class GroupExtensions
    {
        /// <summary>
        /// Register command.
        /// </summary>
        /// <param name="registrator">Group registrator.</param>
        /// <param name="command">Command to register.</param>
        public static void Register(this IGroupRegistrator registrator, ref ICommand command)
        {
            command = registrator.Register(command);
        }
    }
}
