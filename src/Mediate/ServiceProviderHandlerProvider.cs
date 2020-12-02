using Mediate.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mediate
{
    /// <summary>
    /// Message and event handler provider from IServiceProvider
    /// </summary>
    public sealed class ServiceProviderHandlerProvider : IHandlerProvider,IDisposable
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ServiceProviderHandlerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets all event handlers from an event from the IServiceProvider
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <returns>All registered handlers for that event</returns>
        public Task<IEnumerable<IEventHandler<TEvent>>> GetHandlers<TEvent>() where TEvent : IEvent
        {
            IEnumerable<IEventHandler<TEvent>> services = _serviceProvider.GetServices<IEventHandler<TEvent>>();

            IEnumerable<IEventHandler<TEvent>> handlers = new List<IEventHandler<TEvent>>();

            if (services is IEnumerable<IEventHandler<TEvent>>)
            {
                handlers = services;
            }

            return Task.FromResult(handlers);
        }

        /// <summary>
        /// Gets a query handler from a concrete query from the IServiceProvider
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <typeparam name="TResult">Query response type</typeparam>
        /// <returns>Registered handler for that query</returns>
        public Task<IQueryHandler<TQuery, TResult>> GetHandler<TQuery, TResult>() where TQuery : IQuery<TResult>
        {
            IQueryHandler<TQuery, TResult> service = _serviceProvider.GetService<IQueryHandler<TQuery, TResult>>();

            IQueryHandler<TQuery, TResult> handler = default;

            if (service is IQueryHandler<TQuery, TResult>)
            {
                handler = service;
            }

            return Task.FromResult(handler);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
