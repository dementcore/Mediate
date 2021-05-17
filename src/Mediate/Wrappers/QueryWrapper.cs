// File: QueryWrapper.cs
// The MIT License
//
// Copyright (c) 2021 DementCore
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//

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
