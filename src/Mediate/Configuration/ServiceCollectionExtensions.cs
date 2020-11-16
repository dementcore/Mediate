using Mediate;
using Mediate.Abstractions;
using Mediate.Configuration;
using Mediate.Configuration.Builders;
using Mediate.DispatchStrategies;
using Mediate.HostedService;
using Mediate.Queue;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Service collection extension methods
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// This method provides a builder to configure each Mediate service manually.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IMediateBuilder AddMediateCore(this IServiceCollection services)
        {
            return new MediateBuilder(services);
        }

        /// <summary>
        /// This method configures all Mediate default services. Similar to call
        /// <see cref="ServiceCollectionExtensions.AddMediateCore"></see>, 
        /// <see cref="IMediateBuilder.AddDefaultMediator"></see>, 
        /// <see cref="IMediateBuilder.AddServiceProviderHandlerProvider"></see> and
        /// <see cref="IMediateBuilder.AddServiceProviderMiddlewareProvider"></see>
        /// </summary>
        /// <param name="services"></param>
        public static void AddMediate(this IServiceCollection services)
        {
            if (services.Any(s => s.ServiceType == typeof(IMediator)))
            {
                throw new InvalidOperationException("You have already registered a IMediator implementation");
            }

            services.AddTransient<IMediator, Mediator>();

            if (services.Any(s => s.ServiceType == typeof(IHandlerProvider)))
            {
                throw new InvalidOperationException("You have already registered a handler provider");
            }

            services.AddTransient<IHandlerProvider, ServiceProviderHandlerProvider>();

            if (services.Any(s => s.ServiceType == typeof(IMiddlewareProvider)))
            {
                throw new InvalidOperationException("You have already registered a middleware provider");
            }

            services.AddTransient<IMiddlewareProvider, ServiceProviderMiddlewareProvider>();
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
            if (services.Any(s => s.ServiceType == typeof(IEventDispatchStrategy)))
            {
                throw new InvalidOperationException("You have already registered an event dispatch strategy");
            }

            services.AddTransient(typeof(IEventDispatchStrategy), typeof(TDispatchStrategy));
        }

        /// <summary>
        /// Helper method to configure a query in Mediate
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <typeparam name="TResult">Query response type</typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IQueryHandlerBuilder<TQuery, TResult> ForMediateQuery<TQuery, TResult>(this IServiceCollection services) where TQuery : IQuery<TResult>
        {
            return new QueryHandlerBuilder<TQuery, TResult>(services);
        }

        /// <summary>
        /// Helper method to configure an event in Mediate
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IEventHandlerBuilder<TEvent> ForMediateEvent<TEvent>(this IServiceCollection services) where TEvent : IEvent
        {
            return new EventHandlerBuilder<TEvent>(services);
        }

        /// <summary>
        /// Helper method to register a generic event handler
        /// </summary>
        /// <param name="services"></param>
        /// <param name="genericHandler">Generic event handler type</param>
        public static void AddMediateGenericEventHandler(this IServiceCollection services,Type genericHandler)
        {
            if (services.Any(s => s.ServiceType == typeof(IEventHandler<>) && s.ImplementationType == genericHandler))
            {
                return;
            }

            if (genericHandler.GetInterface("IEventHandler`1") == null)
            {
                throw new InvalidOperationException("To register a generic eventHandler the handler must implement IEventHandler interface.");

            }

            if (!genericHandler.IsGenericType)
            {
                throw new InvalidOperationException("To register a generic eventHandler the handler must be a generic type.");
            }

            Type serviceType = typeof(IEventHandler<>);

            services.AddTransient(serviceType, genericHandler.GetGenericTypeDefinition());
        }

        /// <summary>
        /// Helper method to register a generic event middleware
        /// </summary>
        /// <param name="services"></param>
        /// <param name="genericHandler">Generic event middleware type</param>
        public static void AddMediateGenericEventMiddleware(this IServiceCollection services, Type genericHandler)
        {
            if (services.Any(s => s.ServiceType == typeof(IEventMiddleware<>) && s.ImplementationType == genericHandler))
            {
                return;
            }

            if (genericHandler.GetInterface("IEventMiddleware`1") == null)
            {
                throw new InvalidOperationException("To register a generic event middleware the middleware must implement IEventMiddleware interface.");

            }

            if (!genericHandler.IsGenericType)
            {
                throw new InvalidOperationException("To register a generic event middleware the middleware must be a generic type.");
            }

            Type serviceType = typeof(IEventMiddleware<>);

            services.AddTransient(serviceType, genericHandler.GetGenericTypeDefinition());
        }

        /// <summary>
        /// Helper method to register a generic query middleware
        /// </summary>
        /// <param name="services"></param>
        /// <param name="genericHandler">Generic query middleware type</param>
        public static void AddMediateGenericQueryMiddleware(this IServiceCollection services, Type genericHandler)
        {
            if (services.Any(s => s.ServiceType == typeof(IQueryMiddleware<,>) && s.ImplementationType == genericHandler))
            {
                return;
            }

            if (genericHandler.GetInterface("IQueryMiddleware`2") == null)
            {
                throw new InvalidOperationException("To register a generic query middleware the middleware must implement IQueryMiddleware interface.");

            }

            if (!genericHandler.IsGenericType)
            {
                throw new InvalidOperationException("To register a generic query middleware the middleware must be a generic type.");
            }

            Type serviceType = typeof(IQueryMiddleware<,>);

            services.AddTransient(serviceType, genericHandler.GetGenericTypeDefinition());
        }
    }
}
