using Mediate.Core.Abstractions;
using Mediate.Samples.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Samples.AspNetCore.Autofac
{
    public class OnHomeInvokedEventHandler : IEventHandler<OnHomeInvoked>
    {
        private readonly ILogger<OnHomeInvokedEventHandler> _logger;
        private readonly IHubContext<Hubs.TestHub> _hubContext;

        public OnHomeInvokedEventHandler(ILogger<OnHomeInvokedEventHandler> logger, IHubContext<Hubs.TestHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task Handle(OnHomeInvoked @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"OnHomeInvoked Event handler before delay {@event.RequestId}");

            await Task.Delay(5000);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"OnHomeInvoked after 5 seconds delay {@event.RequestId}");

            _logger.LogInformation($"OnHomeInvoked Event handler after delay{@event.RequestId}");
           
        }
    }

    public class OnHomeInvokedEventHandler2 : IEventHandler<OnHomeInvoked>
    {
        private readonly ILogger<OnHomeInvokedEventHandler2> _logger;
        private readonly IHubContext<Hubs.TestHub> _hubContext;

        public OnHomeInvokedEventHandler2(ILogger<OnHomeInvokedEventHandler2> logger, IHubContext<Hubs.TestHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task Handle(OnHomeInvoked @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"OnHomeInvoked Event handler 2 before delay {@event.RequestId}");

            await Task.Delay(5000);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"OnHomeInvoked handler 2 after 5 seconds delay {@event.RequestId}");

            _logger.LogInformation($"OnHomeInvoked Event handler 2 after delay{@event.RequestId}");

        }
    }
}
