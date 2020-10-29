using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Core.Abstractions
{
    /// <summary>
    /// Interface for implement a query handler provider
    /// </summary>
    public interface IQueryHandlerProvider
    {
        /// <summary>
        /// Gets a query handler from a concrete query
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <typeparam name="TResult">Query response type</typeparam>
        /// <param name="query">Query data</param>
        /// <returns>Registered handler for that query</returns>
        Task<IQueryHandler<TQuery, TResult>> GetQueryHandler<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}
