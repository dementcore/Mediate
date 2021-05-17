// File: DefaultEventQueueExceptionHandler.cs
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

using Mediate.BackgroundEventDispatch.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Mediate.BackgroundEventDispatch
{
    /// <summary>
    /// Default event dispatch exception handler that logs the exceptions
    /// </summary>
    public sealed class DefaultEventQueueExceptionHandler : IEventQueueExceptionHandler
    {
        private readonly ILogger<DefaultEventQueueExceptionHandler> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        public DefaultEventQueueExceptionHandler(ILogger<DefaultEventQueueExceptionHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles event dispatch exception
        /// </summary>
        /// <param name="aggregateException">Aggregate exception with all handlers errors</param>
        /// <param name="eventName">Name of the event</param>
        /// <returns></returns>
        public Task Handle(AggregateException aggregateException, string eventName)
        {
            _logger.LogError(aggregateException, $"Errors occurred executing event {eventName}");

            return Task.CompletedTask;
        }
    }
}
