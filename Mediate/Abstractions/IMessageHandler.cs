using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Abstractions
{
    public interface IMessageHandler<in TMessage, TResult>
        where TMessage : IMessage<TResult>
    {
        Task<TResult> Handle(TMessage message, CancellationToken cancellationToken);

    }
}
