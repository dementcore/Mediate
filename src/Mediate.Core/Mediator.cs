using Mediate.Core.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Core
{
    /// <summary>
    /// Default mediator implementation
    /// </summary>
    public sealed class Mediator : IMediator
    {
        private readonly IQueryHandlerProvider _queryHandlerProvider;
        private readonly IEventHandlerProvider _eventHandlerProvider;
        private readonly IEventDispatchStrategy _eventDispatchStrategy;

        public Mediator(IQueryHandlerProvider queryHandlerProvider, IEventHandlerProvider eventHandlerProvider,
            IEventDispatchStrategy eventDispatchStrategy)
        {
            _queryHandlerProvider = queryHandlerProvider;
            _eventHandlerProvider = eventHandlerProvider;
            _eventDispatchStrategy = eventDispatchStrategy;
        }

        /// <summary>
        /// Dispatchs an event into configured dispatch strategy
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="event">Event data</param>
        /// <returns></returns>
        public async Task Dispatch<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            await Dispatch(@event, default).ConfigureAwait(false);
        }

        /// <summary>
        /// Dispatchs an event into configured dispatch strategy
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="event">Event data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public async Task Dispatch<TEvent>(TEvent @event, CancellationToken cancellationToken)
            where TEvent : IEvent
        {
            IEnumerable<IEventHandler<TEvent>> handlers = await _eventHandlerProvider.GetEventHandlers<TEvent>(@event).ConfigureAwait(false);

            if (handlers.Count() > 0)
            {
                await _eventDispatchStrategy.ExecuteHandlers(@event, handlers, cancellationToken).ConfigureAwait(false);
            }
        }


        /// <summary>
        /// Sends a query to his handler and returns a response
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <typeparam name="TResult">Response type</typeparam>
        /// <param name="query">Query data</param>
        /// <returns></returns>
        public async Task<TResult> Send<TQuery, TResult>(TQuery query)
            where TQuery : IQuery<TResult>
        {
            return await Send<TQuery, TResult>(query, default).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a query to his handler and returns a response
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <typeparam name="TResult">Response type</typeparam>
        /// <param name="message">Query data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public async Task<TResult> Send<TQuery, TResult>(TQuery query, CancellationToken cancellationToken)
            where TQuery : IQuery<TResult>
        {
            var handler = await _queryHandlerProvider.GetQueryHandler<TQuery, TResult>(query).ConfigureAwait(false);

            if (handler != null)
            {
                return await handler.Handle(query, cancellationToken).ConfigureAwait(false);
            }

            return default;
        }

    }
}
