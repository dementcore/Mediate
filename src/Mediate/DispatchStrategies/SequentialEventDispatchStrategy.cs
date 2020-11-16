using Mediate.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.DispatchStrategies
{
    /// <summary>
    /// Event dispatch strategy that executes event handlers after one another.
    /// </summary>
    public sealed class SequentialEventDispatchStrategy : IEventDispatchStrategy
    {
        public async Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers) where TEvent : IEvent
        {
            await Dispatch(@event, handlers, default);
        }

        public async Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers, CancellationToken cancellationToken) where TEvent : IEvent
        {
            List<Exception> exceptions = new List<Exception>();

            foreach (IEventHandler<TEvent> handler in handlers)
            {
                try
                {
                    await handler.Handle(@event, cancellationToken);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }
    }
}
