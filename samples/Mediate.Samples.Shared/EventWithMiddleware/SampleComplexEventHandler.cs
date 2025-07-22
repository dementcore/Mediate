// File: SampleComplexEventHandler.cs
// The MIT License
//
// Copyright (c) 2021 DementCore
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//

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
            _logger.LogInformation("Received SampleComplexEvent: {0}", @event);
            _logger.LogInformation("Waiting 5 seconds");

            await Task.Delay(5000);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"Hi {@event.EventData}!!!");
        }
    }
}
