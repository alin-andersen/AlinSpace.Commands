namespace AlinSpace.Command
{
    /// <summary>
    /// Settings for <see cref="IManager"/>.
    /// </summary>
    public interface IManagerSettings
    {
        /// <summary>
        /// Verify CanExecute allows execution before invoking a command.
        /// </summary>
        bool VerifyCanExecuteBeforeExecution { set; }

        /// <summary>
        /// Ignore CanExecute of all commands.
        /// </summary>
        bool IgnoreIndividualCanExecute { set; }

        /// <summary>
        /// Ignore exceptions thrown from commands.
        /// </summary>
        bool IgnoreExceptionsFromCommands { set; }

        /// <summary>
        /// Continue on captured context.
        /// </summary>
        bool ContinueOnCapturedContext { set; }
    }
}
