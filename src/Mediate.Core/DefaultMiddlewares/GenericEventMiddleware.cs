using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mediate.Core.Abstractions;

namespace Mediate.Core.DefaultMiddlewares
{
    public class GenericEventMiddleware : IEventMiddleware<IEvent>
    {
        public async Task Invoke(IEvent @event, CancellationToken cancellationToken, NextMiddlewareDelegate next)
        {
            Console.WriteLine("Hello from middleware 1");

            await next();
        }
    }

    public class GenericEventMiddleware2 : IEventMiddleware<IEvent>
    {
        public async Task Invoke(IEvent @event, CancellationToken cancellationToken, NextMiddlewareDelegate next)
        {
            Console.WriteLine("Hello from middleware 2");

            await next();
        }
    }


    public static class MockMiddlewares
    {
        public static IEnumerable<IEventMiddleware<IEvent>> middles = new List<IEventMiddleware<IEvent>>()
        {
            new GenericEventMiddleware(),
            new GenericEventMiddleware2()
        };
}
}
