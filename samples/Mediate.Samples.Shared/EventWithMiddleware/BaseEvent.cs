using Mediate.Abstractions;
using System;

namespace Mediate.Samples.Shared.EventWithMiddleware
{
    public class BaseEvent : IEvent
    {
        public Guid EventId { get; }

        public BaseEvent()
        {
            EventId = Guid.NewGuid();
        }
    }
}
