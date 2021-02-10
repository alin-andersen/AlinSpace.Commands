using System;
using System.Windows.Input;

namespace FluentCommands
{
    /// <summary>
    /// Fluent command.
    /// </summary>
    public class FluentCommand : ICommand
    {
        readonly bool verifyCanExecuteBeforeExecution;

        Action<object> executeAction;
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
        public static FluentCommand New(bool verifyCanExecuteBeforeExecution = false)
        {
            return new FluentCommand(verifyCanExecuteBeforeExecution);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="verifyCanExecuteBeforeExecution"></param>
        public FluentCommand(bool verifyCanExecuteBeforeExecution = false)
        {
            this.verifyCanExecuteBeforeExecution = verifyCanExecuteBeforeExecution;
        }

        /// <summary>
        /// Execute action.
        /// </summary>
        /// <param name="executeAction"></param>
        /// <returns>Fluent command.</returns>
        public FluentCommand OnExecute(Action<object> executeAction)
        {
            this.executeAction = executeAction;
            return this;
        }

        /// <summary>
        /// Execute action.
        /// </summary>
        /// <param name="executeAction"></param>
        /// <returns>Fluent command.</returns>
        public FluentCommand OnCanExecute(Func<object, bool> canExecuteAction)
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
            if (verifyCanExecuteBeforeExecution)
            {
                if (!CanExecute(parameter))
                    return;
            }

            executeAction?.Invoke(parameter);
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
    public class FluentCommand<TParameter> : ICommand
    {
        readonly bool verifyCanExecuteBeforeExecution;

        Action<TParameter> executeAction;
        Func<TParameter, bool> canExecuteAction;

        /// <summary>
        /// Can execute changed.
        /// </summary>
        public event EventHandler CanExecuteChanged = delegate { };

        /// <summary>
        /// Static factory method.
        /// </summary>
        /// <returns>Fluent command.</returns>
        public static FluentCommand<TParameter> New(bool verifyCanExecuteBeforeExecution = false)
        {
            return new FluentCommand<TParameter>(verifyCanExecuteBeforeExecution);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="verifyCanExecuteBeforeExecution"></param>
        public FluentCommand(bool verifyCanExecuteBeforeExecution = false)
        {
            this.verifyCanExecuteBeforeExecution = verifyCanExecuteBeforeExecution;
        }

        /// <summary>
        /// Execute action.
        /// </summary>
        /// <param name="executeAction"></param>
        /// <returns>Fluent command.</returns>
        public FluentCommand<TParameter> OnExecute(Action<TParameter> executeAction)
        {
            this.executeAction = executeAction;
            return this;
        }

        /// <summary>
        /// Execute action.
        /// </summary>
        /// <param name="executeAction"></param>
        /// <returns>Fluent command.</returns>
        public FluentCommand<TParameter> OnCanExecute(Func<TParameter, bool> canExecuteAction)
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
        public void Execute(TParameter parameter)
        {
            if (verifyCanExecuteBeforeExecution)
            {
                if (!CanExecute(parameter))
                    return;
            }

            executeAction?.Invoke(parameter);
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
