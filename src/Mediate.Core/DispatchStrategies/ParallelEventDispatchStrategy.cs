using Mediate.Core.Abstractions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Core.DispatchStrategies
{
    public sealed class ParallelEventDispatchStrategy : IEventDispatchStrategy
    {
        public Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers, CancellationToken cancellationToken = default) where TEvent : IEvent
        {
            Parallel.ForEach(handlers, async (handler) =>
            {
                await handler.Handle(@event, cancellationToken);
            });

            return Task.CompletedTask;
        }
    }
}
