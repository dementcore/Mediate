using Mediate.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Samples.Shared.EventWithMiddleware
{
    public class SampleComplexEventMiddleware : IEventMiddleware<SampleComplexEvent>
    {
        public async Task Invoke(SampleComplexEvent @event, CancellationToken cancellationToken, NextMiddlewareDelegate next)
        {
            @event.EventData += " [modified from middleware]";

            await next();
        }
    }
}
