using Mediate.Core.Abstractions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.AspNetCore.DispatchStrategies
{
    /// <summary>
    /// Event dispatch strategy that executes event handlers after one another.
    /// </summary>
    public sealed class SequentialEventDispatchStrategy : IEventDispatchStrategy
    {
        public async Task ExecuteStrategy<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers) where TEvent : IEvent
        {
            await ExecuteStrategy(@event, handlers, default).ConfigureAwait(false);
        }

        public async Task ExecuteStrategy<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers, CancellationToken cancellationToken) where TEvent : IEvent
        {
            foreach (IEventHandler<TEvent> handler in handlers)
            {
                await handler.Handle(@event, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
