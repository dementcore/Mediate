using System;
using System.Collections.Generic;
using System.Text;

namespace Mediate.Abstractions
{
    public enum DispatchPolicy
    {
        /// <summary>
        /// Executes the event handlers inmediately
        /// </summary>
        Inmediate,
        /// <summary>
        /// Queues the event to be executed in a background hosted service
        /// </summary>
        Queued
    }
}
