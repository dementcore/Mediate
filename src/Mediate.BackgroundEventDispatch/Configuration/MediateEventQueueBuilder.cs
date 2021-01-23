using Mediate.BackgroundEventDispatch.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Mediate.BackgroundEventDispatch.Configuration
{
    internal sealed class MediateEventQueueBuilder : IMediateEventQueueBuilder
    {
        private readonly IServiceCollection _services;

        public MediateEventQueueBuilder(IServiceCollection services)
        {
            _services = services;
        }

        /// <summary>
        /// Registers a custom EventQueueExceptionHandler
        /// </summary>
        /// <typeparam name="TExceptionHandler">EventQueueExceptionHandler implementation type</typeparam>
        /// <returns></returns>
        public IMediateEventQueueBuilder AddCustomExceptionHandler<TExceptionHandler>() where TExceptionHandler : IEventQueueExceptionHandler
        {
            if (_services.Any(s => s.ServiceType == typeof(TExceptionHandler)))
            {
                throw new InvalidOperationException("You have already registered an EventQueueExceptionHandler");
            }

            _services.AddSingleton(typeof(IEventQueueExceptionHandler), typeof(TExceptionHandler));

            return this;
        }

        /// <summary>
        /// Registers the DefaultEventQueueExceptionHandler that logs the error.
        /// </summary>
        /// <returns></returns>
        public IMediateEventQueueBuilder AddDefaultExceptionHandler()
        {
            if (_services.Any(s => s.ServiceType == typeof(IEventQueueExceptionHandler)))
            {
                throw new InvalidOperationException("You have already registered an EventQueueExceptionHandler");
            }

            _services.AddSingleton<IEventQueueExceptionHandler, DefaultEventQueueExceptionHandler>();

            return this;
        }
    }
}
