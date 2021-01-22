using Mediate.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Wrappers
{
    /// <summary>
    /// Wrapper class to represent a event into the event queue
    /// </summary>
    internal abstract class QueuedEventWrapperBase
    {
        protected QueuedEventWrapperBase(string eventName)
        {
            EventName = eventName;
        }

        public string EventName { get; }

        internal abstract Task Handle(CancellationToken cancellationToken);
    }

    /// <summary>
    /// Wrapper class to represent a event into the event queue
    /// </summary>
    internal sealed class QueuedEventWrapper<TEvent> : QueuedEventWrapperBase where TEvent : IEvent
    {
        private TEvent Event { get; }

        private readonly IEnumerable<IEventHandler<TEvent>> _handlers;

        public QueuedEventWrapper(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers)
        : base(@event.GetType().Name)
        {
            Event = @event;
            _handlers = handlers;
        }

        internal override async Task Handle(CancellationToken cancellationToken)
        {
            List<Exception> exceptions = new List<Exception>();

            foreach (var handler in _handlers)
            {
                try
                {
                    await handler.Handle(Event, cancellationToken);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }
    }
}
