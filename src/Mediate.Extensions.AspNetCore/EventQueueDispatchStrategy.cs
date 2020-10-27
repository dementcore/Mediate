using Mediate.Core.Abstractions;
using Mediate.Extensions.AspNetCore.Queue;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Extensions.AspNetCore
{
    /// <summary>
    /// Event dispatch strategy that enqueues events to be handled by a background job.
    /// This class is public to allow registration into DI containers like Autofac, Unity, etc.
    /// This shouldn't be used from user code. 
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
