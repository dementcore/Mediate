using Mediate.Core.Abstractions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Core.DispatchStrategies
{
    public sealed class SequentialEventDispatchStrategy : IEventDispatchStrategy
    {
        public async Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers) where TEvent : IEvent
        {
            await Dispatch(@event, handlers, default).ConfigureAwait(false);
        }

        public async Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers, CancellationToken cancellationToken) where TEvent : IEvent
        {
            foreach (IEventHandler<TEvent> handler in handlers)
            {
                await handler.Handle(@event, cancellationToken);
            }
        }
    }
}
