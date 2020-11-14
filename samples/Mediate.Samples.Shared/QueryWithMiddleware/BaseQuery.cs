using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediate.Core.Abstractions;

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
