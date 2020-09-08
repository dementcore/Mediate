using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Abstractions
{
    internal abstract class EventHandlerWrapper
    {
        protected IEventHandlerProvider handlerProvider;

        public EventHandlerWrapper(IEventHandlerProvider eventHandlerProvider)
        {
            handlerProvider = eventHandlerProvider;
        }

        public abstract Task Handle(IEvent @event, CancellationToken cancellationToken);
    }

    internal class EventHandlerWrapper<TEvent> : EventHandlerWrapper where TEvent : IEvent
    {
        public EventHandlerWrapper(IEventHandlerProvider eventHandlerProvider) : base(eventHandlerProvider)
        {
        }

        public override async Task Handle(IEvent @event, CancellationToken cancellationToken)
        {
            IEnumerable<IEventHandler<TEvent>> handlers = await handlerProvider.GetEventHandlers<TEvent>(@event);

            foreach (IEventHandler<TEvent> handler in handlers)
            {
                await handler.Handle((TEvent)@event, cancellationToken);
            }
        }
    }
}
