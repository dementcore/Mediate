using Mediate.Abstractions;
using Mediate.Queue;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Queue
{
    internal class EventQueue : IEventQueue
    {
        private ConcurrentQueue<QueuedEvent> _eventQueue =
            new System.Collections.Concurrent.ConcurrentQueue<QueuedEvent>();

        public async Task<QueuedEvent> DequeueEvent(CancellationToken cancellationToken = default)
        {
            QueuedEvent eventHandler;
            
            lock (this)
            {
                _eventQueue.TryDequeue(out eventHandler);
            }

            return await Task.FromResult(eventHandler);
        }

        public void EnqueueEvent(QueuedEvent @event)
        {
            lock (this)
            {
                _eventQueue.Enqueue(@event);
            }
        }

        public bool HasEvents()
        {
            lock (this)
            {
                return !_eventQueue.IsEmpty;
            }
        }
    }
}
