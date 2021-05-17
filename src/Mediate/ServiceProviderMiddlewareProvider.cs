// File: ServiceProviderMiddlewareProvider.cs
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
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mediate
{
    /// <summary>
    /// Message and event middleware provider from IServiceProvider
    /// </summary>
    public sealed class ServiceProviderMiddlewareProvider : IMiddlewareProvider,IDisposable
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ServiceProviderMiddlewareProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// Gets all event middlewares from an event from the IServiceProvider
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <returns>All registered middlewares for that event</returns>
        public Task<IEnumerable<IEventMiddleware<TEvent>>> GetMiddlewares<TEvent>() where TEvent : IEvent
        {
            var services = _serviceProvider.GetServices<IEventMiddleware<TEvent>>();

            IEnumerable<IEventMiddleware<TEvent>> handlers = new List<IEventMiddleware<TEvent>>();

            if (services is IEnumerable<IEventMiddleware<TEvent>>)
            {
                handlers = services;
            }

            return Task.FromResult(handlers);
        }

        /// <summary>
        /// Gets all query middlewares from a query from the IServiceProvider
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <typeparam name="TResult">Query response type</typeparam>
        /// <returns>All registered middlewares for that query</returns>
        public Task<IEnumerable<IQueryMiddleware<TQuery, TResult>>> GetMiddlewares<TQuery, TResult>() where TQuery : IQuery<TResult>
        {
            var services = _serviceProvider.GetServices<IQueryMiddleware<TQuery, TResult>>();

            IEnumerable<IQueryMiddleware<TQuery, TResult>> handlers = new List<IQueryMiddleware<TQuery, TResult>>();

            if (services is IEnumerable<IQueryMiddleware<TQuery, TResult>>)
            {
                handlers = services;
            }

            return Task.FromResult(handlers);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
