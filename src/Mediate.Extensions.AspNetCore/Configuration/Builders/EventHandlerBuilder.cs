using Mediate.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Extensions.AspNetCore.Configuration.Builders
{
    public sealed class EventHandlerBuilder<TEvent> : IEventHandlerBuilder<TEvent> where TEvent : IEvent
    {
        private IServiceCollection _services;

        public EventHandlerBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IEventHandlerBuilder<TEvent> AddHandler<TEventHandler>() where TEventHandler : IEventHandler<TEvent>
        {
            if (_services.Any(s => s.ServiceType == typeof(IEventHandler<TEvent>) && s.ImplementationType == typeof(TEventHandler)))
            {
                throw new InvalidOperationException("Duplicate event handler registration found. You can register multiple event handlers but you must register a concrete event handler only once.");
            }

            Type serviceType = typeof(IEventHandler<TEvent>);

            _services.AddTransient(serviceType, typeof(TEventHandler));

            return this;
        }
    }
}
