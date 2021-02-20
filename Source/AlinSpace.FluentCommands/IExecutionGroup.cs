using System.Windows.Input;

namespace AlinSpace.FluentCommands
{
    /// <summary>
    /// Execution group interface.
    /// </summary>
    public interface IExecutionGroup
    {
        /// <summary>
        /// Register command to the execution group.
        /// </summary>
        /// <param name="command">Command to register.</param>
        /// <returns>Registered command.</returns>
        ICommand Register(IFluentCommand command);

        /// <summary>
        /// Register command to the execution group.
        /// </summary>
        /// <param name="command">Command to register.</param>
        /// <returns>Registered command.</returns>
        ICommand<TParameter> Register<TParameter>(IFluentCommand<TParameter> command);
    }
}
