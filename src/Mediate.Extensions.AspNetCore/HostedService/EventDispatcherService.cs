using Mediate.Extensions.AspNetCore.Queue;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Extensions.AspNetCore.HostedService
{
    public sealed class EventDispatcherService : BackgroundService
    {
        private readonly IEventQueue _backgroundEventQueue;
        private readonly ILogger<EventDispatcherService> _logger;

        public EventDispatcherService(ILogger<EventDispatcherService> logger, IEventQueue backgroundEventQueue)
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
