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
    /// <summary>
    /// Helper class to configure the query handler and middlewares
    /// </summary>
    /// <typeparam name="TQuery">Query type</typeparam>
    /// <typeparam name="TResult">Query response type</typeparam>
    public sealed class QueryHandlerBuilder<TQuery, TResult> : IQueryHandlerBuilder<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="services"></param>
        public QueryHandlerBuilder(IServiceCollection services)
        {
            _services = services;
        }

        /// <summary>
        /// Adds a middleware for <typeparamref name="TQuery"/>
        /// </summary>
        /// <typeparam name="TQueryMiddleware">Query middleware type</typeparam>
        /// <returns></returns>
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

        /// <summary>
        /// Adds a handler for <typeparamref name="TQuery"/>
        /// </summary>
        /// <typeparam name="TQueryHandler">Query handler type</typeparam>
        /// <returns></returns>
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
