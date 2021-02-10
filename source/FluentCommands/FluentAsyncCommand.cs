using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FluentCommands
{
    /// <summary>
    /// Fluent async command.
    /// </summary>
    public class FluentAsyncCommand : IAsyncCommand
    {
        readonly bool verifyCanExecuteBeforeExecution;

        Func<object, Task> executeAction;
        Func<object, bool> canExecuteAction;

        /// <summary>
        /// Can execute changed.
        /// </summary>
        public event EventHandler CanExecuteChanged = delegate { };

        /// <summary>
        /// Static factory method.
        /// </summary>
        /// <param name="verifyCanExecuteBeforeExecution"></param>
        /// <returns>Fluent command.</returns>
        public static FluentAsyncCommand New(bool verifyCanExecuteBeforeExecution = false)
        {
            return new FluentAsyncCommand(verifyCanExecuteBeforeExecution);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="verifyCanExecuteBeforeExecution"></param>
        public FluentAsyncCommand(bool verifyCanExecuteBeforeExecution = false)
        {
            this.verifyCanExecuteBeforeExecution = verifyCanExecuteBeforeExecution;
        }

        /// <summary>
        /// Execute action.
        /// </summary>
        /// <param name="executeAction"></param>
        /// <returns>Fluent command.</returns>
        public FluentAsyncCommand OnExecuteAsync(Func<object, Task> executeAction)
        {
            this.executeAction = executeAction;
            return this;
        }

        /// <summary>
        /// Execute action.
        /// </summary>
        /// <param name="executeAction"></param>
        /// <returns>Fluent command.</returns>
        public FluentAsyncCommand OnCanExecute(Func<object, bool> canExecuteAction)
        {
            this.canExecuteAction = canExecuteAction;
            return this;
        }

        /// <summary>
        /// Execute.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        public async void Execute(object parameter)
        {
            try
            {
                await ExecuteAsync(parameter);//.ConfigureAwait(false);
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Execute.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        public async Task ExecuteAsync(object parameter)
        {
            if (verifyCanExecuteBeforeExecution)
            {
                if (!CanExecute(parameter))
                    return;
            }

            await executeAction?.Invoke(parameter);
        }

        /// <summary>
        /// Can execute.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        /// <returns>True if can execute; false otherwise.</returns>
        public bool CanExecute(object parameter)
        {
            return canExecuteAction?.Invoke(parameter) ?? true;
        }
    }

    /// <summary>
    /// Fluent command.
    /// </summary>
    public class FluentAsyncCommand<TParameter> : ICommand
    {
        readonly bool verifyCanExecuteBeforeExecution;

        Func<TParameter, Task> executeAction;
        Func<TParameter, bool> canExecuteAction;

        /// <summary>
        /// Can execute changed.
        /// </summary>
        public event EventHandler CanExecuteChanged = delegate { };

        /// <summary>
        /// Static factory method.
        /// </summary>
        /// <returns>Fluent command.</returns>
        public static FluentAsyncCommand<TParameter> New(bool verifyCanExecuteBeforeExecution = false)
        {
            return new FluentAsyncCommand<TParameter>(verifyCanExecuteBeforeExecution);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="verifyCanExecuteBeforeExecution"></param>
        public FluentAsyncCommand(bool verifyCanExecuteBeforeExecution = false)
        {
            this.verifyCanExecuteBeforeExecution = verifyCanExecuteBeforeExecution;
        }

        /// <summary>
        /// Execute action.
        /// </summary>
        /// <param name="executeAction"></param>
        /// <returns>Fluent command.</returns>
        public FluentAsyncCommand<TParameter> OnExecute(Func<TParameter, Task> executeAction)
        {
            this.executeAction = executeAction;
            return this;
        }

        /// <summary>
        /// Execute action.
        /// </summary>
        /// <param name="executeAction"></param>
        /// <returns>Fluent command.</returns>
        public FluentAsyncCommand<TParameter> OnCanExecute(Func<TParameter, bool> canExecuteAction)
        {
            this.canExecuteAction = canExecuteAction;
            return this;
        }

        /// <summary>
        /// Execute.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        public void Execute(object parameter)
        {
            Execute((TParameter)parameter);
        }

        /// <summary>
        /// Execute.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        public async void Execute(TParameter parameter)
        {
            try
            {
                // Fire and forget.
                await ExecuteAsync(parameter);//.ConfigureAwait(false);
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        public async Task ExecuteAsync(object parameter)
        {
            await ExecuteAsync((TParameter)parameter);//.ConfigureAwait(false);
        }

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        public async Task ExecuteAsync(TParameter parameter)
        {
            if (verifyCanExecuteBeforeExecution)
            {
                if (!CanExecute(parameter))
                    return;
            }

            await executeAction?.Invoke(parameter);
        }

        /// <summary>
        /// Can execute.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        /// <returns>True if can execute; false otherwise.</returns>
        public bool CanExecute(object parameter)
        {
            return CanExecute((TParameter)parameter);
        }

        /// <summary>
        /// Can execute.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        /// <returns>True if can execute; false otherwise.</returns>
        public bool CanExecute(TParameter parameter)
        {
            return canExecuteAction?.Invoke(parameter) ?? true;
        }
    }
}
