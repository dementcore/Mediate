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
    public class SampleComplexEventMiddleware : IEventMiddleware<SampleComplexEvent>
    {
        public async Task Invoke(SampleComplexEvent @event, CancellationToken cancellationToken, NextMiddlewareDelegate next)
        {
            @event.EventData += " modified from middleware!!";

            await next();
        }
    }
}
