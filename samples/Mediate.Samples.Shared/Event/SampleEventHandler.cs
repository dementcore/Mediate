using Mediate.Abstractions;
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
            await Task.Delay(5000);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"Hi {@event.EventData}!!!");
        }
    }
}
