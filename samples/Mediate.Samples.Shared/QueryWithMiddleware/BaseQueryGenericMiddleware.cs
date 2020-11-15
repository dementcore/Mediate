using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mediate.Core.Abstractions;

namespace Mediate.Samples.Shared.QueryWithMiddleware
{

    public class BaseQueryGenericMiddleware<TQuery, TResult> : IQueryMiddleware<TQuery, TResult> where TQuery : BaseQuery<TResult>
    {
        public async Task<TResult> Invoke(TQuery query, CancellationToken cancellationToken, NextMiddlewareDelegate<TResult> next)
        {
            //example validation
            if (query.QueryId != Guid.Empty)
            {
                return await next();
            }

            //example exception for this example
            throw new InvalidOperationException("The query id must be not null");
        }
    }
}
