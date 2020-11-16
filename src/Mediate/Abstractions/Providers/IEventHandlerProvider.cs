using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Abstractions
{
    /// <summary>
    /// Interface for implement an event handler provider
    /// </summary>
    public interface IEventHandlerProvider
    {
        /// <summary>
        /// Gets all event handlers from an event
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <returns>All registered handlers for that event</returns>
        Task<IEnumerable<IEventHandler<TEvent>>> GetHandlers<TEvent>() where TEvent : IEvent;
    }
}
