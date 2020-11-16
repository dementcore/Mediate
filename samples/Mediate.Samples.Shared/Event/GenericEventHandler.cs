using Mediate.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Samples.Shared.Event
{
    /// <summary>
    /// This class catchs all events
    /// </summary>
    public class GenericEventHandler<T> : IEventHandler<T> where T : IEvent
    {
        private readonly ILogger<GenericEventHandler<T>> _logger;
        public GenericEventHandler(ILogger<GenericEventHandler<T>> logger)
        {
            _logger = logger;
        }

        public Task Handle(T @event, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Received event: ", @event);

            return Task.CompletedTask;
        }
    }
}
