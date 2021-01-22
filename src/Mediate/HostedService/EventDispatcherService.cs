using Mediate.Queue;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.HostedService
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

        /// <summary>
        /// Event dispatcher service constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="backgroundEventQueue"></param>
        public EventDispatcherService(ILogger<EventDispatcherService> logger, EventQueue backgroundEventQueue)
        {
            _backgroundEventQueue = backgroundEventQueue;
            _logger = logger;
        }

        /// <summary>
        /// Hosted service start method
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("BackgroundEventExecutionService execution started");

            return base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// Hosted service execute method
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BackgroundEventExecutionService execution running");

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
                        _logger.LogError(ex, $"Errors occurred executing event {job.EventName}");
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
            _logger.LogInformation("BackgroundEventExecutionService stoped");

            return base.StopAsync(cancellationToken);
        }
    }
}
