using Mediate.Abstractions;
using System;

namespace Mediate.Samples.Shared.EventWithMiddleware
{
    public abstract class BaseEvent : IEvent
    {
        public Guid EventId { get; }

        public BaseEvent()
        {
            EventId = Guid.NewGuid();
        }
    }
}
