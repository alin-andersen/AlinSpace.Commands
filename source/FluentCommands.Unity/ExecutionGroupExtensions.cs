using Unity;

namespace FluentCommands.Unity
{
    /// <summary>
    /// Extensions for <see cref="IExecutionGroup"/>.
    /// </summary>
    public static class ExecutionGroupExtensions
    {
        /// <summary>
        /// Register command implementation instance to the execution group with the given container.
        /// </summary>
        /// <typeparam name="TFluentCommandImplementation">Type of the implementation of the <see cref="IFluentCommand"/>.</typeparam>
        /// <param name="executionGroup">Execution group to register the command to.</param>
        /// <param name="container">Container to register the command to.</param>
        /// <param name="name">Optional command name.</param>
        public static void RegisterCommandWithContainer<TFluentCommandImplementation>(
            this IExecutionGroup executionGroup,
            IUnityContainer container,
            TFluentCommandImplementation instance,
            string name = null)
            where TFluentCommandImplementation : IFluentCommand
        {
            container.RegisterInstance(name, instance, InstanceLifetime.PerContainer);
            var implementation = container.Resolve<TFluentCommandImplementation>(name);

            container.RegisterInstance(name, executionGroup.Register(implementation), InstanceLifetime.PerContainer);
        }

        /// <summary>
        /// Register command implementation type to the execution group with the given container.
        /// </summary>
        /// <typeparam name="TFluentCommandImplementation">Type of the implementation of the <see cref="IFluentCommand"/>.</typeparam>
        /// <param name="executionGroup">Execution group to register the command to.</param>
        /// <param name="container">Container to register the command to.</param>
        /// <param name="name">Optional command name.</param>
        public static void RegisterCommandWithContainer<TFluentCommandImplementation>(
            this IExecutionGroup executionGroup,
            IUnityContainer container,
            string name = null)
            where TFluentCommandImplementation : IFluentCommand
        {
            container.RegisterType<TFluentCommandImplementation>(name, TypeLifetime.PerContainer);
            var implementation = container.Resolve<TFluentCommandImplementation>(name);

            container.RegisterInstance(name, executionGroup.Register(implementation), InstanceLifetime.PerContainer);
        }

        /// <summary>
        /// Register command implementation type to the execution group with the given container.
        /// </summary>
        /// <typeparam name="TFluentCommandImplementation">Type of the implementation of the <see cref="IFluentCommand{TParameter}"/>.</typeparam>
        /// <typeparam name="TCommandParameter">Type of the command parameter.</typeparam>
        /// <param name="executionGroup">Execution group to register the command to.</param>
        /// <param name="container">Container to register the command to.</param>
        /// <param name="name">Optional command name.</param>
        public static void RegisterType<TFluentCommandImplementation, TCommandParameter>(
              this IExecutionGroup executionGroup,
              IUnityContainer container,
              string name = null)
              where TFluentCommandImplementation : IFluentCommand<TCommandParameter>
        {
            container.RegisterType<TFluentCommandImplementation>(name, TypeLifetime.PerContainer);
            var implementation = container.Resolve<TFluentCommandImplementation>(name);

            container.RegisterInstance(name, executionGroup.Register(implementation), InstanceLifetime.PerContainer);
        }
    }
}
