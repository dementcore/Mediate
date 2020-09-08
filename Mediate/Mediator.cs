using Mediate.Abstractions;
using Mediate.Queue;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate
{
    internal sealed class Mediator : IMediator
    {
        private readonly IMessageHandlerProvider _handlerProvider;
        private readonly IEventQueue _eventQueue;

        public Mediator(IMessageHandlerProvider handlerProvider, IEventQueue eventQueue)
        {
            _handlerProvider = handlerProvider;
            _eventQueue = eventQueue;
        }

        /// <summary>
        /// Dispatchs an event executing his handlers immediately
        /// </summary>
        /// <param name="event">Event info</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public async Task Dispatch(IEvent @event, CancellationToken cancellationToken = default)
        {
            await Dispatch(@event, DispatchPolicy.Inmediate, cancellationToken);
        }

        /// <summary>
        /// Dispatchs an event executing his handlers in defined way
        /// </summary>
        /// <param name="event">Event info</param>
        /// <param name="dispatchPolicy">Event dispatching </param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public async Task Dispatch(IEvent @event, DispatchPolicy dispatchPolicy, CancellationToken cancellationToken = default)
        {
            var eventHandler = (EventHandlerWrapper)Activator.CreateInstance(typeof(EventHandlerWrapper<>).MakeGenericType(@event.GetType()), _handlerProvider);

            switch (dispatchPolicy)
            {
                case DispatchPolicy.Inmediate:
                    await eventHandler.Handle(@event, cancellationToken);
                    break;

                case DispatchPolicy.Queued:
                    QueuedEvent queuedEvent = new QueuedEvent(eventHandler, @event);
                    _eventQueue.EnqueueEvent(queuedEvent);
                    break;
            }
        }

        public async Task<TResult> Send<TResult>(IMessage<TResult> message, CancellationToken cancellationToken = default)
        {
            var eventHandler = (MessageHandlerWrapper<TResult>)Activator.CreateInstance(
                typeof(MessageHandlerWrapper<,>).MakeGenericType(message.GetType(), typeof(TResult)), _handlerProvider);

           return await eventHandler.Handle(message, cancellationToken);
        }

    }
}
