using Mediate;
using Mediate.Abstractions;
using Mediate.Configuration;
using Mediate.DispatchStrategies;
using Mediate.HostedService;
using Mediate.Queue;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Service collection extension methods
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// This method registers the Mediator and returns a builder to configure Mediate.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IMediateBuilder AddMediateCore(this IServiceCollection services)
        {
            if (services.Any(s => s.ServiceType == typeof(IMediator)))
            {
                throw new InvalidOperationException("You have already registered a IMediator implementation");
            }

            services.AddScoped<IMediator, Mediator>();

            return new MediateBuilder(services);
        }

        /// <summary>
        /// This method configures all Mediate default services. Similar to call
        /// <see cref="ServiceCollectionExtensions.AddMediateCore"></see>, 
        /// <see cref="IMediateBuilder.AddServiceProviderHandlerProvider"></see> and
        /// <see cref="IMediateBuilder.AddServiceProviderMiddlewareProvider"></see>
        /// </summary>
        /// <param name="services"></param>
        public static void AddMediate(this IServiceCollection services)
        {
            services.AddMediateCore()
                .AddServiceProviderHandlerProvider()
                .AddServiceProviderMiddlewareProvider();
        }

        /// <summary>
        /// Configures Mediate to use the parallel event dispatch strategy
        /// </summary>
        /// <param name="services"></param>
        public static void AddMediateParallelEventDispatchStrategy(this IServiceCollection services)
        {
            if (services.Any(s => s.ServiceType == typeof(IEventDispatchStrategy)))
            {
                throw new InvalidOperationException("You have already registered an event dispatch strategy");
            }

            services.AddTransient<IEventDispatchStrategy, ParallelEventDispatchStrategy>();
        }

        /// <summary>
        /// Configures Mediate to use the background queue event dispatch strategy.
        /// Registers a hosted service called EventDispatcherService, a queue and EventQueueDispatchStrategy class.
        /// </summary>
        /// <param name="services"></param>
        public static void AddMediateEventQueueDispatchStrategy(this IServiceCollection services)
        {
            if (services.Any(s => s.ServiceType == typeof(IEventDispatchStrategy)))
            {
                throw new InvalidOperationException("You have already registered an event dispatch strategy");
            }

            if (services.Any(s => s.ServiceType == typeof(EventQueue)))
            {
                throw new InvalidOperationException("You have already registered the EventQueue");
            }

            if (services.Any(s => s.ImplementationType == typeof(EventDispatcherService)))
            {
                throw new InvalidOperationException("You have already registered the EventDispatcherService hosted service");
            }

            services.AddTransient<IEventDispatchStrategy, EventQueueDispatchStrategy>();
            services.AddSingleton<EventQueue>();
            services.AddHostedService<EventDispatcherService>();
        }

        /// <summary>
        /// Configures Mediate to use the sequential event dispatch strategy
        /// </summary>
        /// <param name="services"></param>
        public static void AddMediateSequentialEventDispatchStrategy(this IServiceCollection services)
        {
            if (services.Any(s => s.ServiceType == typeof(IEventDispatchStrategy)))
            {
                throw new InvalidOperationException("You have already registered an event dispatch strategy");
            }

            services.AddTransient<IEventDispatchStrategy, SequentialEventDispatchStrategy>();
        }

        /// <summary>
        /// Configures Mediate to use a custom event dispatch strategy
        /// </summary>
        /// <typeparam name="TDispatchStrategy"></typeparam>
        /// <param name="services"></param>
        public static void AddMediateCustomDispatchStrategy<TDispatchStrategy>(this IServiceCollection services)
          where TDispatchStrategy : IEventDispatchStrategy
        {
            AddMediateCustomDispatchStrategy<TDispatchStrategy>(services, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Configures Mediate to use a custom event dispatch strategy
        /// </summary>
        /// <typeparam name="TDispatchStrategy"></typeparam>
        /// <param name="services"></param>
        /// <param name="lifetime">ServiceLifetime for the DI container</param>
        public static void AddMediateCustomDispatchStrategy<TDispatchStrategy>(this IServiceCollection services, ServiceLifetime lifetime)
          where TDispatchStrategy : IEventDispatchStrategy
        {
            if (services.Any(s => s.ServiceType == typeof(IEventDispatchStrategy)))
            {
                throw new InvalidOperationException("You have already registered an event dispatch strategy");
            }

            services.Add(new ServiceDescriptor(typeof(IEventDispatchStrategy), typeof(TDispatchStrategy), lifetime));
        }

        /// <summary>
        /// Helper method to register events, querys, handlers and middlewares from an assembly
        /// </summary>
        /// <param name="services">service collection</param>
        /// <param name="assembly">Assembly to scan</param>
        public static void AddMediateClassesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            IEnumerable<Type> assemblyTypes = assembly.DefinedTypes;

            RegisterHelpers.RegisterClassesFromAssemblyAndType(services, typeof(IEventHandler<>), assemblyTypes, true, true);
            RegisterHelpers.RegisterClassesFromAssemblyAndType(services, typeof(IEventMiddleware<>), assemblyTypes, true, true);
            RegisterHelpers.RegisterClassesFromAssemblyAndType(services, typeof(IQueryHandler<,>), assemblyTypes, false, false);
            RegisterHelpers.RegisterClassesFromAssemblyAndType(services, typeof(IQueryMiddleware<,>), assemblyTypes, true, true);
        }


    }
}
