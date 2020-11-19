using Mediate.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate
{
    /// <summary>
    /// Default mediator implementation
    /// </summary>
    public sealed class Mediator : IMediator
    {
        private readonly IQueryHandlerProvider _queryHandlerProvider;
        private readonly IEventHandlerProvider _eventHandlerProvider;

        private readonly IQueryMiddlewareProvider _queryMiddlewareProvider;
        private readonly IEventMiddlewareProvider _eventMiddlewareProvider;

        private readonly IEventDispatchStrategy _eventDispatchStrategy;

        /// <summary>
        /// Mediator constructor
        /// </summary>
        /// <param name="provider">Event and query handlers provider</param>
        /// <param name="middlewareProvider">Event and query middlewares provider</param>
        /// <param name="eventDispatchStrategy">Event dispatching strategy to use</param>
        public Mediator(IHandlerProvider provider, IMiddlewareProvider middlewareProvider, IEventDispatchStrategy eventDispatchStrategy)
        {
            _queryHandlerProvider = provider;
            _eventHandlerProvider = provider;
            _eventDispatchStrategy = eventDispatchStrategy;

            _queryMiddlewareProvider = middlewareProvider;
            _eventMiddlewareProvider = middlewareProvider;
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
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            IEnumerable<IEventHandler<TEvent>> handlers = await _eventHandlerProvider.GetHandlers<TEvent>();

            if (handlers.Count() > 0)
            {
                IEnumerable<IEventMiddleware<TEvent>> middlewares = await _eventMiddlewareProvider.GetMiddlewares<TEvent>();

                NextMiddlewareDelegate pipelineEnd = async delegate
                {
                    await _eventDispatchStrategy.Dispatch(@event, handlers, cancellationToken);
                };

                NextMiddlewareDelegate pipeline = middlewares
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
        /// <param name="query">Query data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public async Task<TResult> Send<TQuery, TResult>(TQuery query, CancellationToken cancellationToken)
            where TQuery : IQuery<TResult>
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            IQueryHandler<TQuery, TResult> handler = await _queryHandlerProvider.GetHandler<TQuery, TResult>();

            if (handler != null)
            {

                IEnumerable<IQueryMiddleware<TQuery, TResult>> middlewares = await _queryMiddlewareProvider.GetMiddlewares<TQuery, TResult>();

                NextMiddlewareDelegate<TResult> pipelineEnd = async delegate
                {
                    return await handler.Handle(query, cancellationToken);
                };

                NextMiddlewareDelegate<TResult> pipeline = middlewares
                    .Reverse()
                    .Aggregate(pipelineEnd, (next, middleware) =>
                    {
                        return async delegate
                        {
                            return await middleware.Invoke(query, cancellationToken, next);
                        };
                    });

                return await pipeline();
            }

            return default;
        }

    }
}
