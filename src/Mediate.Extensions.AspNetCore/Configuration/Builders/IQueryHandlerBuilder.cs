using Mediate.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Extensions.AspNetCore.Configuration.Builders
{
    public interface IQueryHandlerBuilder<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        IQueryHandlerBuilder<TQuery, TResult> AddHandler<TQueryHandler>() where TQueryHandler : IQueryHandler<TQuery, TResult>;
    }
}
