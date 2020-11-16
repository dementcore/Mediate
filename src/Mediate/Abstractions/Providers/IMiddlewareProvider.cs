using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Abstractions
{
    /// <summary>
    /// Defines a provider that encapsulates event and query middlewares provider
    /// </summary>
    public interface IMiddlewareProvider : IEventMiddlewareProvider, IQueryMiddlewareProvider
    {
    }
}
