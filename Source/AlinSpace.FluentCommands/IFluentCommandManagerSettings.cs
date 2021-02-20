namespace AlinSpace.FluentCommands
{
    /// <summary>
    /// Settings for <see cref="IFluentCommandManager"/>.
    /// </summary>
    public interface IFluentCommandManagerSettings
    {
        /// <summary>
        /// Verify CanExecute allows execution before invoking a command.
        /// </summary>
        public bool VerifyCanExecuteBeforeExecution { set; }

        /// <summary>
        /// Ignore CanExecute of all commands.
        /// </summary>
        public bool IgnoreIndividualCanExecute { set; }

        /// <summary>
        /// Ignore exceptions thrown from commands.
        /// </summary>
        public bool IgnoreExceptionsFromCommands { set; }

        /// <summary>
        /// Continue on captured context.
        /// </summary>
        public bool ContinueOnCapturedContext { set; }
    }
}
