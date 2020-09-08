using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Abstractions
{
    internal abstract class MessageHandlerWrapper<TResult> /*: MessageHandlerWrapper*/
    {
        protected IMessageHandlerProvider handlerProvider;

        public MessageHandlerWrapper(IMessageHandlerProvider eventHandlerProvider)
        {
            handlerProvider = eventHandlerProvider;
        }

        public abstract Task<TResult> Handle(IMessage<TResult> message, CancellationToken cancellationToken);
    }

    internal class MessageHandlerWrapper<TMessage, TResult> : MessageHandlerWrapper<TResult> where TMessage : IMessage<TResult>
    {
        public MessageHandlerWrapper(IMessageHandlerProvider eventHandlerProvider) : base(eventHandlerProvider)
        {
        }

        public override async Task<TResult> Handle(IMessage<TResult> message, CancellationToken cancellationToken)
        {
            IMessageHandler<TMessage, TResult> handler = await handlerProvider.GetMessageHandler<TMessage,TResult>(message);

            return await handler.Handle((TMessage)message, cancellationToken);
        }
    }
}
