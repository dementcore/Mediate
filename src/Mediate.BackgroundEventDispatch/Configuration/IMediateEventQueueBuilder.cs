using Mediate.BackgroundEventDispatch.Abstractions;

namespace Mediate.BackgroundEventDispatch.Configuration
{
    /// <summary>
    /// Helper methods to configure Mediate
    /// </summary>
    public interface IMediateEventQueueBuilder
    {
        /// <summary>
        /// Registers the DefaultEventQueueExceptionHandler that logs the error.
        /// </summary>
        /// <returns></returns>
        IMediateEventQueueBuilder AddDefaultExceptionHandler();


        /// <summary>
        /// Registers a custom EventQueueExceptionHandler
        /// </summary>
        /// <typeparam name="TExceptionHandler">EventQueueExceptionHandler implementation type</typeparam>
        /// <returns></returns>
        IMediateEventQueueBuilder AddCustomExceptionHandler<TExceptionHandler>()
                    where TExceptionHandler : IEventQueueExceptionHandler;

    }
}
