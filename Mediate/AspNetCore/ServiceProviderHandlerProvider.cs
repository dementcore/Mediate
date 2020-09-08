using Mediate.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mediate.AspNetCore
{
    internal sealed class ServiceProviderHandlerProvider : IMessageHandlerProvider,IEventHandlerProvider
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

        public Task<IMessageHandler<TMessage, TResult>> GetMessageHandler<TMessage, TResult>(IMessage<TResult> message) where TMessage : IMessage<TResult>
        {
            Type handlerType = typeof(IMessageHandler<,>).MakeGenericType(message.GetType(), typeof(TResult));

            var service = _serviceProvider.GetService(handlerType);

            IMessageHandler<TMessage, TResult> handler = default;

            if (service is IMessageHandler<TMessage, TResult>)
            {
                handler = service as IMessageHandler<TMessage, TResult>;
            }

            return Task.FromResult(handler);
        }
    }
}
