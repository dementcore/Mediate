using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Core.Abstractions
{
    /// <summary>
    /// Interface for implement a query handler for a concrete query
    /// </summary>
    /// <typeparam name="TQuery">Query type</typeparam>
    /// <typeparam name="TResult">Query response type</typeparam>
    public interface IQueryHandler<in TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        /// <summary>
        /// Handle the message
        /// </summary>
        /// <param name="message">Message data</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Message response</returns>
        Task<TResult> Handle(TQuery message, CancellationToken cancellationToken);

    }
}
