using Mediate.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mediate.AspNetCore
{
    internal sealed class ServiceProviderExceptionHandlerProvider : IExceptionHandlerProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderExceptionHandlerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<IExceptionHandler<TException>> GetHandler<TException>(TException exception) where TException : Exception
        {
            Type handlerType = typeof(IMessageHandler<,>).MakeGenericType(exception.GetType());

            var service = _serviceProvider.GetService(handlerType);

            IExceptionHandler<TException> handler=default;

            if (service is IExceptionHandler<TException>)
            {
                handler = service as IExceptionHandler<TException>;
            }

            return Task.FromResult(handler);
        }
    }
}
