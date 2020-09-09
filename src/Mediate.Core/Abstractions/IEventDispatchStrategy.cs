using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Core.Abstractions
{
    public interface IEventDispatchStrategy
    {
        Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers, CancellationToken cancellationToken = default) where TEvent : IEvent;
    }
}
