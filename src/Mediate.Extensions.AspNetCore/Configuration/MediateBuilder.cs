using Mediate.Core;
using Mediate.Core.Abstractions;
using Mediate.Core.DispatchStrategies;
using Mediate.Extensions.AspNetCore.HostedService;
using Mediate.Extensions.AspNetCore.Queue;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Mediate.Extensions.AspNetCore
{
    internal sealed class MediateBuilder : IMediateBuilder
    {
        private readonly IServiceCollection _services;

        public MediateBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IMediateBuilder AddCustomEventHandlerProvider<TEventHandlerProvider>()
            where TEventHandlerProvider : IEventHandlerProvider
        {
            if (_services.Any(s => s.ServiceType == typeof(IEventHandlerProvider)))
            {
                throw new InvalidOperationException("You have already registered an event handler provider");
            }

            _services.AddTransient(typeof(IEventHandlerProvider), typeof(TEventHandlerProvider));

            return this;
        }

        public IMediateBuilder AddCustomQueryHandlerProvider<TQueryHandlerProvider>()
            where TQueryHandlerProvider : IQueryHandlerProvider
        {
            if (_services.Any(s => s.ServiceType == typeof(IQueryHandlerProvider)))
            {
                throw new InvalidOperationException("You have already registered an query handler provider");
            }

            _services.AddTransient(typeof(IQueryHandlerProvider), typeof(TQueryHandlerProvider));

            return this;
        }

        public IMediateBuilder AddServiceProviderHandlerProvider()
        {
            if (_services.Any(s => s.ServiceType == typeof(IQueryHandlerProvider)))
            {
                throw new InvalidOperationException("You have already registered an query handler provider");
            }

            if (_services.Any(s => s.ServiceType == typeof(IEventHandlerProvider)))
            {
                throw new InvalidOperationException("You have already registered an event handler provider");
            }

            _services.AddTransient<IQueryHandlerProvider, ServiceProviderHandlerProvider>();
            _services.AddTransient<IEventHandlerProvider, ServiceProviderHandlerProvider>();

            return this;
        }

        public IMediateBuilder AddCustomMediator<TMediator>() where TMediator : IMediator
        {
            if (_services.Any(s => s.ServiceType == typeof(IMediator)))
            {
                throw new InvalidOperationException("You have already registered a IMediator implementation");
            }

            _services.AddTransient(typeof(IMediator), typeof(TMediator));

            return this;
        }

        public IMediateBuilder AddDefaultMediator()
        {
            if (_services.Any(s => s.ServiceType == typeof(IMediator)))
            {
                throw new InvalidOperationException("You have already registered a IMediator implementation");
            }

            _services.AddTransient<IMediator, Mediator>();

            return this;
        }

        public IMediateBuilder AddParallelEventDispatchStrategy()
        {
            if (_services.Any(s => s.ServiceType == typeof(IEventDispatchStrategy)))
            {
                throw new InvalidOperationException("You have already registered an event dispatch strategy");
            }

            _services.AddTransient<IEventDispatchStrategy, ParallelEventDispatchStrategy>();

            return this;
        }

        public IMediateBuilder AddEventQueueDispatchStrategy()
        {
            if (_services.Any(s => s.ServiceType == typeof(IEventDispatchStrategy)))
            {
                throw new InvalidOperationException("You have already registered an event dispatch strategy");
            }

            if (_services.Any(s => s.ServiceType == typeof(EventQueue)))
            {
                throw new InvalidOperationException("You have already registered the EventQueue");
            }

            if (_services.Any(s => s.ImplementationType == typeof(EventDispatcherService)))
            {
                throw new InvalidOperationException("You have already registered the EventDispatcherService hosted service");
            }

            _services.AddTransient<IEventDispatchStrategy, EventQueueDispatchStrategy>();
            _services.AddSingleton<EventQueue>();
            _services.AddHostedService<EventDispatcherService>();

            return this;
        }

        public IMediateBuilder AddSequentialEventDispatchStrategy()
        {
            if (_services.Any(s => s.ServiceType == typeof(IEventDispatchStrategy)))
            {
                throw new InvalidOperationException("You have already registered an event dispatch strategy");
            }

            _services.AddTransient<IEventDispatchStrategy, SequentialEventDispatchStrategy>();

            return this;
        }

        public IMediateBuilder AddCustomDispatchStrategy<TDispatchStrategy>()
          where TDispatchStrategy : IEventDispatchStrategy
        {
            if (_services.Any(s => s.ServiceType == typeof(IEventDispatchStrategy)))
            {
                throw new InvalidOperationException("You have already registered an event dispatch strategy");
            }

            _services.AddTransient(typeof(IEventDispatchStrategy), typeof(TDispatchStrategy));

            return this;
        }
    }
}
