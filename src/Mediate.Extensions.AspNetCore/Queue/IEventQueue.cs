using Mediate.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Extensions.AspNetCore.Queue
{
    public interface IEventQueue
    {
        bool HasEvents();

        void EnqueueEvent(QueuedEventWrapperBase @event);

        Task<QueuedEventWrapperBase> DequeueEvent();
        
        Task<QueuedEventWrapperBase> DequeueEvent(CancellationToken cancellationToken);

    }
}
