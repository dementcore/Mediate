using Mediate.Core.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Core
{
    public interface IMediator
    {
        Task<TResult> Send<TMessage, TResult>(TMessage message, CancellationToken cancellationToken = default)
            where TMessage : IMessage<TResult>;

        Task Dispatch<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent;
    }
}
