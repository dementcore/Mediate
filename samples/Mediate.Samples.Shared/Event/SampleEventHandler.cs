﻿using Mediate.Core.Abstractions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Samples.Shared.Event
{
    public class SampleEventHandler : IEventHandler<SampleEvent>
    {
        private readonly ILogger<SampleEventHandler> _logger;
        private readonly IHubContext<Hubs.SignalRSampleHub> _hubContext;

        public SampleEventHandler(ILogger<SampleEventHandler> logger, IHubContext<Hubs.SignalRSampleHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task Handle(SampleEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"OnHomeInvoked Event handler before delay {@event.EventData}");

            await Task.Delay(5000);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"OnHomeInvoked after 5 seconds delay {@event.EventData}");

            _logger.LogInformation($"OnHomeInvoked Event handler after delay{@event.EventData}");
        }
    }
}
