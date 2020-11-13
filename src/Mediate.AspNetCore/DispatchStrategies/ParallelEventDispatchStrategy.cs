using Mediate.Core.Abstractions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.AspNetCore.DispatchStrategies
{
    /// <summary>
    /// Event dispatch strategy that executes event handlers in parallel.
    /// </summary>
    public sealed class ParallelEventDispatchStrategy : IEventDispatchStrategy
    {
        public Task ExecuteStrategy<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers) where TEvent : IEvent
        {
            return ExecuteStrategy(@event, handlers, default);
        }

        public Task ExecuteStrategy<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers, CancellationToken cancellationToken) where TEvent : IEvent
        {
            Parallel.ForEach(handlers, (handler) =>
            {
                handler.Handle(@event, cancellationToken);
            });

            return Task.CompletedTask;
        }
    }
}
