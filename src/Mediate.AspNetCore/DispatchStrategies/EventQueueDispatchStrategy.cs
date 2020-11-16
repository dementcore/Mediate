using Mediate.Core.Abstractions;
using Mediate.AspNetCore.Queue;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.AspNetCore.DispatchStrategies
{
    /// <summary>
    /// Event dispatch strategy that enqueues events to be handled by a background job.
    /// </summary>
    public sealed class EventQueueDispatchStrategy : IEventDispatchStrategy
    {
        private readonly EventQueue _eventQueue;
        public EventQueueDispatchStrategy(EventQueue eventQueue)
        {
            _eventQueue = eventQueue;
        }

        public Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers) where TEvent : IEvent
        {
            return Dispatch(@event, handlers, default);
        }

        public Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers, CancellationToken cancellationToken) where TEvent : IEvent
        {
            var queuedEvent = (QueuedEventWrapperBase)
                Activator.CreateInstance(typeof(QueuedEventWrapper<>).MakeGenericType(typeof(TEvent)), @event, handlers);

            _eventQueue.EnqueueEvent(queuedEvent);

            return Task.CompletedTask;
        }
    }
}
