using Mediate.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Configuration.Builders
{
    public sealed class QueryHandlerBuilder<TQuery, TResult> : IQueryHandlerBuilder<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private IServiceCollection _services;

        public QueryHandlerBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IQueryHandlerBuilder<TQuery, TResult> AddMiddleware<TQueryMiddleware>() where TQueryMiddleware : IQueryMiddleware<TQuery, TResult>
        {
            if (_services.Any(s => s.ServiceType == typeof(IQueryMiddleware<TQuery, TResult>) && s.ImplementationType == typeof(TQueryMiddleware)))
            {
                throw new InvalidOperationException("Duplicate query middleware found. You can register multiple middleware for a concrete query but you must register a concrete query middleware only once.");
            }

            Type serviceType = typeof(IQueryMiddleware<TQuery, TResult>);

            _services.AddTransient(serviceType, typeof(TQueryMiddleware));

            return this;
        }

        IQueryHandlerBuilder<TQuery, TResult> IQueryHandlerBuilder<TQuery, TResult>.AddHandler<TQueryHandler>()
        {
            if (_services.Any(s => s.ServiceType == typeof(IQueryHandler<TQuery, TResult>)))
            {
                throw new InvalidOperationException("Duplicate query handler found. You can't register multiple query handler for a concrete query.");
            }

            Type serviceType = typeof(IQueryHandler<TQuery, TResult>);

            _services.AddTransient(serviceType, typeof(TQueryHandler));

            return this;
        }
    }
}
