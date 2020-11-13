using Mediate.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mediate.AspNetCore
{
    /// <summary>
    /// Message and event handler provider from IServiceProvider
    /// </summary>
    public sealed class ServiceProviderHandlerProvider : IHandlerProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderHandlerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<IEnumerable<IEventHandler<TEvent>>> GetEventHandlers<TEvent>(IEvent @event) where TEvent : IEvent
        {
            Type handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());

            var services = _serviceProvider.GetServices(handlerType);

            IEnumerable<IEventHandler<TEvent>> handlers = new List<IEventHandler<TEvent>>();

            if (services is IEnumerable<IEventHandler<TEvent>>)
            {
                handlers = services as IEnumerable<IEventHandler<TEvent>>;
            }

            return Task.FromResult(handlers);
        }

        public Task<IQueryHandler<TMessage, TResult>> GetQueryHandler<TMessage, TResult>(TMessage message) where TMessage : IQuery<TResult>
        {
            Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(message.GetType(), typeof(TResult));

            var service = _serviceProvider.GetService(handlerType);

            IQueryHandler<TMessage, TResult> handler = default;

            if (service is IQueryHandler<TMessage, TResult>)
            {
                handler = service as IQueryHandler<TMessage, TResult>;
            }

            return Task.FromResult(handler);
        }
    }
}
