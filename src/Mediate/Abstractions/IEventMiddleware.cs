using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Abstractions
{
    /// <summary>
    /// Interface to implement a middleware to process an event before it reaches it's handlers.
    /// <typeparamref name="TEvent">Event type</typeparamref>
    /// </summary>
    public interface IEventMiddleware<in TEvent> where TEvent : IEvent
    {
        /// <summary>
        /// Invoke the middleware logic
        /// </summary>
        /// <param name="event">Event object</param>
        /// <param name="cancellationToken"></param>
        /// <param name="next">Delegate that encapsulates a call to the next element in the pipeline</param>
        /// <returns></returns>
        Task Invoke(TEvent @event, CancellationToken cancellationToken, NextMiddlewareDelegate next);
    }
}