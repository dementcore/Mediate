using Mediate.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mediate.AspNetCore
{
    public sealed class ServiceProviderMiddlewareProvider : IMiddlewareProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderMiddlewareProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

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
