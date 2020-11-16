using Mediate.Abstractions;
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
    public interface IQueryHandlerBuilder<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        /// <summary>
        /// Adds a handler for <typeparamref name="TQuery"/>
        /// </summary>
        /// <typeparam name="TQueryHandler">Query handler type</typeparam>
        /// <returns></returns>
        IQueryHandlerBuilder<TQuery, TResult> AddHandler<TQueryHandler>() where TQueryHandler : IQueryHandler<TQuery, TResult>;

        /// <summary>
        /// Adds a middleware for <typeparamref name="TQuery"/>
        /// </summary>
        /// <typeparam name="TQueryMiddleware">Query middleware type</typeparam>
        /// <returns></returns>
        IQueryHandlerBuilder<TQuery, TResult> AddMiddleware<TQueryMiddleware>() where TQueryMiddleware : IQueryMiddleware<TQuery, TResult>;
    }
}
