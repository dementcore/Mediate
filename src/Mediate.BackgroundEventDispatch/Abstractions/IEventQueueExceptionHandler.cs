using System;
using System.Threading.Tasks;

namespace Mediate.BackgroundEventDispatch.Abstractions
{
    /// <summary>
    /// Interface for implement an exception handler for the event queue dispatch strategy
    /// </summary>
    public interface IEventQueueExceptionHandler
    {
        /// <summary>
        /// Handles event queue dispatch strategy
        /// </summary>
        /// <param name="aggregateException">Aggregate exception with all handlers errors</param>
        /// <param name="eventName">Name of the event</param>
        /// <returns></returns>
        Task Handle(AggregateException aggregateException, string eventName);
    }
}
