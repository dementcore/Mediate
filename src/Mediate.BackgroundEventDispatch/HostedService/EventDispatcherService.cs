// File: EventDispatcherService.cs
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
using Mediate.BackgroundEventDispatch.Queue;
using Mediate.BackgroundEventDispatch.Wrappers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.BackgroundEventDispatch.HostedService
{
    /// <summary>
    /// Job to handle the enqueued events. <br/>
    /// This class is public to allow registration into DI containers like Autofac, Unity, etc. <br/>
    /// This shouldn't be used directly from user code. <br/>
    /// </summary>
    public sealed class EventDispatcherService : BackgroundService
    {
        private readonly EventQueue _backgroundEventQueue;
        private readonly ILogger<EventDispatcherService> _logger;
        private readonly IEventQueueExceptionHandler _exceptionHandler;

        /// <summary>
        /// Event dispatcher service constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="backgroundEventQueue"></param>
        /// <param name="exceptionHandler"></param>
        public EventDispatcherService(ILogger<EventDispatcherService> logger, EventQueue backgroundEventQueue, IEventQueueExceptionHandler exceptionHandler)
        {
            _backgroundEventQueue = backgroundEventQueue;
            _logger = logger;
            _exceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// Hosted service execute method
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {

                    if (!await _backgroundEventQueue.HasEvents(stoppingToken))
                    {
                        continue;
                    }

                    QueuedEventWrapperBase job = await _backgroundEventQueue.DequeueEvent(stoppingToken);

                    try
                    {
                        await job.Handle(stoppingToken);
                    }
                    catch (AggregateException ex)
                    {
                        await _exceptionHandler.Handle(ex, job.EventName);
                    }
                }
            }, stoppingToken);
        }

        /// <summary>
        /// Hosted service stop method
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
