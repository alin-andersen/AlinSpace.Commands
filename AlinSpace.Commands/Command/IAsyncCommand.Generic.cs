using System.Threading.Tasks;

namespace AlinSpace.Commands
{
    /// <summary>
    /// Represents a generic asynchronous command interface.
    /// </summary>
    public interface IAsyncCommand<TParameter> : IAsyncCommand
    {
        /// <summary>
        /// Evaluates whether or not the command can execute asynchronously.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>True, if command can be executed; false otherwise.</returns>
        bool CanExecute(TParameter? parameter = default);

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        Task ExecuteAsync(TParameter? parameter = default);
    }
}
