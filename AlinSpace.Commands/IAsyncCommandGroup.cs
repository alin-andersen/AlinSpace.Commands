namespace AlinSpace.Exceptions
{
    /// <summary>
    /// Asynchronous command group interface.
    /// </summary>
    public interface IAsyncCommandGroup
    {
        /// <summary>
        /// Register command to the async command group.
        /// </summary>
        /// <param name="command">Command to register.</param>
        /// <returns>Registered async command.</returns>
        IAsyncCommand Register(IAsyncCommand command);
    }
}
