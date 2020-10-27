using System;
using System.Collections.Generic;
using System.Text;

namespace Mediate.Core.Abstractions
{
    /// <summary>
    /// Marker interface for implement a message with a response
    /// </summary>
    /// <typeparam name="TResult">Message response type</typeparam>
    public interface IMessage<out TResult>
    {
    }
}
