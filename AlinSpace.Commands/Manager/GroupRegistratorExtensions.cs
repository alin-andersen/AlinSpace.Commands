using System;

namespace AlinSpace.Commands
{
    /// <summary>
    /// Extensions for <see cref="IGroupRegistrator"/>.
    /// </summary>
    public static class GroupRegistratorExtensions
    {
        /// <summary>
        /// Register command to the group.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <returns>Itself.</returns>
        /// <remarks>
        /// Command argument will be overwriten by the registered command.
        /// </remarks>
        public static IGroupRegistrator Register(this IGroupRegistrator groupRegistrator, ref ICommand command)
        {
            var registeredCommand = groupRegistrator.Register(command);
            command = registeredCommand;

            return groupRegistrator;
        }

        /// <summary>
        /// Register command to the group.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="registerCommandProvider">Registered command provider.</param>
        /// <returns>Itself.</returns>
        public static IGroupRegistrator Register(this IGroupRegistrator groupRegistrator, ICommand command, Action<ICommand> registerCommandProvider)
        {
            var registeredCommand = groupRegistrator.Register(command);
            registerCommandProvider(registeredCommand);

            return groupRegistrator;
        }
    }
}
