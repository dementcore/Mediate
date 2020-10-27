using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Core.Abstractions
{
    /// <summary>
    /// Interface for implement a message handler provider
    /// </summary>
    public interface IMessageHandlerProvider
    {
        /// <summary>
        /// Gets a message handler from a message
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <typeparam name="TResult">Response type</typeparam>
        /// <param name="message">Message data</param>
        /// <returns>Registered handler for that message</returns>
        Task<IMessageHandler<TMessage, TResult>> GetMessageHandler<TMessage, TResult>(TMessage message) where TMessage : IMessage<TResult>;
    }
}
