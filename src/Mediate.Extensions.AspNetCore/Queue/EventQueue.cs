using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Extensions.AspNetCore.Queue
{
    public sealed class EventQueue : IEventQueue
    {
        private readonly ConcurrentQueue<QueuedEventWrapperBase> _eventQueue =
            new ConcurrentQueue<QueuedEventWrapperBase>();

        public async Task<QueuedEventWrapperBase> DequeueEvent(CancellationToken cancellationToken = default)
        {
            QueuedEventWrapperBase eventHandler;

            lock (this)
            {
                _eventQueue.TryDequeue(out eventHandler);
            }

            return await Task.FromResult(eventHandler);
        }

        public void EnqueueEvent(QueuedEventWrapperBase @event)
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
