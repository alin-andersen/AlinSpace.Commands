using System.Threading.Tasks;

namespace AlinSpace.Commands
{
    /// <summary>
    /// Abstract implementation of the <see cref="IAsyncCommand{TParameter}"/> interface.
    /// </summary>
    public abstract class AbstractAsyncCommand<TParameter> : AbstractAsyncCommand
    {
        /// <summary>
        /// Evaluates whether or not the command can execute asynchronously.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>True, if command can be executed; false otherwise.</returns>
        public override bool CanExecute(object? parameter = default)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return CanExecute((TParameter)parameter);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }

        /// <summary>
        /// Can command execute.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>True, if command can be executed; false otherwise.</returns>
        public virtual bool CanExecute(TParameter? parameter = default)
        {
            return true;
        }

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        public override Task ExecuteAsync(object? parameter = null)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return ExecuteAsync((TParameter)parameter);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        public abstract Task ExecuteAsync(TParameter? parameter = default);
    }
}
