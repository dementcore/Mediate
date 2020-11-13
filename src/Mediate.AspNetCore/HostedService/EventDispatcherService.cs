using Mediate.AspNetCore.Queue;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.AspNetCore.HostedService
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

        public EventDispatcherService(ILogger<EventDispatcherService> logger, EventQueue backgroundEventQueue)
        {
            _backgroundEventQueue = backgroundEventQueue;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("BackgroundEventExecutionService execution started");

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BackgroundEventExecutionService execution running");

            await Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {

                    if (!_backgroundEventQueue.HasEvents())
                    {
                        continue;
                    }

                    QueuedEventWrapperBase job = await _backgroundEventQueue.DequeueEvent(stoppingToken);

                    try
                    {
                        await job.Handle(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error occurred executing event {job.EventName}");
                    }
                }
            }, stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("BackgroundEventExecutionService stoped");

            return base.StopAsync(cancellationToken);
        }
    }
}
