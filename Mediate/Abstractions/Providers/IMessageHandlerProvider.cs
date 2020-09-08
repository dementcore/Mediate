using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Abstractions
{
    public interface IMessageHandlerProvider
    {
        Task<IMessageHandler<TMessage, TResult>> GetMessageHandler<TMessage, TResult>(IMessage<TResult> message) where TMessage : IMessage<TResult>;
    }
}
