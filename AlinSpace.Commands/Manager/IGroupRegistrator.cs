namespace AlinSpace.Commands
{
    /// <summary>
    /// Represents the group registrator.
    /// </summary>
    public interface IGroupRegistrator
    {
        /// <summary>
        /// Register command to the group.
        /// </summary>
        /// <param name="command">Command to register.</param>
        /// <returns>Registered command.</returns>
        IAsyncCommand Register(IAsyncCommand command);
    }
}
