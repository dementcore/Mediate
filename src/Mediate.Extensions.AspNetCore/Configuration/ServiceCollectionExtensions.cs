using Mediate.Core.Abstractions;
using Mediate.Extensions.AspNetCore.Configuration.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Mediate.Extensions.AspNetCore
{
    public static class ServiceCollectionExtensions
    {

        public static IMediateBuilder AddMediate(this IServiceCollection services)
        {
            return new MediateBuilder(services);
        }

        public static IQueryHandlerBuilder<TQuery, TResult> ForMediateQuery<TQuery, TResult>(this IServiceCollection services) where TQuery : IQuery<TResult>
        {
            return new QueryHandlerBuilder<TQuery, TResult>(services);
        }

        public static IEventHandlerBuilder<TEvent> ForMediateEvent<TEvent>(this IServiceCollection services) where TEvent : IEvent
        {
            return new EventHandlerBuilder<TEvent>(services);
        }

        public static void AddMediateGenericEventHandler(this IServiceCollection services,Type genericHandler)
        {
            if (services.Any(s => s.ServiceType == typeof(IEventHandler<>) && s.ImplementationType == genericHandler))
            {
                return;
            }

            if (genericHandler.GetInterface("IEventHandler`1") == null)
            {
                throw new InvalidOperationException("To register a generic eventHandler the handler must implement IEventHandler interface.");

            }

            if (!genericHandler.IsGenericType)
            {
                throw new InvalidOperationException("To register a generic eventHandler the handler must be a generic type.");
            }

            Type serviceType = typeof(IEventHandler<>);

            services.AddTransient(serviceType, genericHandler.GetGenericTypeDefinition());
        }

        ///// <summary>
        ///// Registers an event handler for <typeparamref name="TEvent"/>
        ///// </summary>
        ///// <typeparam name="TEvent">Event type</typeparam>
        ///// <typeparam name="TEventHandler">Event handler type </typeparam>
        //public static void AddMediateEventHandler<TEvent, TEventHandler>(this IServiceCollection services)
        //    where TEvent : IEvent
        //    where TEventHandler : IEventHandler<TEvent>
        //{
        //    if (services.Any(s => s.ServiceType == typeof(IEventHandler<TEvent>) && s.ImplementationType == typeof(TEventHandler)))
        //    {
        //        return;
        //    }

        //    Type serviceType = typeof(IEventHandler<TEvent>);

        //    services.AddTransient(serviceType, typeof(TEventHandler));
        //}

        ///// <summary>
        ///// Registers a query handler for <typeparamref name="TQuery"/>
        ///// </summary>
        ///// <typeparam name="TQuery">Query type</typeparam>
        ///// <typeparam name="TResult">Query response type </typeparam>
        ///// <typeparam name="TQueryHandler">Query handler type </typeparam>
        //public static void AddMediateQueryHandler<TQuery, TResult, TQueryHandler>(this IServiceCollection services)
        //  where TQueryHandler : IQueryHandler<TQuery, TResult>
        //    where TQuery : IQuery<TResult>
        //{
        //    if (services.Any(s => s.ServiceType == typeof(IQueryHandler<TQuery, TResult>)))
        //    {
        //        return;
        //    }

        //    Type serviceType = typeof(IQueryHandler<TQuery, TResult>);

        //    services.AddTransient(serviceType, typeof(TQueryHandler));
        //}
    }
}
