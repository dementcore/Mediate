﻿// File: IQueryMiddlewareProvider.cs
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Abstractions
{
    /// <summary>
    /// Interface for implement a query middleware provider
    /// </summary>
    public interface IQueryMiddlewareProvider
    {
        /// <summary>
        /// Gets all query middlewares from a query
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <typeparam name="TResult">Query response type</typeparam>
        /// <returns>All registered middlewares for that query</returns>
        Task<IEnumerable<IQueryMiddleware<TQuery, TResult>>> GetMiddlewares<TQuery, TResult>() where TQuery : IQuery<TResult>;
    }
}
