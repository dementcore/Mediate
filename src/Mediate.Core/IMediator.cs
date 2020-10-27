using Mediate.Core.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Core
{
    /// <summary>
    /// Mediator interface
    /// </summary>
    public interface IMediator
    {
        /// <summary>
        /// Sends a message to the mediator
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <typeparam name="TResult">Message response type</typeparam>
        /// <param name="message">Message data</param>
        /// <returns>Message response</returns>
        Task<TResult> Send<TMessage, TResult>(TMessage message)
            where TMessage : IMessage<TResult>;

        /// <summary>
        /// Sends a message to the mediator
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <typeparam name="TResult">Message response type</typeparam>
        /// <param name="message">Message data</param>
        /// <returns>Message response</returns>
        Task<TResult> Send<TMessage, TResult>(TMessage message, CancellationToken cancellationToken)
            where TMessage : IMessage<TResult>;

        /// <summary>
        /// Dispatchs an event to the mediator
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="event">Event data</param>
        /// <returns></returns>
        Task Dispatch<TEvent>(TEvent @event) where TEvent : IEvent;

        /// <summary>
        /// Dispatchs an event to the mediator
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="event">Event data</param>
        /// <returns></returns>
        Task Dispatch<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : IEvent;
    }
}
