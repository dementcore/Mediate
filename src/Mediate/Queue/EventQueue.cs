using Mediate.Wrappers;
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
