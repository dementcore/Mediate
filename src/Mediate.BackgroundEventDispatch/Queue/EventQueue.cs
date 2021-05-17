// File: EventQueue.cs
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

using Mediate.BackgroundEventDispatch.Wrappers;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.BackgroundEventDispatch.Queue
{
    /// <summary>
    /// Event queue for the EventQueueDispatchStrategy. <br/>
    /// This class is public to allow registration into DI containers like Autofac, Unity, etc.<br/>
    /// This shouldn't be used directly from user code. <br/>
    /// </summary>
    public sealed class EventQueue
    {
        private readonly ConcurrentQueue<QueuedEventWrapperBase> _eventQueue =
            new ConcurrentQueue<QueuedEventWrapperBase>();


        SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(10);

        internal async Task<QueuedEventWrapperBase> DequeueEvent(CancellationToken cancellationToken)
        {
            QueuedEventWrapperBase eventHandler;

            await _semaphoreSlim.WaitAsync(cancellationToken);

            _eventQueue.TryDequeue(out eventHandler);

            _semaphoreSlim.Release();

            return eventHandler;
        }

        internal async Task EnqueueEvent(QueuedEventWrapperBase @event, CancellationToken cancellationToken)
        {
            await _semaphoreSlim.WaitAsync(cancellationToken);

            _eventQueue.Enqueue(@event);

            _semaphoreSlim.Release();

        }

        internal async Task<bool> HasEvents(CancellationToken cancellationToken)
        {
            await _semaphoreSlim.WaitAsync(cancellationToken);

            bool empty = !_eventQueue.IsEmpty;

            _semaphoreSlim.Release();

            return empty;
        }
    }
}
