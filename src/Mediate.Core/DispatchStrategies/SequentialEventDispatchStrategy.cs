using Mediate.Core.Abstractions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Core.DispatchStrategies
{
    public sealed class SequentialEventDispatchStrategy : IEventDispatchStrategy
    {
        public async Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers, CancellationToken cancellationToken = default) where TEvent : IEvent
        {
            foreach (IEventHandler<TEvent> handler in handlers)
            {
                await handler.Handle(@event, cancellationToken);
            }
        }
    }
}
