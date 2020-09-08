using Mediate.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Queue
{
    internal interface IEventQueue
    {
        bool HasEvents();

        void EnqueueEvent(QueuedEvent @event);

        Task<QueuedEvent> DequeueEvent(CancellationToken cancellationToken = default);
    }
}
