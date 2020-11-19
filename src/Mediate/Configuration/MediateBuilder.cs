using Mediate.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Mediate.Configuration
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

            _services.AddScoped(typeof(IHandlerProvider), typeof(THandlerProvider));

            return this;
        }

        public IMediateBuilder AddServiceProviderHandlerProvider()
        {
            if (_services.Any(s => s.ServiceType == typeof(IHandlerProvider)))
            {
                throw new InvalidOperationException("You have already registered a handler provider");
            }

            _services.AddScoped<IHandlerProvider, ServiceProviderHandlerProvider>();

            return this;
        }

        public IMediateBuilder AddServiceProviderMiddlewareProvider()
        {
            if (_services.Any(s => s.ServiceType == typeof(IMiddlewareProvider)))
            {
                throw new InvalidOperationException("You have already registered a middleware provider");
            }

            _services.AddScoped<IMiddlewareProvider, ServiceProviderMiddlewareProvider>();

            return this;
        }

        public IMediateBuilder AddCustomMiddlewareProvider<TMiddlewareProvider>() where TMiddlewareProvider : IMiddlewareProvider
        {
            if (_services.Any(s => s.ServiceType == typeof(IMiddlewareProvider)))
            {
                throw new InvalidOperationException("You have already registered a middleware provider");
            }

            _services.AddScoped(typeof(IMiddlewareProvider), typeof(TMiddlewareProvider));

            return this;
        }
    }
}
