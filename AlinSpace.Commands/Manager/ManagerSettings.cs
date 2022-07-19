namespace AlinSpace.Commands
{
    /// <summary>
    /// Represents the manager settings.
    /// </summary>
    public class ManagerSettings
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
        public bool RaiseCanExecuteChangedOnAllAfterCommandExecution { get; } = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="verifyCanExecuteBeforeExecution"></param>
        /// <param name="ignoreIndividualCanExecute"></param>
        /// <param name="ignoreExceptionsFromCommands"></param>
        /// <param name="continueOnCapturedContext"></param>
        /// <param name="raiseCanExecuteChangedOnAllAfterCommandExecution"></param>
        public ManagerSettings(
            bool verifyCanExecuteBeforeExecution = true,
            bool ignoreIndividualCanExecute = false,
            bool ignoreExceptionsFromCommands = true,
            bool continueOnCapturedContext = true,
            bool raiseCanExecuteChangedOnAllAfterCommandExecution = false)
        {
            VerifyCanExecuteBeforeExecution = verifyCanExecuteBeforeExecution;
            IgnoreIndividualCanExecute = ignoreIndividualCanExecute;
            IgnoreExceptionsFromCommands = ignoreExceptionsFromCommands;
            ContinueOnCapturedContext = continueOnCapturedContext;
            RaiseCanExecuteChangedOnAllAfterCommandExecution = raiseCanExecuteChangedOnAllAfterCommandExecution;
        }

        /// <summary>
        /// Create default settings.
        /// </summary>
        /// <returns>Default settings.</returns>
        public static ManagerSettings Default()
        {
            return new ManagerSettings();
        }

        public ManagerSettings WithVerifyCanExecuteBeforeExecution(bool enable)
        {
            return new ManagerSettings(
                verifyCanExecuteBeforeExecution: enable,
                ignoreIndividualCanExecute: IgnoreIndividualCanExecute,
                ignoreExceptionsFromCommands: IgnoreExceptionsFromCommands,
                continueOnCapturedContext: ContinueOnCapturedContext,
                raiseCanExecuteChangedOnAllAfterCommandExecution: RaiseCanExecuteChangedOnAllAfterCommandExecution);
        }

        public ManagerSettings WithIgnoreIndividualCanExecute(bool enable)
        {
            return new ManagerSettings(
                verifyCanExecuteBeforeExecution: VerifyCanExecuteBeforeExecution,
                ignoreIndividualCanExecute: enable,
                ignoreExceptionsFromCommands: IgnoreExceptionsFromCommands,
                continueOnCapturedContext: ContinueOnCapturedContext,
                raiseCanExecuteChangedOnAllAfterCommandExecution: RaiseCanExecuteChangedOnAllAfterCommandExecution);
        }

        public ManagerSettings WithIgnoreExceptionsFromCommands(bool enable)
        {
            return new ManagerSettings(
                verifyCanExecuteBeforeExecution: VerifyCanExecuteBeforeExecution,
                ignoreIndividualCanExecute: IgnoreIndividualCanExecute,
                ignoreExceptionsFromCommands: enable,
                continueOnCapturedContext: ContinueOnCapturedContext,
                raiseCanExecuteChangedOnAllAfterCommandExecution: RaiseCanExecuteChangedOnAllAfterCommandExecution);
        }

        public ManagerSettings WithContinueOnCapturedContext(bool enable)
        {
            return new ManagerSettings(
                verifyCanExecuteBeforeExecution: VerifyCanExecuteBeforeExecution,
                ignoreIndividualCanExecute: IgnoreIndividualCanExecute,
                ignoreExceptionsFromCommands: IgnoreExceptionsFromCommands,
                continueOnCapturedContext: enable,
                raiseCanExecuteChangedOnAllAfterCommandExecution: RaiseCanExecuteChangedOnAllAfterCommandExecution);
        }

        public ManagerSettings WithRaiseCanExecuteChangedOnAllAfterCommandExecution(bool enable)
        {
            return new ManagerSettings(
                verifyCanExecuteBeforeExecution: VerifyCanExecuteBeforeExecution,
                ignoreIndividualCanExecute: IgnoreIndividualCanExecute,
                ignoreExceptionsFromCommands: IgnoreExceptionsFromCommands,
                continueOnCapturedContext: ContinueOnCapturedContext,
                raiseCanExecuteChangedOnAllAfterCommandExecution: enable);
        }
    }
}
