// File: BaseEventGenericHandler.cs
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

namespace Mediate.Samples.Shared.EventWithMiddleware
{
    /// <summary>
    /// This class catchs all BaseEvent derived events
    /// </summary>
    public class BaseEventGenericHandler<T> : IEventHandler<T> where T : BaseEvent
    {
        private readonly ILogger<BaseEventGenericHandler<T>> _logger;
        public BaseEventGenericHandler(ILogger<BaseEventGenericHandler<T>> logger)
        {
            _logger = logger;
        }

        public Task Handle(T @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received base event derived event: {0}", @event);

            return Task.CompletedTask;
        }
    }
}
