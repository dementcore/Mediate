// File: QueuedEventWrapper.cs
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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.BackgroundEventDispatch.Wrappers
{
    /// <summary>
    /// Wrapper class to represent a event into the event queue
    /// </summary>
    internal abstract class QueuedEventWrapperBase
    {
        protected QueuedEventWrapperBase(string eventName)
        {
            EventName = eventName;
        }

        public string EventName { get; }

        internal abstract Task Handle(CancellationToken cancellationToken);
    }

    /// <summary>
    /// Wrapper class to represent a event into the event queue
    /// </summary>
    internal sealed class QueuedEventWrapper<TEvent> : QueuedEventWrapperBase where TEvent : IEvent
    {
        private TEvent Event { get; }

        private readonly IEnumerable<IEventHandler<TEvent>> _handlers;

        public QueuedEventWrapper(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers)
        : base(@event.GetType().Name)
        {
            Event = @event;
            _handlers = handlers;
        }

        internal override async Task Handle(CancellationToken cancellationToken)
        {
            List<Exception> exceptions = new List<Exception>();

            foreach (var handler in _handlers)
            {
                try
                {
                    await handler.Handle(Event, cancellationToken);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }
    }
}
