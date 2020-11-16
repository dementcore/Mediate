using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Abstractions
{
    /// <summary>
    /// Interface for implement a query middleware provider
    /// </summary>
    public interface IQueryMiddlewareProvider
    {
        /// <summary>
        /// Gets all query middlewares from a query
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <typeparam name="TResult">Query response type</typeparam>
        /// <returns>All registered middlewares for that query</returns>
        Task<IEnumerable<IQueryMiddleware<TQuery, TResult>>> GetMiddlewares<TQuery, TResult>() where TQuery : IQuery<TResult>;
    }
}
