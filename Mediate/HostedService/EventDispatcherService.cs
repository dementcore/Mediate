using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mediate.Abstractions;
using Mediate.Queue;

namespace Mediate.AspNetCore
{
    internal class EventDispatcherService : BackgroundService
    {
        private readonly IEventQueue _backgroundEventQueue;
        private readonly ILogger<EventDispatcherService> _logger;
        private readonly IExceptionHandlerProvider _exceptionHandlerProvider;

        public EventDispatcherService(ILogger<EventDispatcherService> logger, IEventQueue backgroundEventQueue,
            IExceptionHandlerProvider exceptionHandlerProvider)
        {
            _backgroundEventQueue = backgroundEventQueue;
            _logger = logger;
            _exceptionHandlerProvider = exceptionHandlerProvider;
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

                    QueuedEvent job = await _backgroundEventQueue.DequeueEvent(stoppingToken);

                    try
                    {
                        await job.EventHandler.Handle(job.Event, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        IExceptionHandler<Exception> exceptionHandler = await _exceptionHandlerProvider.GetHandler(ex);

                        await exceptionHandler.Handle(ex);

                        _logger.LogError(ex, "Error occurred executing event");
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
