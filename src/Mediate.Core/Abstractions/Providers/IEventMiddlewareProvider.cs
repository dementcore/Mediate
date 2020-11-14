using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Core.Abstractions
{
    public interface IEventMiddlewareProvider
    {
        Task<IEnumerable<IEventMiddleware<TEvent>>> GetMiddlewares<TEvent>() where TEvent : IEvent;
    }
}
