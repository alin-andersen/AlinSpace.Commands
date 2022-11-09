using System;
using System.Threading.Tasks;

namespace AlinSpace.Commands
{
    /// <summary>
    /// Default implementation of the <see cref="IAsyncCommand"/> interface.
    /// </summary>
    public class AsyncCommand : AbstractAsyncCommand
    {
        private readonly bool verifyCanExecuteBeforeExecution;
        private readonly bool continueOnCapturedContext;

        private Func<object?, Task>? executeFunc;
        private Func<object?, bool>? canExecuteFunc;

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="verifyCanExecuteBeforeExecution">
        /// This flag indicates whether or not the can execute shall should be called and checked before execution.
        /// </param>
        /// <param name="continueOnCapturedContext">
        /// This flag indicates whether or not the command shall be executed on the captured context.
        /// </param>
        public AsyncCommand(
            bool verifyCanExecuteBeforeExecution = false,
            bool continueOnCapturedContext = true)
        {
            this.verifyCanExecuteBeforeExecution = verifyCanExecuteBeforeExecution;
            this.continueOnCapturedContext = continueOnCapturedContext;
        }

        /// <summary>
        /// Creates a new asynchronous command
        /// </summary>
        /// <param name="verifyCanExecuteBeforeExecution">
        /// This flag indicates whether or not the can execute shall should be called and checked before execution.
        /// </param>
        /// <param name="continueOnCapturedContext">
        /// This flag indicates whether or not the command shall be executed on the captured context.
        /// </param>
        /// <returns>Asynchronous command.</returns>
        public static AsyncCommand New(
            bool verifyCanExecuteBeforeExecution = false,
            bool continueOnCapturedContext = true)
        {
            return new AsyncCommand(
                verifyCanExecuteBeforeExecution,
                continueOnCapturedContext);
        }

        #endregion

        #region Setters

        /// <summary>
        /// Sets the asynchronously can execute function.
        /// </summary>
        /// <param name="canExecuteFunc">Can execute func.</param>
        /// <returns>Asynchronous command.</returns>
        public AsyncCommand SetCanExecute(Func<object?, bool>? canExecuteFunc)
        {
            this.canExecuteFunc = canExecuteFunc;
            return this;
        }

        /// <summary>
        /// Sets the asynchronously execute function.
        /// </summary>
        /// <param name="executeFunc">Execute func.</param>
        /// <returns>Asynchronous command.</returns>
        public AsyncCommand SetExecuteAsync(Func<object?, Task>? executeFunc)
        {
            this.executeFunc = executeFunc;
            return this;
        }

        #endregion

        #region IAsyncCommand

        /// <summary>
        /// Evaluates whether or not the command can execute asynchronously.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>True, if command can be executed; false otherwise.</returns>
        public override bool CanExecute(object? parameter = null)
        {
            if (canExecuteFunc == null)
                return true;

            return canExecuteFunc(parameter);
        }

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        public override async Task ExecuteAsync(object? parameter = null)
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

        #endregion
    }
}
