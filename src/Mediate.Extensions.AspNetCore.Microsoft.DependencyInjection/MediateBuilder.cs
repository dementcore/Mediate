using Mediate.Core;
using Mediate.Core.Abstractions;
using Mediate.Core.DispatchStrategies;
using Mediate.Extensions.AspNetCore.HostedService;
using Mediate.Extensions.AspNetCore.Queue;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Mediate.Extensions.AspNetCore.Microsoft.DependencyInjection
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
                throw new InvalidOperationException("You have already registered a IEventHandlerProvider implementation");
            }

            _services.AddTransient(typeof(IEventHandlerProvider), typeof(TEventHandlerProvider));

            return this;
        }

        public IMediateBuilder AddCustomMessageHandlerProvider<TMessageHandlerProvider>()
            where TMessageHandlerProvider : IMessageHandlerProvider
        {
            if (_services.Any(s => s.ServiceType == typeof(IMessageHandlerProvider)))
            {
                throw new InvalidOperationException("You have already registered a IMessageHandlerProvider implementation");
            }

            _services.AddTransient(typeof(IMessageHandlerProvider), typeof(TMessageHandlerProvider));

            return this;
        }

        public IMediateBuilder AddDefaultHandlerProvider()
        {
            if (_services.Any(s => s.ServiceType == typeof(IMessageHandlerProvider)))
            {
                throw new InvalidOperationException("You have already registered a IMessageHandlerProvider implementation");
            }

            if (_services.Any(s => s.ServiceType == typeof(IEventHandlerProvider)))
            {
                throw new InvalidOperationException("You have already registered a IEventHandlerProvider implementation");
            }

            _services.AddTransient<IMessageHandlerProvider, ServiceProviderHandlerProvider>();
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
                throw new InvalidOperationException("You have already registered a IEventDispatchStrategy implementation");
            }

            _services.AddTransient<IEventDispatchStrategy, ParallelEventDispatchStrategy>();

            return this;
        }

        public IMediateBuilder AddQueuedEventDispatchStrategy()
        {
            if (_services.Any(s => s.ServiceType == typeof(IEventDispatchStrategy)))
            {
                throw new InvalidOperationException("You have already registered a IEventDispatchStrategy implementation");
            }

            if (_services.Any(s => s.ServiceType == typeof(IEventQueue)))
            {
                throw new InvalidOperationException("You have already registered a IEventQueue implementation");
            }

            if (_services.Any(s => s.ImplementationType == typeof(EventDispatcherService)))
            {
                throw new InvalidOperationException("You have already registered the EventDispatcherService hosted service");
            }

            _services.AddTransient<IEventDispatchStrategy, QueueEventDispatchStrategy>();
            _services.AddSingleton<IEventQueue, EventQueue>();
            _services.AddHostedService<EventDispatcherService>();

            return this;
        }

        public IMediateBuilder AddSequentialEventDispatchStrategy()
        {
            if (_services.Any(s => s.ServiceType == typeof(IEventDispatchStrategy)))
            {
                throw new InvalidOperationException("You have already registered a IEventDispatchStrategy implementation");
            }

            _services.AddTransient<IEventDispatchStrategy, SequentialEventDispatchStrategy>();

            return this;
        }

        public IMediateBuilder AddCustomDispatchStrategy<TDispatchStrategy>()
          where TDispatchStrategy : IEventDispatchStrategy
        {
            if (_services.Any(s => s.ServiceType == typeof(IEventDispatchStrategy)))
            {
                throw new InvalidOperationException("You have already registered a IEventDispatchStrategy implementation");
            }

            _services.AddTransient(typeof(IEventDispatchStrategy), typeof(TDispatchStrategy));

            return this;
        }
    }
}
