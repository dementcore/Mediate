using Mediate.Abstractions;
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
    public interface IEventHandlerBuilder<TEvent> where TEvent : IEvent
    {
        /// <summary>
        /// Adds a handler for <typeparamref name="TEvent"/>
        /// </summary>
        /// <typeparam name="TEventHandler">Event handler type</typeparam>
        /// <returns></returns>
        IEventHandlerBuilder<TEvent> AddHandler<TEventHandler>() where TEventHandler : IEventHandler<TEvent>;

        /// <summary>
        /// Adds a middleware for <typeparamref name="TEvent"/>
        /// </summary>
        /// <typeparam name="TEventMiddleware">Event middleware type</typeparam>
        /// <returns></returns>
        IEventHandlerBuilder<TEvent> AddMiddleware<TEventMiddleware>() where TEventMiddleware : IEventMiddleware<TEvent>;
    }
}
