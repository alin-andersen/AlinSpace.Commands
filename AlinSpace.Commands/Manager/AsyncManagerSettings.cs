namespace AlinSpace.Commands
{
    /// <summary>
    /// Represents the manager settings.
    /// </summary>
    public class AsyncManagerSettings
    {
        /// <summary>
        /// Verify CanExecute allows execution before invoking a command.
        /// </summary>
        public bool VerifyCanExecuteBeforeExecution { get; } = true;

        /// <summary>
        /// Ignore CanExecute of all commands.
        /// </summary>
        public bool IgnoreIndividualCanExecute { get; } = false;

        /// <summary>
        /// Ignore exceptions thrown from commands.
        /// </summary>
        public bool IgnoreExceptionsFromCommands { get; } = true;

        /// <summary>
        /// Continue on captured context.
        /// </summary>
        public bool ContinueOnCapturedContext { get; } = true;

        /// <summary>
        /// Raises can execute changed on all commands after command execution.
        /// </summary>
        public bool RaiseCanExecuteChangedOnAllCommandsAfterAnyCommandExecution { get; } = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsyncManagerSettings(
            bool verifyCanExecuteBeforeExecution = true,
            bool ignoreIndividualCanExecute = false,
            bool ignoreExceptionsFromCommands = true,
            bool continueOnCapturedContext = true,
            bool raiseCanExecuteChangedOnAllCommandsAfterAnyCommandExecution = false)
        {
            VerifyCanExecuteBeforeExecution = verifyCanExecuteBeforeExecution;
            IgnoreIndividualCanExecute = ignoreIndividualCanExecute;
            IgnoreExceptionsFromCommands = ignoreExceptionsFromCommands;
            ContinueOnCapturedContext = continueOnCapturedContext;
            RaiseCanExecuteChangedOnAllCommandsAfterAnyCommandExecution = raiseCanExecuteChangedOnAllCommandsAfterAnyCommandExecution;
        }

        /// <summary>
        /// Create default settings.
        /// </summary>
        /// <returns>Default settings.</returns>
        public static AsyncManagerSettings Default()
        {
            return new AsyncManagerSettings();
        }

        public AsyncManagerSettings WithVerifyCanExecuteBeforeExecution(bool enable)
        {
            return new AsyncManagerSettings(
                verifyCanExecuteBeforeExecution: enable,
                ignoreIndividualCanExecute: IgnoreIndividualCanExecute,
                ignoreExceptionsFromCommands: IgnoreExceptionsFromCommands,
                continueOnCapturedContext: ContinueOnCapturedContext,
                raiseCanExecuteChangedOnAllCommandsAfterAnyCommandExecution: RaiseCanExecuteChangedOnAllCommandsAfterAnyCommandExecution);
        }

        public AsyncManagerSettings WithIgnoreIndividualCanExecute(bool enable)
        {
            return new AsyncManagerSettings(
                verifyCanExecuteBeforeExecution: VerifyCanExecuteBeforeExecution,
                ignoreIndividualCanExecute: enable,
                ignoreExceptionsFromCommands: IgnoreExceptionsFromCommands,
                continueOnCapturedContext: ContinueOnCapturedContext,
                raiseCanExecuteChangedOnAllCommandsAfterAnyCommandExecution: RaiseCanExecuteChangedOnAllCommandsAfterAnyCommandExecution);
        }

        public AsyncManagerSettings WithIgnoreExceptionsFromCommands(bool enable)
        {
            return new AsyncManagerSettings(
                verifyCanExecuteBeforeExecution: VerifyCanExecuteBeforeExecution,
                ignoreIndividualCanExecute: IgnoreIndividualCanExecute,
                ignoreExceptionsFromCommands: enable,
                continueOnCapturedContext: ContinueOnCapturedContext,
                raiseCanExecuteChangedOnAllCommandsAfterAnyCommandExecution: RaiseCanExecuteChangedOnAllCommandsAfterAnyCommandExecution);
        }

        public AsyncManagerSettings WithContinueOnCapturedContext(bool enable)
        {
            return new AsyncManagerSettings(
                verifyCanExecuteBeforeExecution: VerifyCanExecuteBeforeExecution,
                ignoreIndividualCanExecute: IgnoreIndividualCanExecute,
                ignoreExceptionsFromCommands: IgnoreExceptionsFromCommands,
                continueOnCapturedContext: enable,
                raiseCanExecuteChangedOnAllCommandsAfterAnyCommandExecution: RaiseCanExecuteChangedOnAllCommandsAfterAnyCommandExecution);
        }

        public AsyncManagerSettings WithRaiseCanExecuteChangedOnAllCommandsAfterAnyCommandExecution(bool enable)
        {
            return new AsyncManagerSettings(
                verifyCanExecuteBeforeExecution: VerifyCanExecuteBeforeExecution,
                ignoreIndividualCanExecute: IgnoreIndividualCanExecute,
                ignoreExceptionsFromCommands: IgnoreExceptionsFromCommands,
                continueOnCapturedContext: ContinueOnCapturedContext,
                raiseCanExecuteChangedOnAllCommandsAfterAnyCommandExecution: enable);
        }
    }
}
