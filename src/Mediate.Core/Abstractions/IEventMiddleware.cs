using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Core.Abstractions
{
    /// <summary>
    /// Interface to implement a middleware to process an event before it reaches it's handlers.
    /// <typeparamref name="TEvent">Event type</typeparam>
    /// </summary>
    public interface IEventMiddleware<in TEvent> where TEvent : IEvent
    {
        Task Invoke(TEvent @event, CancellationToken cancellationToken, NextMiddlewareDelegate next);
    }
}
