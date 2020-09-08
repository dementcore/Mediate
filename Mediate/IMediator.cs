using Mediate.Abstractions;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate
{
    public interface IMediator
    {
        Task<TResult> Send<TResult>(IMessage<TResult> message, CancellationToken cancellationToken = default);

        Task Dispatch(IEvent @event, CancellationToken cancellationToken = default);

        Task Dispatch(IEvent @event, DispatchPolicy executionPolicy, CancellationToken cancellationToken = default);
    }
}
