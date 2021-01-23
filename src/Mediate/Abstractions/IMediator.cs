using Mediate.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Abstractions
{
    /// <summary>
    /// Mediator interface
    /// </summary>
    public interface IMediator
    {
        /// <summary>
        /// Sends a query to the mediator
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <typeparam name="TResult">Query response type</typeparam>
        /// <param name="query">Query data</param>
        /// <returns>Query response</returns>
        [Obsolete("This method is obsolete. Use IMediator.Send<TResult>(IQuery<TResult>) instead.", false)]
        Task<TResult> Send<TQuery, TResult>(TQuery query)
            where TQuery : IQuery<TResult>;

        /// <summary>
        /// Sends a query to the mediator
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <typeparam name="TResult">Query response type</typeparam>
        /// <param name="query">Query data</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Query response</returns>
        [Obsolete("This method is obsolete. Use IMediator.Send<TResult>(IQuery<TResult>, CancellationToken) instead.", false)]
        Task<TResult> Send<TQuery, TResult>(TQuery query, CancellationToken cancellationToken)
            where TQuery : IQuery<TResult>;

        /// <summary>
        /// Sends a query to the mediator
        /// </summary>
        /// <typeparam name="TResult">Query response type</typeparam>
        /// <param name="query">Query data</param>
        /// <returns>Query response</returns>
        Task<TResult> Send<TResult>(IQuery<TResult> query);

        /// <summary>
        /// Sends a query to the mediator
        /// </summary>
        /// <typeparam name="TResult">Query response type</typeparam>
        /// <param name="query">Query data</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Query response</returns>
        Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);

        /// <summary>
        /// Dispatchs an event to the mediator
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="event">Event data</param>
        /// <returns></returns>
        Task Dispatch<TEvent>(TEvent @event) where TEvent : IEvent;

        /// <summary>
        /// Dispatchs an event to the mediator
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="event">Event data</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Dispatch<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : IEvent;
    }
}
