using Mediate.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mediate.Queue
{
    internal sealed class QueuedEvent
    {
        public EventHandlerWrapper EventHandler { get; }

        public IEvent Event { get; }

        public QueuedEvent(EventHandlerWrapper eventHandler,IEvent @event)
        {
            EventHandler = eventHandler;
            Event = @event;
        }
    }
}
