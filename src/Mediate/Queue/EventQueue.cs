using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Queue
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


        internal QueuedEventWrapperBase DequeueEvent()
        {
            QueuedEventWrapperBase eventHandler;

            lock (lockObj)
            {
                _eventQueue.TryDequeue(out eventHandler);
            }

            return eventHandler;
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
