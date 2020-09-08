using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Abstractions
{
    public interface IEventHandlerProvider
    {
        Task<IEnumerable<IEventHandler<TEvent>>> GetEventHandlers<TEvent>(IEvent @event) where TEvent : IEvent;
    }
}
