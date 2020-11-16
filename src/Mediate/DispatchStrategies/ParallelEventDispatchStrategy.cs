using Mediate.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.DispatchStrategies
{
    /// <summary>
    /// Event dispatch strategy that executes event handlers in parallel.
    /// </summary>
    public sealed class ParallelEventDispatchStrategy : IEventDispatchStrategy
    {
        public Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers) where TEvent : IEvent
        {
            return Dispatch(@event, handlers, default);
        }

        public Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers, CancellationToken cancellationToken) where TEvent : IEvent
        {
            ConcurrentQueue<Exception> exceptions = new ConcurrentQueue<Exception>();

            Parallel.ForEach(handlers, async (handler) =>
            {
                try
                {
                    await handler.Handle(@event, cancellationToken);
                }
                catch (Exception ex)
                {
                    exceptions.Enqueue(ex);
                }
            });

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }

            return Task.CompletedTask;
        }
    }
}
