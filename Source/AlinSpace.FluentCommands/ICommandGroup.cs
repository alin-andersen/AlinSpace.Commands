using System.Windows.Input;

namespace AlinSpace.FluentCommands
{
    /// <summary>
    /// Command group interface.
    /// </summary>
    public interface ICommandGroup
    {
        /// <summary>
        /// Register command to the command group.
        /// </summary>
        /// <param name="command">Command to register.</param>
        /// <returns>Registered fluent command.</returns>
        IFluentCommand Register(IFluentCommand command);
    }
}
