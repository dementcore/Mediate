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
            
            await next();
        }
    }
}
