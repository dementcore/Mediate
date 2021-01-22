using Mediate.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediate
{
    /// <summary>
    /// Default event dispatch exception handler that logs the exceptions
    /// </summary>
    public sealed class DefaultEventQueueExceptionHandler : IEventQueueExceptionHandler
    {
        private readonly ILogger<DefaultEventQueueExceptionHandler> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        public DefaultEventQueueExceptionHandler(ILogger<DefaultEventQueueExceptionHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles event dispatch exception
        /// </summary>
        /// <param name="aggregateException">Aggregate exception with all handlers errors</param>
        /// <param name="eventName">Name of the event</param>
        /// <returns></returns>
        public Task Handle(AggregateException aggregateException, string eventName)
        {
            _logger.LogError(aggregateException, $"Errors occurred executing event {eventName}");

            return Task.CompletedTask;
        }
    }
}
