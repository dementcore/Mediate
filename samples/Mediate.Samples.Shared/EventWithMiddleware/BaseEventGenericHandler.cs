using Mediate.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Samples.Shared.EventWithMiddleware
{
    /// <summary>
    /// This class catchs all BaseEvent derived events
    /// </summary>
    public class BaseEventGenericHandler<T> : IEventHandler<T> where T : BaseEvent
    {
        private readonly ILogger<BaseEventGenericHandler<T>> _logger;
        public BaseEventGenericHandler(ILogger<BaseEventGenericHandler<T>> logger)
        {
            _logger = logger;
        }

        public Task Handle(T @event, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Received base event derived event: ", @event);

            return Task.CompletedTask;
        }
    }
}
