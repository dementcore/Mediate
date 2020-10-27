using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Core.Abstractions
{
    /// <summary>
    /// Interface for implement a message handler for a message
    /// </summary>
    /// <typeparam name="TMessage">Message type</typeparam>
    /// <typeparam name="TResult">Message response type</typeparam>
    public interface IMessageHandler<in TMessage, TResult>
        where TMessage : IMessage<TResult>
    {
        /// <summary>
        /// Handle the message
        /// </summary>
        /// <param name="message">Message data</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Message response</returns>
        Task<TResult> Handle(TMessage message, CancellationToken cancellationToken);

    }
}
