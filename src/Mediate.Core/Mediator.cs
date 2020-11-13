using Mediate.Core.Abstractions;
using Mediate.Core.DefaultMiddlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public Mediator(IHandlerProvider provider, IEventDispatchStrategy eventDispatchStrategy)
        {
            _queryHandlerProvider = provider;
            _eventHandlerProvider = provider;
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

                NextMiddlewareDelegate pipelineEnd = async delegate
                {
                    await _eventDispatchStrategy.ExecuteStrategy(@event, handlers, cancellationToken).ConfigureAwait(false);

                };

                NextMiddlewareDelegate pipeline = MockMiddlewares.middles
                    .Reverse()
                    .Aggregate(pipelineEnd, (next, middleware) =>
                    {
                        return async delegate
                        {
                            await middleware.Invoke(@event, cancellationToken, next);
                        };
                    });

                await pipeline();

              

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
