using Mediate.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Extensions.AspNetCore.Configuration.Builders
{
    public interface IEventHandlerBuilder<T> where T : IEvent
    {
        IEventHandlerBuilder<T> AddHandler<TEventHandler>() where TEventHandler : IEventHandler<T>;
    }
}
