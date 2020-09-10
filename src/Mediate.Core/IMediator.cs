using Mediate.Core.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Core
{
    public interface IMediator
    {
        Task<TResult> Send<TMessage, TResult>(TMessage message)
            where TMessage : IMessage<TResult>;
        
        Task<TResult> Send<TMessage, TResult>(TMessage message, CancellationToken cancellationToken)
            where TMessage : IMessage<TResult>;

        Task Dispatch<TEvent>(TEvent @event) where TEvent : IEvent;

        Task Dispatch<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : IEvent;
    }
}
