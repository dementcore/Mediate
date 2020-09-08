using Mediate;
using Mediate.Abstractions;
using Mediate.AspNetCore;
using Mediate.Queue;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediate(this IServiceCollection services)
        {
            services.AddTransient<IMediator, Mediator>();
            services.AddTransient<IExceptionHandlerProvider, ServiceProviderExceptionHandlerProvider>();
            services.AddTransient<IMessageHandlerProvider, ServiceProviderHandlerProvider>();
            services.AddSingleton<IEventQueue, EventQueue>();
            services.AddTransient<IExceptionHandler<Exception>, DefaultExceptionHandler>();
            services.AddHostedService<EventDispatcherService>();

            return services;
        }

        public static void AddMediateEventHandler<TEvent, TEventHandler>(this IServiceCollection services)
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent>
        {
            if (services.Any(s => s.ServiceType == typeof(IEventHandler<TEvent>) && s.ImplementationType == typeof(TEventHandler)))
            {
                return;
            }

            Type serviceType = typeof(IEventHandler<TEvent>);

            services.AddTransient(serviceType, typeof(TEventHandler));
        }

        public static void AddMediateMessageHandler<TResult, TMessage, TMessageHandler>(this IServiceCollection services)
          where TMessageHandler : IMessageHandler<TMessage, TResult>
            where TMessage : IMessage<TResult>
        {
            if (services.Any(s => s.ServiceType == typeof(IMessageHandler<TMessage, TResult>)))
            {
                return;
            }

            Type serviceType = typeof(IMessageHandler<TMessage, TResult>);

            services.AddTransient(serviceType, typeof(TMessageHandler));
        }
    }
}
