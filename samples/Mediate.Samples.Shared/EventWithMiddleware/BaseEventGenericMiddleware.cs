using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mediate.Core.Abstractions;

namespace Mediate.Samples.Shared.EventWithMiddleware
{
    public class BaseEventGenericMiddleware<TEvent> : IEventMiddleware<TEvent> where TEvent : BaseEvent
    {
        public async Task Invoke(TEvent @event, CancellationToken cancellationToken, NextMiddlewareDelegate next)
        {

            //example validation
            if (@event.EventId == Guid.Empty)
            {
                //example exception for this example
                throw new InvalidOperationException("The event id must be not null");
            }

            await next();
        }
    }
}
