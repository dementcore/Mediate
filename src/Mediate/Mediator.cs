// File: Mediator.cs
// The MIT License
//
// Copyright (c) 2021 DementCore
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//

using Mediate.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate
{
    /// <summary>
    /// Default mediator implementation
    /// </summary>
    public sealed class Mediator : IMediator, IDisposable
    {
        private readonly IQueryHandlerProvider _queryHandlerProvider;
        private readonly IEventHandlerProvider _eventHandlerProvider;

        private readonly IQueryMiddlewareProvider _queryMiddlewareProvider;
        private readonly IEventMiddlewareProvider _eventMiddlewareProvider;

        private readonly IEventDispatchStrategy _eventDispatchStrategy;

        private ConcurrentDictionary<Type, Wrappers.QueryWrapperBase> _queryWrappersCache = new ConcurrentDictionary<Type, Wrappers.QueryWrapperBase>();

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
            await Dispatch(@event, CancellationToken.None).ConfigureAwait(false);
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

            if (!handlers.Any())
            {
                throw new InvalidOperationException($"There isn't any registered event handler for {typeof(TEvent).Name}");

            }

            IEnumerable<IEventMiddleware<TEvent>> middlewares = await _eventMiddlewareProvider.GetMiddlewares<TEvent>();

            async Task pipelineEnd()
            {
                await _eventDispatchStrategy.Dispatch(@event, handlers, cancellationToken);
            }

            NextMiddlewareDelegate pipeline = middlewares
                .Reverse()
                .Aggregate((NextMiddlewareDelegate)pipelineEnd, (next, middleware) => {
                    return async delegate {
                        await middleware.Invoke(@event, cancellationToken, next);
                    };
                });

            await pipeline();
        }

        /// <summary>
        /// Sends a query to his handler and returns a response
        /// </summary>
        /// <typeparam name="TResult">Response type</typeparam>
        /// <param name="query">Query data</param>
        /// <returns></returns>
        public async Task<TResult> Send<TResult>(IQuery<TResult> query)
        {
            return await Send(query, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a query to his handler and returns a response
        /// </summary>
        /// <typeparam name="TResult">Response type</typeparam>
        /// <param name="query">Query data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public async Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            Type queryType = query.GetType();

            Wrappers.QueryWrapperBase queryWrapper = _queryWrappersCache.GetOrAdd(queryType, (queryType) => {
                Type wrapperType = typeof(Wrappers.QueryWrapper<,>).MakeGenericType(query.GetType(), typeof(TResult));

                return (Wrappers.QueryWrapperBase)Activator.CreateInstance(wrapperType, _queryHandlerProvider, _queryMiddlewareProvider);
            });

            return (TResult)await queryWrapper.Handle(query, cancellationToken);

        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}