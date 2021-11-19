namespace AlinSpace.Exceptions
{
    /// <summary>
    /// Settings for <see cref="IAsyncCommandManager"/>.
    /// </summary>
    public interface IAsyncCommandManagerSettings
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
