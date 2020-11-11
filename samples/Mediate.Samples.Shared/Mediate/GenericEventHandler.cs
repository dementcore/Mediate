using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mediate.Core.Abstractions;

namespace Mediate.Samples.Shared
{
    /// <summary>
    /// This class catchs all events
    /// </summary>
    public class GenericEventHandler<T> : IEventHandler<T> where T : IEvent
    {
        public Task Handle(T @event, CancellationToken cancellationToken)
        {
            //whatever action
            Console.WriteLine("Generic event handler");

            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// This class catchs all base events
    /// </summary>
    public class GenericBaseEventHandler<T> : IEventHandler<T> where T : BaseEvent
    {
        public Task Handle(T @event, CancellationToken cancellationToken)
        {
            //whatever action
            Console.WriteLine("Generic event handler with base " + @event.EventId);

            return Task.CompletedTask;
        }
    }
}
