using Mediate.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Configuration.Builders
{
    public interface IQueryHandlerBuilder<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        IQueryHandlerBuilder<TQuery, TResult> AddHandler<TQueryHandler>() where TQueryHandler : IQueryHandler<TQuery, TResult>;
        IQueryHandlerBuilder<TQuery, TResult> AddMiddleware<TQueryMiddleware>() where TQueryMiddleware : IQueryMiddleware<TQuery, TResult>;
    }
}
