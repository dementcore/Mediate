using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Abstractions
{
    
    /// <summary>
    /// Delegate to call next middleware in the mediate pipeline
    /// </summary>
    /// <returns></returns>
    public delegate Task NextMiddlewareDelegate();

    /// <summary>
    /// Delegate to call next middleware in the mediate pipeline
    /// </summary>
    /// <typeparam name="TResult">Query response type</typeparam>
    /// <returns></returns>
    public delegate Task<TResult> NextMiddlewareDelegate<TResult>();
}
