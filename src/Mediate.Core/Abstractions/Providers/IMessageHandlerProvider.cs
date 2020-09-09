using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Core.Abstractions
{
    public interface IMessageHandlerProvider
    {
        Task<IMessageHandler<TMessage, TResult>> GetMessageHandler<TMessage, TResult>(TMessage message) where TMessage : IMessage<TResult>;
    }
}
