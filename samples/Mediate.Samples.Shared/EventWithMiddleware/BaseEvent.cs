using Mediate.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
