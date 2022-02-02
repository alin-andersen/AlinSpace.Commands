using System.Threading.Tasks;

namespace AlinSpace.Command
{
    /// <summary>
    /// Represents a generic asynchronous command interface.
    /// </summary>
    public interface ICommand<TParameter> : ICommand
    {
        /// <summary>
        /// Can command execute.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>True, if command can be executed; false otherwise.</returns>
        bool CanExecute(TParameter parameter = default);

        /// <summary>
        /// Execute command asynchronously.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        Task ExecuteAsync(TParameter parameter = default);
    }
}
