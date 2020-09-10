using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Extensions.AspNetCore.Queue
{
    public sealed class EventQueue : IEventQueue
    {
        private readonly ConcurrentQueue<QueuedEventWrapperBase> _eventQueue =
            new ConcurrentQueue<QueuedEventWrapperBase>();

        private readonly object lockObj = new object();
        public async Task<QueuedEventWrapperBase> DequeueEvent()
        {
            return await DequeueEvent(default).ConfigureAwait(false);
        }

        public async Task<QueuedEventWrapperBase> DequeueEvent(CancellationToken cancellationToken)
        {
            QueuedEventWrapperBase eventHandler;

            lock (lockObj)
            {
                _eventQueue.TryDequeue(out eventHandler);
            }

            return await Task.FromResult(eventHandler).ConfigureAwait(false);
        }

        public void EnqueueEvent(QueuedEventWrapperBase @event)
        {
            lock (lockObj)
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
