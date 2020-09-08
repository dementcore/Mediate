using Mediate.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Samples.AspNetCore.Mediate
{
    public class OnHomeInvokedEventHandler : IEventHandler<OnHomeInvoked>
    {
        private readonly ILogger<OnHomeInvokedEventHandler> _logger;

        public OnHomeInvokedEventHandler(ILogger<OnHomeInvokedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(OnHomeInvoked @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"OnHomeInvoked Event handler before delay {@event.RequestId}");

            await Task.Delay(5000);
            _logger.LogInformation($"OnHomeInvoked Event handler after delay{@event.RequestId}");
           
        }
    }

    public class OnHomeInvokedEventHandler2 : IEventHandler<OnHomeInvoked>
    {
        private readonly ILogger<OnHomeInvokedEventHandler> _logger;

        public OnHomeInvokedEventHandler2(ILogger<OnHomeInvokedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(OnHomeInvoked @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"OnHomeInvoked Event handler 2 before delay {@event.RequestId}");

            await Task.Delay(5000);
            _logger.LogInformation($"OnHomeInvoked Event handler 2 after delay{@event.RequestId}");

        }
    }
}
