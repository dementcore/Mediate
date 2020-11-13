using Mediate.Core;
using Mediate.Core.Abstractions;
using Mediate.AspNetCore.DispatchStrategies;
using Mediate.AspNetCore.HostedService;
using Mediate.AspNetCore.Queue;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Mediate.AspNetCore
{
    internal sealed class MediateBuilder : IMediateBuilder
    {
        private readonly IServiceCollection _services;

        public MediateBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IMediateBuilder AddCustomHandlerProvider<THandlerProvider>()
            where THandlerProvider : IHandlerProvider
        {
            if (_services.Any(s => s.ServiceType == typeof(IHandlerProvider)))
            {
                throw new InvalidOperationException("You have already registered a handler provider");
            }

            _services.AddTransient(typeof(IHandlerProvider), typeof(THandlerProvider));

            return this;
        }

        public IMediateBuilder AddServiceProviderHandlerProvider()
        {
            if (_services.Any(s => s.ServiceType == typeof(IHandlerProvider)))
            {
                throw new InvalidOperationException("You have already registered a handler provider");
            }

            _services.AddTransient<IHandlerProvider, ServiceProviderHandlerProvider>();

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
