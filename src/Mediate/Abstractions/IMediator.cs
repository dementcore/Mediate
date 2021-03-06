﻿// File: IMediator.cs
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
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Abstractions
{
    /// <summary>
    /// Mediator interface
    /// </summary>
    public interface IMediator
    {
        /// <summary>
        /// Sends a query to the mediator
        /// </summary>
        /// <typeparam name="TResult">Query response type</typeparam>
        /// <param name="query">Query data</param>
        /// <returns>Query response</returns>
        Task<TResult> Send<TResult>(IQuery<TResult> query);

        /// <summary>
        /// Sends a query to the mediator
        /// </summary>
        /// <typeparam name="TResult">Query response type</typeparam>
        /// <param name="query">Query data</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Query response</returns>
        Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);

        /// <summary>
        /// Dispatchs an event to the mediator
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="event">Event data</param>
        /// <returns></returns>
        Task Dispatch<TEvent>(TEvent @event) where TEvent : IEvent;

        /// <summary>
        /// Dispatchs an event to the mediator
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="event">Event data</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Dispatch<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : IEvent;
    }
}
