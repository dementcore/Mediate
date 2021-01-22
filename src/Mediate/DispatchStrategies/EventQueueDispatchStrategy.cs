using Mediate.Abstractions;
using Mediate.Queue;
using Mediate.Wrappers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.DispatchStrategies
{
    /// <summary>
    /// Event dispatch strategy that enqueues events to be handled by a background job.
    /// </summary>
    public sealed class EventQueueDispatchStrategy : IEventDispatchStrategy, IDisposable
    {
        private readonly EventQueue _eventQueue;

        /// <summary>
        /// Event queue dispatch strategy constructor
        /// </summary>
        /// <param name="eventQueue">Event queue implementation</param>
        public EventQueueDispatchStrategy(EventQueue eventQueue)
        {
            _eventQueue = eventQueue;
        }

        /// <summary>
        /// Executes this strategy to dispatch an event
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="event">Event data</param>
        /// <param name="handlers">Event handlers</param>
        /// <returns></returns>
        public Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers) where TEvent : IEvent
        {
            return Dispatch(@event, handlers, default);
        }

        /// <summary>
        /// Executes this strategy to dispatch an event
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="event">Event data</param>
        /// <param name="handlers">Event handlers</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers, CancellationToken cancellationToken) where TEvent : IEvent
        {

            var queuedEvent = (QueuedEventWrapperBase)
                Activator.CreateInstance(typeof(QueuedEventWrapper<>).MakeGenericType(typeof(TEvent)), @event, handlers);

            await _eventQueue.EnqueueEvent(queuedEvent, cancellationToken);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
