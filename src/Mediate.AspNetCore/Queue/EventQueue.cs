using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.AspNetCore.Queue
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

        private readonly object lockObj = new object();
        
        internal async Task<QueuedEventWrapperBase> DequeueEvent()
        {
            return await DequeueEvent(default).ConfigureAwait(false);
        }

        internal async Task<QueuedEventWrapperBase> DequeueEvent(CancellationToken cancellationToken)
        {
            QueuedEventWrapperBase eventHandler;

            lock (lockObj)
            {
                _eventQueue.TryDequeue(out eventHandler);
            }

            return await Task.FromResult(eventHandler).ConfigureAwait(false);
        }

        internal void EnqueueEvent(QueuedEventWrapperBase @event)
        {
            lock (lockObj)
            {
                _eventQueue.Enqueue(@event);
            }
        }

        internal bool HasEvents()
        {
            lock (lockObj)
            {
                return !_eventQueue.IsEmpty;
            }
        }
    }
}
