using Mediate.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mediate
{
    /// <summary>
    /// Message and event middleware provider from IServiceProvider
    /// </summary>
    public sealed class ServiceProviderMiddlewareProvider : IMiddlewareProvider
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ServiceProviderMiddlewareProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets all event middlewares from an event from the IServiceProvider
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <returns>All registered middlewares for that event</returns>
        public Task<IEnumerable<IEventMiddleware<TEvent>>> GetMiddlewares<TEvent>() where TEvent : IEvent
        {
            var services = _serviceProvider.GetServices<IEventMiddleware<TEvent>>();

            IEnumerable<IEventMiddleware<TEvent>> handlers = new List<IEventMiddleware<TEvent>>();

            if (services is IEnumerable<IEventMiddleware<TEvent>>)
            {
                handlers = services;
            }

            return Task.FromResult(handlers);
        }

        /// <summary>
        /// Gets all query middlewares from a query from the IServiceProvider
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <typeparam name="TResult">Query response type</typeparam>
        /// <returns>All registered middlewares for that query</returns>
        public Task<IEnumerable<IQueryMiddleware<TQuery, TResult>>> GetMiddlewares<TQuery, TResult>() where TQuery : IQuery<TResult>
        {
            var services = _serviceProvider.GetServices<IQueryMiddleware<TQuery, TResult>>();

            IEnumerable<IQueryMiddleware<TQuery, TResult>> handlers = new List<IQueryMiddleware<TQuery, TResult>>();

            if (services is IEnumerable<IQueryMiddleware<TQuery, TResult>>)
            {
                handlers = services;
            }

            return Task.FromResult(handlers);
        }
    }
}
