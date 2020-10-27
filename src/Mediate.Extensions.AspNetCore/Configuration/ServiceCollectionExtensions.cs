using Mediate.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Mediate.Extensions.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
       
        public static IMediateBuilder AddMediate(this IServiceCollection services)
        {
            return new MediateBuilder(services);
        }

        /// <summary>
        /// Registers an event handler for <typeparamref name="TEvent"/>
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <typeparam name="TEventHandler">Event handler type </typeparam>
        public static void AddMediateEventHandler<TEvent, TEventHandler>(this IServiceCollection services)
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent>
        {
            if (services.Any(s => s.ServiceType == typeof(IEventHandler<TEvent>) && s.ImplementationType == typeof(TEventHandler)))
            {
                return;
            }

            Type serviceType = typeof(IEventHandler<TEvent>);

            services.AddTransient(serviceType, typeof(TEventHandler));
        }

        /// <summary>
        /// Registers a message handler for <typeparamref name="TMessage"/>
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <typeparam name="TResult">Message response type </typeparam>
        /// <typeparam name="TMessageHandler">Event handler type </typeparam>
        public static void AddMediateMessageHandler<TMessage, TResult, TMessageHandler>(this IServiceCollection services)
          where TMessageHandler : IMessageHandler<TMessage, TResult>
            where TMessage : IMessage<TResult>
        {
            if (services.Any(s => s.ServiceType == typeof(IMessageHandler<TMessage, TResult>)))
            {
                return;
            }

            Type serviceType = typeof(IMessageHandler<TMessage, TResult>);

            services.AddTransient(serviceType, typeof(TMessageHandler));
        }
    }
}
