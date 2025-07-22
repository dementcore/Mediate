// File: GenericEventHandler.cs
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
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Samples.Shared.Event
{
    /// <summary>
    /// This class catchs all events
    /// </summary>
    public class GenericEventHandler<T> : IEventHandler<T> where T : IEvent
    {
        private readonly ILogger<GenericEventHandler<T>> _logger;
        public GenericEventHandler(ILogger<GenericEventHandler<T>> logger)
        {
            _logger = logger;
        }

        public Task Handle(T @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received event: {0}", @event.GetType().ToString());

            return Task.CompletedTask;
        }
    }
}
