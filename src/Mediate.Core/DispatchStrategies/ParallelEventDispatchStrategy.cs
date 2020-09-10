using Mediate.Core.Abstractions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Core.DispatchStrategies
{
    public sealed class ParallelEventDispatchStrategy : IEventDispatchStrategy
    {
        public Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers) where TEvent : IEvent
        {
            return Dispatch(@event, handlers, default);
        }

        public Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers, CancellationToken cancellationToken) where TEvent : IEvent
        {
            Parallel.ForEach(handlers, (handler) =>
            {
                handler.Handle(@event, cancellationToken);
            });

            return Task.CompletedTask;
        }
    }
}
