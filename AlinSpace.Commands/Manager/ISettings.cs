namespace AlinSpace.Commands
{
    /// <summary>
    /// Settings for <see cref="IManager"/>.
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Raises can execute changed for all groups after execution of any command.
        /// </summary>
        bool RaiseCanExecuteAfterExecutionForAllGroups { get; set; }

        /// <summary>
        /// Verify CanExecute allows execution before invoking a command.
        /// </summary>
        bool VerifyCanExecuteBeforeExecution { get; set; }

        /// <summary>
        /// Ignore CanExecute of all commands.
        /// </summary>
        bool IgnoreIndividualCanExecute { get; set; }

        /// <summary>
        /// Ignore exceptions thrown from commands.
        /// </summary>
        bool IgnoreExceptionsFromCommands { get; set; }

        /// <summary>
        /// Continue on captured context.
        /// </summary>
        bool ContinueOnCapturedContext { get; set; }
    }
}
