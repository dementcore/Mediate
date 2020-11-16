using Mediate.Abstractions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Samples.Shared.EventWithMiddleware
{
    public class SampleComplexEventHandler : IEventHandler<SampleComplexEvent>
    {
        private readonly ILogger<SampleComplexEventHandler> _logger;
        private readonly IHubContext<Hubs.SignalRSampleHub> _hubContext;

        public SampleComplexEventHandler(ILogger<SampleComplexEventHandler> logger, IHubContext<Hubs.SignalRSampleHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task Handle(SampleComplexEvent @event, CancellationToken cancellationToken)
        {
            await Task.Delay(5000);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"Hi {@event.EventData}!!!");
        }
    }
}
