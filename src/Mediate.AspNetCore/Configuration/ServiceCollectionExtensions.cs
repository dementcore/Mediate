using Mediate.AspNetCore;
using Mediate.AspNetCore.Configuration.Builders;
using Mediate.Core.Abstractions;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
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

       
    }
}
