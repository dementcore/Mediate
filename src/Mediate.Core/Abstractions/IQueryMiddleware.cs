using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Core.Abstractions
{
    /// <summary>
    /// Interface to implement a middleware to process a query before it reaches it's handler.
    /// <typeparam name="TQuery">Query type</typeparam>
    /// <typeparam name="TResult">Query response type</typeparam>
    /// </summary>
    public interface IQueryMiddleware<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> Invoke(TQuery message, CancellationToken cancellationToken, NextMiddlewareDelegate<TResult> next);
    }
}
