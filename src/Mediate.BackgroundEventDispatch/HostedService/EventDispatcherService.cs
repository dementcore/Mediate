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
