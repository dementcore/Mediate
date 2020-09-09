using Mediate.Core.Abstractions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Extensions.AspNetCore.Queue
{
    public abstract class QueuedEventWrapperBase
    {
        public QueuedEventWrapperBase(string eventName)
        {
            EventName = eventName;
        }

        public string EventName { get; }

        protected internal abstract Task Handle(CancellationToken cancellationToken);
    }

    public sealed class QueuedEventWrapper<TEvent> : QueuedEventWrapperBase where TEvent : IEvent
    {
        private TEvent Event { get; }

        private readonly IEnumerable<IEventHandler<TEvent>> _handlers;

        public QueuedEventWrapper(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers) : base(@event.GetType().Name)
        {
            Event = @event;
            _handlers = handlers;
        }

        protected internal override Task Handle(CancellationToken cancellationToken)
        {
             Parallel.ForEach(_handlers, (handler) =>
             {
                 handler.Handle(Event, cancellationToken);
             });

            return Task.CompletedTask;
        }
    }
}
