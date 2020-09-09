using Mediate.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Core
{
    public sealed class Mediator : IMediator
    {
        private readonly IMessageHandlerProvider _messageHandlerProvider;
        private readonly IEventHandlerProvider _eventHandlerProvider;
        private readonly IEventDispatchStrategy _eventDispatchStrategy;

        public Mediator(IMessageHandlerProvider messageHandlerProvider, IEventHandlerProvider eventHandlerProvider,
            IEventDispatchStrategy eventDispatchStrategy)
        {
            _messageHandlerProvider = messageHandlerProvider;
            _eventHandlerProvider = eventHandlerProvider;
            _eventDispatchStrategy = eventDispatchStrategy;
        }

        /// <summary>
        /// Dispatchs an event into configured dispatch strategy
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="event">Event data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public async Task Dispatch<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : IEvent
        {
            IEnumerable<IEventHandler<TEvent>> handlers = await _eventHandlerProvider.GetEventHandlers<TEvent>(@event);
            await _eventDispatchStrategy.Dispatch(@event, handlers, cancellationToken);
        }

        /// <summary>
        /// Sends a message to his handler and returns a response
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <typeparam name="TResult">Response type</typeparam>
        /// <param name="message">Message data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public async Task<TResult> Send<TMessage, TResult>(TMessage message, CancellationToken cancellationToken = default)
            where TMessage : IMessage<TResult>
        {
            var handler = await _messageHandlerProvider.GetMessageHandler<TMessage, TResult>(message);

            return await handler.Handle(message, cancellationToken);
        }

    }
}
