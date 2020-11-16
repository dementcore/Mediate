using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Abstractions
{
    /// <summary>
    /// Interface to implement a middleware to process a query before it reaches it's handler.
    /// <typeparam name="TQuery">Query type</typeparam>
    /// <typeparam name="TResult">Query response type</typeparam>
    /// </summary>
    public interface IQueryMiddleware<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        /// <summary>
        /// Invoke the middleware logic
        /// </summary>
        /// <param name="query">Query object</param>
        /// <param name="cancellationToken"></param>
        /// <param name="next">Delegate that encapsulates a call to the next element in the pipeline</param>
        /// <returns></returns>
        Task<TResult> Invoke(TQuery query, CancellationToken cancellationToken, NextMiddlewareDelegate<TResult> next);
    }
}
