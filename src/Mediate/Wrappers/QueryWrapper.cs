using Mediate.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Wrappers
{

    internal abstract class QueryWrapperBase
    {
        internal abstract Task<object> Handle(object query, CancellationToken cancellationToken);
    }

    internal sealed class QueryWrapper<TQuery, TResult> : QueryWrapperBase
        where TQuery : IQuery<TResult>
    {

        private readonly IQueryHandlerProvider _queryHandlerProvider;

        private readonly IQueryMiddlewareProvider _queryMiddlewareProvider;

        public QueryWrapper(IQueryHandlerProvider queryHandlerProvider, IQueryMiddlewareProvider queryMiddlewareProvider)
        {
            _queryHandlerProvider = queryHandlerProvider;
            _queryMiddlewareProvider = queryMiddlewareProvider;
        }

        internal override async Task<object> Handle(object query, CancellationToken cancellationToken)
        {
            return await Handle((IQuery<TResult>)query, cancellationToken);
        }

        internal async Task<TResult> Handle(IQuery<TResult> query, CancellationToken cancellationToken)
        {
            IQueryHandler<TQuery, TResult> handler = await _queryHandlerProvider.GetHandler<TQuery, TResult>();

            if (handler == null)
            {
                throw new InvalidOperationException($"There isn't any registered query handler for {typeof(TQuery).Name}");
            }

            IEnumerable<IQueryMiddleware<TQuery, TResult>> middlewares = await _queryMiddlewareProvider.GetMiddlewares<TQuery, TResult>();

            async Task<TResult> pipelineEnd()
            {
                return await handler.Handle((TQuery)query, cancellationToken);
            }

            NextMiddlewareDelegate<TResult> pipeline = middlewares
                .Reverse()
                .Aggregate((NextMiddlewareDelegate<TResult>)pipelineEnd, (next, middleware) =>
                {
                    return async delegate
                    {
                        return await middleware.Invoke((TQuery)query, cancellationToken, next);
                    };
                });

            return await pipeline();

        }

    }
}
