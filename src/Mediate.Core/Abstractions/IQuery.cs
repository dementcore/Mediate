using System;
using System.Collections.Generic;
using System.Text;

namespace Mediate.Core.Abstractions
{
    /// <summary>
    /// Marker interface for implement a query with a response
    /// </summary>
    /// <typeparam name="TResult">Query response type</typeparam>
    public interface IQuery<out TResult>
    {
    }
}
