using System;
using System.Threading.Tasks;

namespace AlinSpace.FluentCommands
{
    /// <summary>
    /// Default implementation of <see cref="IFluentCommand"/>.
    /// </summary>
    public class FluentCommand : IFluentCommand
    {
        readonly bool verifyCanExecuteBeforeExecution;
        readonly bool continueOnCapturedContext;

        Func<object, Task> executeFunc;
        Func<object, bool> canExecuteFunc;

        /// <summary>
        /// Can execute changed.
        /// </summary>
        public event EventHandler CanExecuteChanged = delegate { };

        /// <summary>
        /// Static factory method.
        /// </summary>
        /// <param name="verifyCanExecuteBeforeExecution"></param>
        /// <param name="continueOnCapturedContext"></param>
        /// <returns>Fluent command.</returns>
        public static FluentCommand New(
            bool verifyCanExecuteBeforeExecution = false,
            bool continueOnCapturedContext = true)
        {
            return new FluentCommand(
                verifyCanExecuteBeforeExecution,
                continueOnCapturedContext);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="verifyCanExecuteBeforeExecution"></param>
        /// <param name="continueOnCapturedContext"></param>
        public FluentCommand(
            bool verifyCanExecuteBeforeExecution = false,
            bool continueOnCapturedContext = true)
        {
            this.verifyCanExecuteBeforeExecution = verifyCanExecuteBeforeExecution;
            this.continueOnCapturedContext = continueOnCapturedContext;
        }

        /// <summary>
        /// On can execute callback.
        /// </summary>
        /// <param name="executeFunc"></param>
        /// <returns>Fluent command.</returns>
        public FluentCommand OnCanExecute(Func<object, bool> canExecuteFunc)
        {
            this.canExecuteFunc = canExecuteFunc;
            return this;
        }

        /// <summary>
        /// On execute asynchronously callback.
        /// </summary>
        /// <param name="executeAction"></param>
        /// <returns>Fluent command.</returns>
        public FluentCommand OnExecuteAsync(Func<object, Task> executeFunc)
        {
            this.executeFunc = executeFunc;
            return this;
        }

        /// <summary>
        /// Can execute.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        /// <returns>True if can execute; false otherwise.</returns>
        public bool CanExecute(object parameter = null)
        {
            if (canExecuteFunc == null)
                return true;

            return canExecuteFunc(parameter);
        }

        /// <summary>
        /// Execute command asynchronously.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>Task.</returns>
        public async Task ExecuteAsync(object parameter = null)
        {
            if (executeFunc == null)
                return;

            if (verifyCanExecuteBeforeExecution)
            {
                if (!CanExecute(parameter))
                    return;
            }

            await executeFunc(parameter).ConfigureAwait(continueOnCapturedContext);
        }
    }
}
