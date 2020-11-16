using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Abstractions
{
    /// <summary>
    /// Interface for implement an event middleware provider
    /// </summary>
    public interface IEventMiddlewareProvider
    {
        /// <summary>
        /// Gets all event middlewares from an event
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <returns>All registered middlewares for that event</returns>
        Task<IEnumerable<IEventMiddleware<TEvent>>> GetMiddlewares<TEvent>() where TEvent : IEvent;
    }
}
