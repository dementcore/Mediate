using Mediate.Abstractions;
using Mediate.BackgroundEventDispatch.Abstractions;
using Mediate.BackgroundEventDispatch;
using Mediate.BackgroundEventDispatch.HostedService;
using Mediate.BackgroundEventDispatch.Queue;
using Mediate.BackgroundEventDispatch.Configuration;
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
        /// Configures Mediate to use the background queue event dispatch strategy with the DefaultEventQueueExceptionHandler.
        /// </summary>
        /// <param name="services"></param>
        public static void AddMediateEventQueueDispatchStrategy(this IServiceCollection services)
        {
            if (services.Any(s => s.ServiceType == typeof(IEventDispatchStrategy)))
            {
                throw new InvalidOperationException("You have already registered an EventDispatchStrategy");
            }

            if (services.Any(s => s.ServiceType == typeof(EventQueue)))
            {
                throw new InvalidOperationException("You have already registered the EventQueue");
            }

            if (services.Any(s => s.ServiceType == typeof(IEventQueueExceptionHandler)))
            {
                throw new InvalidOperationException("You have already registered an EventQueueExceptionHandler");
            }

            if (services.Any(s => s.ImplementationType == typeof(EventDispatcherService)))
            {
                throw new InvalidOperationException("You have already registered the EventDispatcherService hosted service");
            }

            services.AddScoped<IEventDispatchStrategy, EventQueueDispatchStrategy>();
            services.AddSingleton<EventQueue>();
            services.AddSingleton<IEventQueueExceptionHandler,DefaultEventQueueExceptionHandler>();
            services.AddHostedService<EventDispatcherService>();
        }

        /// <summary>
        /// Configures Mediate to use the background queue event dispatch strategy and returns a builder object to configure the queue.
        /// </summary>
        /// <param name="services"></param>
        public static IMediateEventQueueBuilder AddMediateEventQueueDispatchStrategyCore(this IServiceCollection services)
        {
            if (services.Any(s => s.ServiceType == typeof(IEventDispatchStrategy)))
            {
                throw new InvalidOperationException("You have already registered an EventDispatchStrategy");
            }

            if (services.Any(s => s.ServiceType == typeof(EventQueue)))
            {
                throw new InvalidOperationException("You have already registered the EventQueue");
            }

            if (services.Any(s => s.ServiceType == typeof(IEventQueueExceptionHandler)))
            {
                throw new InvalidOperationException("You have already registered an EventQueueExceptionHandler");
            }

            if (services.Any(s => s.ImplementationType == typeof(EventDispatcherService)))
            {
                throw new InvalidOperationException("You have already registered the EventDispatcherService hosted service");
            }

            services.AddScoped<IEventDispatchStrategy, EventQueueDispatchStrategy>();
            services.AddSingleton<EventQueue>();
            services.AddHostedService<EventDispatcherService>();

            return new MediateEventQueueBuilder(services);
        }
    }
}
