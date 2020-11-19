using Mediate.Abstractions;
using System;

namespace Mediate.Samples.Shared.QueryWithMiddleware
{
    public abstract class BaseQuery<TResult>:IQuery<TResult>
    {
        public Guid QueryId { get; }

        public BaseQuery()
        {
            QueryId = Guid.NewGuid();
        }
    }
}
