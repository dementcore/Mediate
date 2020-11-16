using Mediate.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Configuration.Builders
{
    /// <summary>
    /// Helper class to configure the event handlers and middlewares
    /// </summary>
    /// <typeparam name="TEvent">Event type</typeparam>
    public sealed class EventHandlerBuilder<TEvent> : IEventHandlerBuilder<TEvent> where TEvent : IEvent
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="services"></param>
        public EventHandlerBuilder(IServiceCollection services)
        {
            _services = services;
        }

        /// <summary>
        /// Adds a handler for <typeparamref name="TEvent"/>
        /// </summary>
        /// <typeparam name="TEventHandler">Event handler type</typeparam>
        /// <returns></returns>
        public IEventHandlerBuilder<TEvent> AddHandler<TEventHandler>() where TEventHandler : IEventHandler<TEvent>
        {
            if (_services.Any(s => s.ServiceType == typeof(IEventHandler<TEvent>) && s.ImplementationType == typeof(TEventHandler)))
            {
                throw new InvalidOperationException("Duplicate event handler registration found. You can register multiple event handlers but you must register a concrete event handler only once.");
            }

            Type serviceType = typeof(IEventHandler<TEvent>);

            _services.AddTransient(serviceType, typeof(TEventHandler));

            return this;
        }

        /// <summary>
        /// Adds a middleware for <typeparamref name="TEvent"/>
        /// </summary>
        /// <typeparam name="TEventMiddleware">Event middleware type</typeparam>
        /// <returns></returns>
        public IEventHandlerBuilder<TEvent> AddMiddleware<TEventMiddleware>() where TEventMiddleware : IEventMiddleware<TEvent>
        {
            if (_services.Any(s => s.ServiceType == typeof(IEventMiddleware<TEvent>) && s.ImplementationType == typeof(TEventMiddleware)))
            {
                throw new InvalidOperationException("Duplicate event middleware found. You can register multiple middleware for a concrete event but you must register a concrete middleware only once.");
            }

            Type serviceType = typeof(IEventMiddleware<TEvent>);

            _services.AddTransient(serviceType, typeof(TEventMiddleware));

            return this;
        }
    }
}
