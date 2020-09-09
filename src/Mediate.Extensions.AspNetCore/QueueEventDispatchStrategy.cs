using Mediate.Core.Abstractions;
using Mediate.Extensions.AspNetCore.Queue;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Extensions.AspNetCore
{
    public sealed class QueueEventDispatchStrategy : IEventDispatchStrategy
    {
        private readonly IEventQueue _eventQueue;
        public QueueEventDispatchStrategy(IEventQueue eventQueue)
        {
            _eventQueue = eventQueue;
        }

        public Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers, CancellationToken cancellationToken = default) where TEvent : IEvent
        {
            var queuedEvent = (QueuedEventWrapperBase)
                Activator.CreateInstance(typeof(QueuedEventWrapper<>).MakeGenericType(typeof(TEvent)), @event, handlers);

            _eventQueue.EnqueueEvent(queuedEvent);

            return Task.CompletedTask;
        }
    }
}
