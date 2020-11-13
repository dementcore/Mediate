using Mediate.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.AspNetCore.Configuration.Builders
{
    public sealed class QueryHandlerBuilder<TQuery, TResult> : IQueryHandlerBuilder<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private IServiceCollection _services;

        public QueryHandlerBuilder(IServiceCollection services)
        {
            _services = services;
        }

        IQueryHandlerBuilder<TQuery, TResult> IQueryHandlerBuilder<TQuery, TResult>.AddHandler<TQueryHandler>()
        {
            if (_services.Any(s => s.ServiceType == typeof(IQueryHandler<TQuery, TResult>)))
            {
                throw new InvalidOperationException("Duplicate query handler found. You can't register multiple query handler for a concrete query.");
            }

            Type serviceType = typeof(IQueryHandler<TQuery, TResult>);

            _services.AddTransient(serviceType, typeof(TQueryHandler));

            return this;
        }
    }
}
