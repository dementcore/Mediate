using Mediate.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Configuration.Builders
{
    public interface IEventHandlerBuilder<TEvent> where TEvent : IEvent
    {
        IEventHandlerBuilder<TEvent> AddHandler<TEventHandler>() where TEventHandler : IEventHandler<TEvent>;
        IEventHandlerBuilder<TEvent> AddMiddleware<TEventMiddleware>() where TEventMiddleware : IEventMiddleware<TEvent>;
    }
}
