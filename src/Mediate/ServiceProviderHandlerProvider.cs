﻿using Mediate.Abstractions;
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
    public sealed class ServiceProviderHandlerProvider : IHandlerProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderHandlerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

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
    }
}