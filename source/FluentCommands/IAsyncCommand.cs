using System.Threading.Tasks;
using System.Windows.Input;

namespace FluentCommands
{
    /// <summary>
    /// Async command interface.
    /// </summary>
    public interface IAsyncCommand : ICommand
    {
        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        Task ExecuteAsync(object parameter);
    }
}
