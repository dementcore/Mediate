using Mediate.Core.Abstractions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Extensions.AspNetCore.Queue
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

        internal override Task Handle(CancellationToken cancellationToken)
        {
            foreach (var handler in _handlers)
            {
                handler.Handle(Event, cancellationToken);
            }

            return Task.CompletedTask;
        }
    }
}
