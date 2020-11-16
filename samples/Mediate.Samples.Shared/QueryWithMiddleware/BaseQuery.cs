using Mediate.Abstractions;
using System;

namespace Mediate.Samples.Shared.QueryWithMiddleware
{
    public class BaseQuery<TResult>:IQuery<TResult>
    {
        public Guid QueryId { get; }

        public BaseQuery()
        {
            QueryId = Guid.NewGuid();
        }
    }
}
