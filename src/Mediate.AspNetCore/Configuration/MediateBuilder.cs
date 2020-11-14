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

        public IMediateBuilder AddServiceProviderMiddlewareProvider()
        {
            if (_services.Any(s => s.ServiceType == typeof(IMiddlewareProvider)))
            {
                throw new InvalidOperationException("You have already registered a middleware provider");
            }

            _services.AddTransient<IMiddlewareProvider, ServiceProviderMiddlewareProvider>();

            return this;
        }

        public IMediateBuilder AddCustomMiddlewareProvider<TMiddlewareProvider>() where TMiddlewareProvider : IMiddlewareProvider
        {
            if (_services.Any(s => s.ServiceType == typeof(IMiddlewareProvider)))
            {
                throw new InvalidOperationException("You have already registered a middleware provider");
            }

            _services.AddTransient(typeof(IMiddlewareProvider), typeof(TMiddlewareProvider));

            return this;
        }
    }
}
