using Mediate.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Mediate
{
    public class DefaultExceptionHandler : IExceptionHandler<Exception>
    {
        private readonly ILogger<DefaultExceptionHandler> _logger;
        public DefaultExceptionHandler(ILogger<DefaultExceptionHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(Exception exception)
        {
            Debug.Fail(exception.Message + " " + exception.StackTrace);
            _logger.Log(LogLevel.Error, exception, "Error: " + exception.Message);

            return Task.CompletedTask;
        }
    }
}
