// File: ParallelEventDispatchStrategy.cs
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
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.DispatchStrategies
{
    /// <summary>
    /// Event dispatch strategy that executes event handlers in parallel.
    /// </summary>
    public sealed class ParallelEventDispatchStrategy : IEventDispatchStrategy, IDisposable
    {
        /// <summary>
        /// Executes this strategy to dispatch an event
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="event">Event data</param>
        /// <param name="handlers">Event handlers</param>
        /// <returns></returns>
        public Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers) where TEvent : IEvent
        {
            return Dispatch(@event, handlers, default);
        }

        /// <summary>
        /// Executes this strategy to dispatch an event
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="event">Event data</param>
        /// <param name="handlers">Event handlers</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers, CancellationToken cancellationToken) where TEvent : IEvent
        {
            ConcurrentQueue<Exception> exceptions = new ConcurrentQueue<Exception>();

            await Parallel.ForEachAsync(handlers, async (handler, ct) => {
                try
                {
                    ct.ThrowIfCancellationRequested();

                    try
                    {
                        await handler.Handle(@event, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Enqueue(ex);
                    }
                }
                catch (OperationCanceledException)
                {
                    //no op
                }
            });

            if (!exceptions.IsEmpty)
            {
                throw new AggregateException(exceptions);
            }
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
