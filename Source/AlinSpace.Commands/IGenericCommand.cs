namespace AlinSpace.Exceptions
{
    /// <summary>
    /// Generic version of <see cref="System.Windows.Input.ICommand"/>.
    /// </summary>
    /// <typeparam name="TParameter">Type of the command parameter.</typeparam>
    public interface ICommand<TParameter> : System.Windows.Input.ICommand
    {
        /// <summary>
        /// Can execute.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>True, if command can execute; false otherwise.</returns>
        bool CanExecute(TParameter parameter);

        /// <summary>
        /// Execute command.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        void Execute(TParameter parameter);
    }
}
